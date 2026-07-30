using TC1.RepairShop.Domain.Common;

namespace TC1.RepairShop.Domain.Parts;

public class Part
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal UnitPrice { get; private set; }
    public int StockQuantity { get; private set; }
    public int MinimumQuantity { get; private set; }
    public Status Status { get; private set; }

    private Part()
    {
    }

    public static Part Create(string name, decimal unitPrice, int minimumQuantity)
    {
        return new Part
        {
            Id = Guid.NewGuid(),
            Name = name,
            UnitPrice = unitPrice,
            StockQuantity = 0,
            MinimumQuantity = minimumQuantity,
            Status = Status.Active,
        };
    }

    public void ReceiveStock(int quantity)
    {
        StockQuantity += quantity;
    }

    public void ConsumeStock(int quantity)
    {
        StockQuantity -= quantity;
    }

    public void Delete()
    {
        Status = Status.Deleted;
    }
}
