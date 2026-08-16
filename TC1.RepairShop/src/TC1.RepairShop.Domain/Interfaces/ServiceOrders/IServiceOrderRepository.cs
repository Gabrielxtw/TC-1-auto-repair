using TC1.RepairShop.Domain.Entities.ServiceOrders;

namespace TC1.RepairShop.Domain.Interfaces.ServiceOrders
{
    public interface IServiceOrderRepository : IRepository<ServiceOrder, Guid>
    {
        Task<ServiceOrder> GetByUserId(Guid userId);
        Task<ServiceOrder> GetByServiceId(Guid serviceId);
    }
}
