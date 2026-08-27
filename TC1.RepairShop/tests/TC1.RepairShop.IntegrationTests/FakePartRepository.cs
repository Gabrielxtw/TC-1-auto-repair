using System.Collections.Concurrent;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.IntegrationTests;

public class FakePartRepository : IPartRepository
{
    public static readonly ConcurrentDictionary<Guid, Part> Parts = new();

    public Task<bool> ExistsByNameAsync(string name)
    {
        var exists = Parts.Values.Any(p => p.Name == name && p.Status != Status.Deleted);
        return Task.FromResult(exists);
    }

    public Task<IEnumerable<Part>> GetAllAsync() =>
        Task.FromResult(Parts.Values.Where(p => p.Status != Status.Deleted));

    public Task<Part?> GetByIdAsync(Guid id)
    {
        Parts.TryGetValue(id, out var part);
        return Task.FromResult(part is not null && part.Status != Status.Deleted ? part : null);
    }

    public Task AddAsync(Part part)
    {
        Parts[part.Id] = part;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Part part)
    {
        Parts[part.Id] = part;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        if (Parts.TryGetValue(id, out var part))
            part.Delete();
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync() => Task.CompletedTask;
    public Task Add(Part part) => Task.CompletedTask;
    public Task Update(Part part) => Task.CompletedTask;


    public Task<bool> ExistsAsync(Guid id) =>
        Task.FromResult(Parts.TryGetValue(id, out var part) && part.Status != Status.Deleted);
}
