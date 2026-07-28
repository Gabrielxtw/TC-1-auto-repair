namespace TC1.RepairShop.Domain.Registration;

public class Vehicle
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
}
