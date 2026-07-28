using System.CommandLine;
using System.Diagnostics;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using TC1.RepairShop.Migrations.Migrations;

namespace TC1.RepairShop.Migrations;

public class Program
{
    private const long MinTimestamp = 202607261000;

    public static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand("Applies TC1.RepairShop database migrations.");

        var connectionOption = new Option<string>("--connectionstring", "Name of the connection string to use.");
        connectionOption.AddAlias("-c");
        connectionOption.SetDefaultValue("Default");
        rootCommand.AddOption(connectionOption);

        var downOption = new Option<long?>("--down", $"Migrate down to the specified version (minimum {MinTimestamp}).");
        downOption.AddAlias("-d");
        rootCommand.AddOption(downOption);

        rootCommand.SetHandler((connectionStringName, downVersion) =>
        {
            Log.Logger = CreateLogger();

            var configuration = LoadConfiguration();
            var connectionString = configuration.GetConnectionString(connectionStringName)
                ?? throw new InvalidOperationException($"Connection string '{connectionStringName}' was not found.");

            Log.Information("Ensuring database exists...");
            DatabaseCreator.CreateIfNotExists(connectionString);

            Log.Information("Running migrations...");
            var serviceProvider = CreateServices(connectionString);
            using var scope = serviceProvider.CreateScope();
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

            if (downVersion.HasValue)
            {
                var target = Math.Max(downVersion.Value, MinTimestamp);
                runner.MigrateDown(target);
            }
            else
            {
                runner.MigrateUp();

                Log.Information("Seeding initial data...");
                DatabaseSeeder.SeedAdminUser(configuration, connectionString);
            }

            Log.Information("Done.");
        }, connectionOption, downOption);

        return await rootCommand.InvokeAsync(args);
    }

    private static IConfiguration LoadConfiguration()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        var baseDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule!.FileName)!;

        return new ConfigurationBuilder()
            .SetBasePath(baseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();
    }

    private static IServiceProvider CreateServices(string connectionString) =>
        new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .WithGlobalCommandTimeout(TimeSpan.FromMinutes(5))
                .ScanIn(typeof(CreateUsers).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider(false);

    private static Serilog.Core.Logger CreateLogger() =>
        new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.Console(theme: SystemConsoleTheme.Literate)
            .CreateLogger();
}
