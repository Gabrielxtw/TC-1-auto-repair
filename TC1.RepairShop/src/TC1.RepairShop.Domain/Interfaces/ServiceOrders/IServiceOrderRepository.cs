using TC1.RepairShop.Domain.Entities.ServiceOrders;

namespace TC1.RepairShop.Domain.Interfaces.ServiceOrders
{
    public interface IServiceOrderRepository : IRepository<ServiceOrder, Guid>
    {
        Task<ServiceOrder> GetByUserId(Guid userId);
        Task<ServiceOrderService?> GetServiceOrderServiceById(Guid serviceOrderId, Guid serviceId);
        Task<ServiceOrderPart?> GetServiceOrderPartById(Guid serviceOrderId, Guid partId);
        Task<ServiceOrder?> GetByIdDetailedAsync(Guid id);

    }
}
