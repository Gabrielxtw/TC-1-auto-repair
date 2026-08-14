using TC1.RepairShop.Domain.Entities.Common;

namespace TC1.RepairShop.Domain.Entities.Services;

public class Service: BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }

    private Service()
    {
    }

    public static Service Create(string name, string description, decimal price)
    {
        return new Service
        {
            Name = name,
            Description = description,
            Price = price,
        };
    }
}
