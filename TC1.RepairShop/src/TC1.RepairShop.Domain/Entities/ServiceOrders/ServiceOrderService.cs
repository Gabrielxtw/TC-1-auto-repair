using TC1.RepairShop.Domain.Entities.Services;

namespace TC1.RepairShop.Domain.Entities.ServiceOrders;

public class ServiceOrderService
{
    public Guid Id { get; private set; }
    public Guid ServiceOrderId { get; private set; }
    public Guid ServiceId { get; private set; }
    public decimal Price { get; private set; }
    public ServiceOrder ServiceOrder { get; private set; } = null!;
    public Service Service { get; private set; } = null!;

    private ServiceOrderService()
    {
    }

    public static ServiceOrderService Create(Guid serviceOrderId, Guid serviceId, decimal price)
    {
        return new ServiceOrderService
        {
            Id = Guid.NewGuid(),
            ServiceOrderId = serviceOrderId,
            ServiceId = serviceId,
            Price = price,
        };
    }
}
