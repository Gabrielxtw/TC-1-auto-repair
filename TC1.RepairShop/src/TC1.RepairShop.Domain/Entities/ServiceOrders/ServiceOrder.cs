using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Common;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Entities.Vehicles;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Events;
using static TC1.RepairShop.Domain.CustomExceptions.BusinessErrors;

namespace TC1.RepairShop.Domain.Entities.ServiceOrders;

public class ServiceOrder: BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid VehicleId { get; private set; }
    public ServiceOrderStatus OrderStatusValue { get; private set; } = ServiceOrderStatus.Received;
    public DateTime OpenedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public Guid? QuoteId { get; private set; }

    public User User { get; private set; } = null!;
    public Vehicle Vehicle { get; private set; } = null!;
    public Quote? Quote { get; private set; } = null!;
    public ICollection<ServiceOrderService> ServiceOrderServices { get; } = new List<ServiceOrderService>();
    public ICollection<ServiceOrderPart> ServiceOrderParts { get; } = new List<ServiceOrderPart>();

    private ServiceOrder()
    {
    }

    public static ServiceOrder Create(Guid userId, Guid vehicleId)
    {
        return new ServiceOrder
        {
            UserId = userId,
            VehicleId = vehicleId,
            OpenedAt = DateTime.UtcNow,
        };
    }

    public void AttachQuote(Guid quoteId)
    {
        QuoteId = quoteId;
        RaiseDomainEvent(new QuoteCreatedUpdatedEvent(quoteId));
    }

    public void AdvanceTo(ServiceOrderStatus newStatus)
    {
        if(!OrderStatusValue.CanTransitionTo(newStatus))
        {
            throw new BusinessException(ServiceOrderErrors.InvalidStatusTransition);
        }
        if(newStatus == ServiceOrderStatus.AwaitingApproval)
            RaiseDomainEvent(new DiagnosisConcludedEvent(
                Id,
                price:
                    ServiceOrderServices.Sum(sos => sos.Price) + 
                    ServiceOrderParts.Where(sop => !sop.SuppliedByCustomer).Sum(sop => sop.Price * sop.Quantity),
                QuoteId)
            );
        OrderStatusValue = newStatus;

        if (newStatus == ServiceOrderStatus.Delivered || newStatus == ServiceOrderStatus.Cancelled)
        {
            CompletedAt = DateTime.UtcNow;
        }
    }
}
