using TC1.RepairShop.Application.Auth;
using TC1.RepairShop.Domain.Auth;

namespace TC1.RepairShop.IntegrationTests;

public class FakeUserRepository : IUserRepository
{
    public static readonly User SeedAdmin = new()
    {
        Id = Guid.NewGuid(),
        Username = "admin",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
        Role = "Admin",
    };

    public Task<User?> GetByUsernameAsync(string username)
    {
        var user = username == SeedAdmin.Username ? SeedAdmin : null;
        return Task.FromResult(user);
    }
}
