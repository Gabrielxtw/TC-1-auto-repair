namespace TC1.RepairShop.Domain.Entities.ServiceOrders;

public class ServiceOrderService
{
    public Guid Id { get; private set; }
    public Guid ServiceOrderId { get; private set; }
    public Guid ServiceId { get; private set; }

    private ServiceOrderService()
    {
    }

    public static ServiceOrderService Create(Guid serviceOrderId, Guid serviceId)
    {
        return new ServiceOrderService
        {
            Id = Guid.NewGuid(),
            ServiceOrderId = serviceOrderId,
            ServiceId = serviceId,
        };
    }
}
