using TC1.RepairShop.Domain.Entities.Common;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Registration;

namespace TC1.RepairShop.Domain.Entities.Vehicles;

public class Vehicle: BaseEntity
{
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public LicensePlate LicensePlate { get; private set; } = null!;
    public string Brand { get; private set; } = string.Empty;
    public string Model { get; private set; } = string.Empty;
    public int Year { get; private set; }

    private Vehicle()
    {
    }

    public static Vehicle Create(Guid userId, string licensePlate, string brand, string model, int year)
    {
        return new Vehicle
        {
            UserId = userId,
            LicensePlate = LicensePlate.Create(licensePlate),
            Brand = brand,
            Model = model,
            Year = year,
        };
    }
}
