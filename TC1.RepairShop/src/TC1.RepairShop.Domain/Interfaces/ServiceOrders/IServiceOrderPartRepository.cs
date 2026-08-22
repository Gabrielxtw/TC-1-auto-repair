using TC1.RepairShop.Domain.Entities.ServiceOrders;

namespace TC1.RepairShop.Domain.Interfaces;

public interface IServiceOrderPartRepository : IRepository<ServiceOrderPart, Guid>
{
    Task<IEnumerable<ServiceOrderPart>> GetByServiceOrderIdAsync(Guid serviceOrderId);
    Task<ServiceOrderPart?> GetByServiceOrderAndPartIdAsync(Guid serviceOrderId, Guid partId);
}
