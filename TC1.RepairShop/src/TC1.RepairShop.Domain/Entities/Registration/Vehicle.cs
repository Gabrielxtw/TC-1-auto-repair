using TC1.RepairShop.Domain.Entities.Common;

namespace TC1.RepairShop.Domain.Entities.Registration;

public class Vehicle
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public string LicensePlate { get; private set; } = string.Empty;
    public string Brand { get; private set; } = string.Empty;
    public string Model { get; private set; } = string.Empty;
    public int Year { get; private set; }
    public Status Status { get; private set; }

    private Vehicle()
    {
    }

    public static Vehicle Create(Guid customerId, string licensePlate, string brand, string model, int year)
    {
        return new Vehicle
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            LicensePlate = Registration.LicensePlate.Create(licensePlate).Value,
            Brand = brand,
            Model = model,
            Year = year,
            Status = Status.Active,
        };
    }

    public void Delete()
    {
        Status = Status.Deleted;
    }
}
