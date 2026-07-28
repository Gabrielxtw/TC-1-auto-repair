using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

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

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(seedPassword);

        using var insertCommand = connection.CreateCommand();
        insertCommand.CommandText = """
            INSERT INTO Users (Id, Username, PasswordHash, Role)
            VALUES (@Id, @Username, @PasswordHash, @Role)
            """;
        insertCommand.Parameters.AddWithValue("@Id", Guid.NewGuid());
        insertCommand.Parameters.AddWithValue("@Username", SeedUsername);
        insertCommand.Parameters.AddWithValue("@PasswordHash", passwordHash);
        insertCommand.Parameters.AddWithValue("@Role", "Admin");
        insertCommand.ExecuteNonQuery();

        Console.WriteLine("Seed admin user created.");
    }
}
