using System.Collections.Concurrent;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.IntegrationTests;

public class FakeServiceRepository : IServiceRepository
{
    public static readonly ConcurrentDictionary<Guid, Service> Services = new();

    public Task<bool> ExistsByNameAsync(string name)
    {
        var exists = Services.Values.Any(s => s.Name == name && s.Status != Status.Deleted);
        return Task.FromResult(exists);
    }

    public Task<IEnumerable<Service>> GetAllAsync() =>
        Task.FromResult(Services.Values.Where(s => s.Status != Status.Deleted));

    public Task<Service?> GetByIdAsync(Guid id)
    {
        Services.TryGetValue(id, out var service);
        return Task.FromResult(service is not null && service.Status != Status.Deleted ? service : null);
    }

    public Task AddAsync(Service service)
    {
        Services[service.Id] = service;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Service service)
    {
        Services[service.Id] = service;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        if (Services.TryGetValue(id, out var service))
            service.Delete();
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync() => Task.CompletedTask;

    public Task<bool> ExistsAsync(Guid id) =>
        Task.FromResult(Services.TryGetValue(id, out var service) && service.Status != Status.Deleted);
}
