using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Enums;

namespace TC1.RepairShop.Domain.Interfaces
{
    public interface IServiceOrderRepository : IRepository<ServiceOrder, Guid>
    {
        Task<ServiceOrder> GetByUserId(Guid userId);
        Task<ServiceOrderService?> GetServiceOrderServiceById(Guid serviceOrderId, Guid serviceId);
        Task<ServiceOrderPart?> GetServiceOrderPartById(Guid serviceOrderId, Guid partId);
        Task<ServiceOrder?> GetByIdDetailedAsync(Guid id);
        Task<IEnumerable<ServiceOrder>> GetFilteredAsync(ServiceOrderStatus? status, DateTime? openedFrom, DateTime? openedTo, bool descending);

    }
}
