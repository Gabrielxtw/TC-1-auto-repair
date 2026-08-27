using System.Collections.Concurrent;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.IntegrationTests;

public class FakeServiceOrderServiceRepository : IServiceOrderServiceRepository
{
    public static readonly ConcurrentDictionary<Guid, ServiceOrderService> ServiceOrderServices = new();

    public Task<IEnumerable<ServiceOrderService>> GetByServiceOrderIdAsync(Guid serviceOrderId) =>
        Task.FromResult(ServiceOrderServices.Values.Where(s => s.ServiceOrderId == serviceOrderId));

    public Task<ServiceOrderService?> GetByServiceOrderAndServiceIdAsync(Guid serviceOrderId, Guid serviceId) =>
        Task.FromResult(ServiceOrderServices.Values.FirstOrDefault(s => s.ServiceOrderId == serviceOrderId && s.ServiceId == serviceId));

    public Task<IEnumerable<ServiceOrderService>> GetAllAsync() =>
        Task.FromResult(ServiceOrderServices.Values.AsEnumerable());

    public Task<ServiceOrderService?> GetByIdAsync(Guid id)
    {
        ServiceOrderServices.TryGetValue(id, out var service);
        return Task.FromResult(service);
    }

    public Task AddAsync(ServiceOrderService service)
    {
        ServiceOrderServices[service.Id] = service;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(ServiceOrderService service)
    {
        ServiceOrderServices[service.Id] = service;
        return Task.CompletedTask;
    }

    public Task Add(ServiceOrderService service)
    {
        ServiceOrderServices[service.Id] = service;
        return Task.CompletedTask;
    }

    public Task Update(ServiceOrderService service)
    {
        ServiceOrderServices[service.Id] = service;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        ServiceOrderServices.TryRemove(id, out _);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync() => Task.CompletedTask;

    public Task<bool> ExistsAsync(Guid id) => Task.FromResult(ServiceOrderServices.ContainsKey(id));
}
