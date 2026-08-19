using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Common;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Entities.Vehicles;
using TC1.RepairShop.Domain.Enums;

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
    public ICollection<Service> Services { get; } = new List<Service>();
    public ICollection<ServiceOrderPart> ServiceOrderParts { get; } = new List<ServiceOrderPart>();
    public ICollection<Part> Parts { get; } = new List<Part>();

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
    }

    public void AttachServices(ICollection<Guid> serviceIds)
    {
        if (!IsActive())
            throw new BusinessException(BusinessErrors.Entity.CannotDoActionInactiveEntity);

        foreach (var serviceId in serviceIds)
        {
            if (ServiceOrderServices.Any(s => s.ServiceId == serviceId))
                continue;

            ServiceOrderServices.Add(ServiceOrderService.Create(Id, serviceId));
        }
    }
    public void AttachParts(ICollection<(Guid PartId, int Quantity, bool SuppliedByCustomer)> parts)
    {
        if (!IsActive())
            throw new BusinessException(BusinessErrors.Entity.CannotDoActionInactiveEntity);

        foreach (var (partId, quantity, suppliedByCustomer) in parts)
        {
            if (quantity <= 0)
                throw new BusinessException(BusinessErrors.ServiceOrder.QuantityMustBePositive);

            ServiceOrderParts.Add(ServiceOrderPart.Create(Id, partId, quantity, suppliedByCustomer));
        }
    }

    public void AdvanceTo(ServiceOrderStatus newStatus)
    {
        if(!OrderStatusValue.CanTransitionTo(newStatus))
        {
            throw new BusinessException(BusinessErrors.ServiceOrder.InvalidStatusTransition);
        }
        OrderStatusValue = newStatus;

        if (newStatus == ServiceOrderStatus.Delivered || newStatus == ServiceOrderStatus.Cancelled)
        {
            CompletedAt = DateTime.UtcNow;
        }
    }
}
