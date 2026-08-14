using TC1.RepairShop.Domain.Entities.Parts;

namespace TC1.RepairShop.Domain.Entities.ServiceOrders;

public class ServiceOrderPart
{
    public Guid Id { get; private set; }
    public Guid ServiceOrderId { get; private set; }
    public ServiceOrder ServiceOrder { get; private set; } = null!;
    public Guid PartId { get; private set; }
    public Part Part { get; private set; } = null!;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public bool SuppliedByCustomer { get; private set; }

    private ServiceOrderPart()
    {
    }

    public static ServiceOrderPart Create(Guid serviceOrderId, Guid partId, int quantity, decimal unitPrice, bool suppliedByCustomer)
    {
        return new ServiceOrderPart
        {
            Id = Guid.NewGuid(),
            ServiceOrderId = serviceOrderId,
            PartId = partId,
            Quantity = quantity,
            UnitPrice = suppliedByCustomer ? 0 : unitPrice,
            SuppliedByCustomer = suppliedByCustomer,
        };
    }
}
