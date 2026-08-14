using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Common;

namespace TC1.RepairShop.Domain.Entities.Parts;

public class Part : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int StockQuantity { get; set; }

    private Part()
    {
    }

    public void ReceiveStock(int quantity)
    {
        if (!IsActive())
            throw new BusinessException(BusinessErrors.Part.CannotAlterStockFromInactivePart);

        StockQuantity += quantity;
    }

    public void ConsumeStock(int quantity)
    {
        if (!IsActive())
            throw new BusinessException(BusinessErrors.Part.CannotAlterStockFromInactivePart);

        StockQuantity -= quantity;
    }

    public static Part Create(string name, decimal price)
    {
        return new Part
        {
            Name = name,
            Price = price,
        };
    }
}
