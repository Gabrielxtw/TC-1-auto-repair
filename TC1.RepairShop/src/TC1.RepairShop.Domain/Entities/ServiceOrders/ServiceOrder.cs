using TC1.RepairShop.Domain.Entities.Common;
using TC1.RepairShop.Domain.Enums;

namespace TC1.RepairShop.Domain.Entities.ServiceOrders;

public class ServiceOrder: BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid VehicleId { get; private set; }
    public ServiceOrderStatus OrderStatusValue { get; private set; }
    public DateTime OpenedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public Guid? QuoteId { get; private set; }

    private ServiceOrder()
    {
    }

    public static ServiceOrder Create(Guid userId, Guid vehicleId)
    {
        return new ServiceOrder
        {
            UserId = userId,
            VehicleId = vehicleId,
            OrderStatusValue = ServiceOrderStatus.Received,
            OpenedAt = DateTime.UtcNow,
        };
    }

    public void AttachQuote(Guid quoteId)
    {
        QuoteId = quoteId;
    }

    public void AdvanceTo(ServiceOrderStatus newStatus)
    {
        OrderStatusValue = newStatus;

        if (newStatus == ServiceOrderStatus.Delivered)
        {
            CompletedAt = DateTime.UtcNow;
        }
    }
}
