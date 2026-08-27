using System;
using System.Collections.Concurrent;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.IntegrationTests;

public class FakeUserRepository : IUserRepository
{
    private readonly ConcurrentDictionary<Guid, User> Users = new();

    public Task<User?> GetByUsernameAsync(string username)
    {
        var user = Users.Values.SingleOrDefault(u => u.Username == username && u.Status != Status.Deleted);
        return Task.FromResult(user);
    }

    public Task<IEnumerable<User>> GetAllAsync()
    {
        return Task.FromResult(Users.Values.Where(u => u.Status != Status.Deleted).AsEnumerable());
    }

    public Task<User?> GetByIdAsync(Guid id)
    {
        Users.TryGetValue(id, out var user);
        return Task.FromResult(user is not null && user.Status != Status.Deleted ? user : null);
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

    public Task Add(User user)
    {
        Users[user.Id] = user;
        return Task.CompletedTask;
    }

    public Task Update(User user)
    {
        Users[user.Id] = user;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        if (Users.TryGetValue(id, out var user))
        {
            user.Delete();
            Users[id] = user;
        }
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync()
    {
        return Task.CompletedTask;
    }
    public Task<bool> ExistsAsync(Guid id)
    {
        return Task.FromResult(Users.TryGetValue(id, out var user) && user.Status != Status.Deleted);
    }

    public Task<User?> GetByDocumentAsync(string document)
    {
        var user = Users.Values.SingleOrDefault(u => u.Document.Value == document && u.Status != Status.Deleted);
        return Task.FromResult(user);
    }
}
