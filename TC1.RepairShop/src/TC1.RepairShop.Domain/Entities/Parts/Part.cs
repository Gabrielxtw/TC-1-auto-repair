using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Common;
using TC1.RepairShop.Domain.Enums;

namespace TC1.RepairShop.Domain.Entities.Parts;

public class Part: BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }

    private Part()
    {
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
