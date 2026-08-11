using System.Collections.Concurrent;
using TC1.RepairShop.Application.Clients;
using TC1.RepairShop.Domain.Clients;
using TC1.RepairShop.Domain.Common;

namespace TC1.RepairShop.IntegrationTests;

public class FakeUserRepository : IUserRepository
{
    public static readonly User SeedAdmin = User.Create("admin", "Admin@123", Role.Admin);

    private static readonly ConcurrentDictionary<Guid, User> Users = new([
        new KeyValuePair<Guid, User>(SeedAdmin.Id, SeedAdmin),
    ]);

    public Task<User?> GetByUsernameAsync(string username)
    {
        var user = Users.Values.SingleOrDefault(u => u.Username == username && u.Status != Status.Deleted);
        return Task.FromResult(user);
    }

    public Task<User?> GetByIdAsync(Guid id)
    {
        Users.TryGetValue(id, out var user);
        return Task.FromResult(user is not null && user.Status != Status.Deleted ? user : null);
    }

    public Task<IEnumerable<User>> GetAllAsync()
    {
        return Task.FromResult(Users.Values.Where(u => u.Status != Status.Deleted));
    }

    public Task AddAsync(User user)
    {
        Users[user.Id] = user;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(User user)
    {
        Users[user.Id] = user;
        return Task.CompletedTask;
    }
}
