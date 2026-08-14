using TC1.RepairShop.Domain.Entities.Common;

namespace TC1.RepairShop.Domain.Entities.Registration;

public class Customer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string NationalId { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? PasswordHash { get; private set; }
    public DateTime RegisteredAt { get; private set; }
    public Status Status { get; private set; }

    private Customer()
    {
    }

    public static Customer Create(string name, string nationalId, string phone, string email)
    {
        var normalizedNationalId = Registration.NationalId.Create(nationalId).Value;

        return new Customer
        {
            Id = Guid.NewGuid(),
            Name = name,
            NationalId = normalizedNationalId,
            Phone = phone,
            Email = email,
            PasswordHash = PasswordHasher.Hash(BuildDefaultPassword(normalizedNationalId, name)),
            RegisteredAt = DateTime.UtcNow,
            Status = Status.Active,
        };
    }

    private static string BuildDefaultPassword(string normalizedNationalId, string name)
    {
        var firstName = name.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? string.Empty;
        var capitalizedFirstName = firstName.Length == 0
            ? firstName
            : char.ToUpperInvariant(firstName[0]) + firstName[1..];

        return normalizedNationalId[..3] + capitalizedFirstName;
    }

    public bool VerifyPassword(string password) =>
        PasswordHash is not null && PasswordHasher.Verify(password, PasswordHash);

    public void ChangePassword(string newPassword)
    {
        PasswordHash = PasswordHasher.Hash(newPassword);
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
