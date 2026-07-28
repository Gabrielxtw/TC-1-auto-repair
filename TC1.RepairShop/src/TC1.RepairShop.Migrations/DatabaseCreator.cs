using System.Data.Common;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;

namespace TC1.RepairShop.Migrations;

public static class DatabaseCreator
{
    private static readonly Regex InitialCatalogRegex = new(
        @"(Initial Catalog|Database)=(?<database>.*?);",
        RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static void CreateIfNotExists(string connectionString)
    {
        var databaseName = GetDatabaseName(connectionString);
        var masterConnectionString = new SqlConnectionStringBuilder(connectionString) { InitialCatalog = "master" }.ConnectionString;

        using var connection = new SqlConnection(masterConnectionString);
        connection.Open();

        using var checkCommand = connection.CreateCommand();
        checkCommand.CommandText = "SELECT 1 FROM sys.databases WHERE name = @name";
        checkCommand.Parameters.Add(new SqlParameter("@name", databaseName));

        using (var reader = (DbDataReader)checkCommand.ExecuteReader())
        {
            if (reader.HasRows)
            {
                return;
            }
        }

        using var createCommand = connection.CreateCommand();
        createCommand.CommandText = $"CREATE DATABASE [{databaseName}]";
        createCommand.ExecuteNonQuery();
    }

    public static string GetDatabaseName(string connectionString)
    {
        var match = InitialCatalogRegex.Match(connectionString);
        if (!match.Success)
        {
            throw new InvalidOperationException(
                "Could not determine the database name from the connection string. Expected 'Initial Catalog=...;' or 'Database=...;'.");
        }

        return match.Groups["database"].Value;
    }
}
