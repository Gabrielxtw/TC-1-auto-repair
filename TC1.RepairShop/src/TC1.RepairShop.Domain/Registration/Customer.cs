using TC1.RepairShop.Domain.Common;

namespace TC1.RepairShop.Domain.Registration;

public class Customer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string NationalId { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public DateTime RegisteredAt { get; private set; }
    public Status Status { get; private set; }

    private Customer()
    {
    }

    public static Customer Create(string name, string nationalId, string phone, string email)
    {
        return new Customer
        {
            Id = Guid.NewGuid(),
            Name = name,
            NationalId = Registration.NationalId.Create(nationalId).Value,
            Phone = phone,
            Email = email,
            RegisteredAt = DateTime.UtcNow,
            Status = Status.Active,
        };
    }

    public void UpdateContactInfo(string phone, string email)
    {
        Phone = phone;
        Email = email;
    }

    public void Delete()
    {
        Status = Status.Deleted;
    }
}
