using TC1.RepairShop.Domain.Entities.ServiceOrders;

namespace TC1.RepairShop.Domain.Interfaces;

public interface IServiceOrderServiceRepository : IRepository<ServiceOrderService, Guid>
{
    Task<IEnumerable<ServiceOrderService>> GetByServiceOrderIdAsync(Guid serviceOrderId);
    Task<ServiceOrderService?> GetByServiceOrderAndServiceIdAsync(Guid serviceOrderId, Guid serviceId);
}
