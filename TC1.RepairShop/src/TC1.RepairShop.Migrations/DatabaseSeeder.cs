using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TC1.RepairShop.Domain.Clients;

namespace TC1.RepairShop.Migrations;

public static class DatabaseSeeder
{
    private const string SeedUsername = "admin";

    public static void SeedAdminUser(IConfiguration configuration, string connectionString)
    {
        var seedPassword = configuration["SeedAdmin:Password"]
            ?? Environment.GetEnvironmentVariable("SEED_ADMIN_PASSWORD")
            ?? throw new InvalidOperationException(
                "Set the SEED_ADMIN_PASSWORD environment variable (or SeedAdmin:Password) with the initial admin user's password.");

        using var connection = new SqlConnection(connectionString);
        connection.Open();

        using var checkCommand = connection.CreateCommand();
        checkCommand.CommandText = "SELECT COUNT(1) FROM Users WHERE Username = @Username";
        checkCommand.Parameters.AddWithValue("@Username", SeedUsername);

        var alreadyExists = (int)checkCommand.ExecuteScalar()! > 0;
        if (alreadyExists)
        {
            Console.WriteLine("Seed admin user already exists, skipping.");
            return;
        }

        var admin = User.Create(SeedUsername, seedPassword, "Admin");

        using var insertCommand = connection.CreateCommand();
        insertCommand.CommandText = """
            INSERT INTO Users (Id, Username, PasswordHash, Role, Status)
            VALUES (@Id, @Username, @PasswordHash, @Role, @Status)
            """;
        insertCommand.Parameters.AddWithValue("@Id", admin.Id);
        insertCommand.Parameters.AddWithValue("@Username", admin.Username);
        insertCommand.Parameters.AddWithValue("@PasswordHash", admin.PasswordHash);
        insertCommand.Parameters.AddWithValue("@Role", admin.Role);
        insertCommand.Parameters.AddWithValue("@Status", admin.Status.ToString());
        insertCommand.ExecuteNonQuery();

        Console.WriteLine("Seed admin user created.");
    }
}
