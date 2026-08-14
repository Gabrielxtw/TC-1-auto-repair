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

    private bool IsActive() => Status == Enums.Status.Active;

    public void ReceiveStock(int quantity)
    {
        if (!IsActive())
            ///ToDo exception para esse caso
            throw new Exception();

        StockQuantity += quantity;
    }

    public void ConsumeStock(int quantity)
    {
        if (!IsActive())
            ///ToDo exception para esse caso
            throw new Exception();

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
