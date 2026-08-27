using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Common;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Events;

namespace TC1.RepairShop.Domain.Entities.Parts;

public class Part : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int StockQuantity { get; set; }

    public ICollection<ServiceOrderPart> ServiceOrderParts { get; } = new List<ServiceOrderPart>();

    private Part()
    {
    }

    public void ReceiveStock(int quantity)
    {
        if (!IsActive())
            throw new BusinessException(BusinessErrors.PartErrors.CannotAlterStockFromInactivePart);

        StockQuantity += quantity;
        RaiseDomainEvent(new PartReceivedEvent(Id, quantity));
    }

    public void ConsumeStock(int quantity)
    {
        if (!IsActive())
            throw new BusinessException(BusinessErrors.PartErrors.CannotAlterStockFromInactivePart);

        StockQuantity -= quantity;
    }

    public void Update(string name, decimal price)
    {
        if (!IsActive())
            throw new BusinessException(BusinessErrors.PartErrors.CannotAlterStockFromInactivePart);

        Name = name;
        Price = price;
    }

    public static Part Create(string name, decimal price, int stockQuantity = 0)
    {
        return new Part
        {
            Name = name,
            Price = price,
            StockQuantity = stockQuantity,
        };
    }
}
