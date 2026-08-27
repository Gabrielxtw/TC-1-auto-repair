using System.Collections.Concurrent;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.IntegrationTests;

public class FakeServiceOrderPartRepository : IServiceOrderPartRepository
{
    public static readonly ConcurrentDictionary<Guid, ServiceOrderPart> ServiceOrderParts = new();

    public Task<IEnumerable<ServiceOrderPart>> GetByServiceOrderIdAsync(Guid serviceOrderId) =>
        Task.FromResult(ServiceOrderParts.Values.Where(p => p.ServiceOrderId == serviceOrderId));

    public Task<ServiceOrderPart?> GetByServiceOrderAndPartIdAsync(Guid serviceOrderId, Guid partId) =>
        Task.FromResult(ServiceOrderParts.Values.FirstOrDefault(p => p.ServiceOrderId == serviceOrderId && p.PartId == partId));

    public Task<IEnumerable<ServiceOrderPart>> GetAllAsync() =>
        Task.FromResult(ServiceOrderParts.Values.AsEnumerable());

    public Task<ServiceOrderPart?> GetByIdAsync(Guid id)
    {
        ServiceOrderParts.TryGetValue(id, out var part);
        return Task.FromResult(part);
    }

    public Task AddAsync(ServiceOrderPart part)
    {
        ServiceOrderParts[part.Id] = part;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(ServiceOrderPart part)
    {
        ServiceOrderParts[part.Id] = part;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        ServiceOrderParts.TryRemove(id, out _);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync() => Task.CompletedTask;
    public Task Add(ServiceOrderPart part)
    {
        ServiceOrderParts[part.Id] = part;
        return Task.CompletedTask;
    }

    public Task Update(ServiceOrderPart part)
    {
        ServiceOrderParts[part.Id] = part;
        return Task.CompletedTask;
    }

    public Task<IEnumerable<ServiceOrderPart>> GetByPartIdAsync(Guid partId) =>
        Task.FromResult(ServiceOrderParts.Values.Where(p => p.PartId == partId));

    public Task<bool> ExistsAsync(Guid id) => Task.FromResult(ServiceOrderParts.ContainsKey(id));
}
