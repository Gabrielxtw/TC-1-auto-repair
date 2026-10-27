using System.Collections.Concurrent;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.IntegrationTests;

public class FakeServiceOrderRepository : IServiceOrderRepository
{
    public static readonly ConcurrentDictionary<Guid, ServiceOrder> ServiceOrders = new();

    public Task<ServiceOrder> GetByUserId(Guid userId) =>
        Task.FromResult(ServiceOrders.Values.First(o => o.UserId == userId));

    public Task<ServiceOrderService?> GetServiceOrderServiceById(Guid serviceOrderId, Guid serviceId)
    {
        var match = FakeServiceOrderServiceRepository.ServiceOrderServices.Values
            .FirstOrDefault(s => s.ServiceOrderId == serviceOrderId && s.ServiceId == serviceId);
        return Task.FromResult(match);
    }

    public Task<ServiceOrderPart?> GetServiceOrderPartById(Guid serviceOrderId, Guid partId)
    {
        var match = FakeServiceOrderPartRepository.ServiceOrderParts.Values
            .FirstOrDefault(p => p.ServiceOrderId == serviceOrderId && p.PartId == partId);
        return Task.FromResult(match);
    }

    public Task<ServiceOrder?> GetByIdDetailedAsync(Guid id)
    {
        ServiceOrders.TryGetValue(id, out var order);
        return Task.FromResult(order);
    }

    public Task<IEnumerable<ServiceOrder>> GetAllAsync() =>
        Task.FromResult(ServiceOrders.Values.AsEnumerable());

    public Task<IEnumerable<ServiceOrder>> GetFilteredAsync(ServiceOrderStatus? status, DateTime? openedFrom, DateTime? openedTo, bool descending)
    {
        IEnumerable<ServiceOrder> orders = ServiceOrders.Values;
        if (status is not null)
            orders = orders.Where(order => order.OrderStatusValue == status);
        if (openedFrom.HasValue)
            orders = orders.Where(order => order.OpenedAt >= openedFrom.Value);
        if (openedTo.HasValue)
            orders = orders.Where(order => order.OpenedAt <= openedTo.Value);

        orders = descending
            ? orders.OrderByDescending(order => order.OpenedAt)
            : orders.OrderBy(order => order.OpenedAt);
        return Task.FromResult(orders);
    }

    public Task<ServiceOrder?> GetByIdAsync(Guid id)
    {
        ServiceOrders.TryGetValue(id, out var order);
        return Task.FromResult(order);
    }

    public Task AddAsync(ServiceOrder order)
    {
        ServiceOrders[order.Id] = order;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(ServiceOrder order)
    {
        ServiceOrders[order.Id] = order;
        return Task.CompletedTask;
    }

    public Task Add(ServiceOrder order)
    {
        ServiceOrders[order.Id] = order;
        return Task.CompletedTask;
    }

    public Task Update(ServiceOrder order)
    {
        ServiceOrders[order.Id] = order;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        ServiceOrders.TryRemove(id, out _);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync() => Task.CompletedTask;

    public Task<bool> ExistsAsync(Guid id) => Task.FromResult(ServiceOrders.ContainsKey(id));
}
