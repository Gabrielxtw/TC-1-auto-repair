using TC1.RepairShop.Domain.Common;

namespace TC1.RepairShop.Domain.Clients;

public class User
{
    public Guid Id { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Role { get; private set; } = string.Empty;
    public Status Status { get; private set; }

    private User()
    {
    }

    public static User Create(string username, string password, string role)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            PasswordHash = PasswordHasher.Hash(password),
            Role = role,
            Status = Status.Active,
        };
    }

    public bool VerifyPassword(string password) => PasswordHasher.Verify(password, PasswordHash);

    public void UpdateProfile(string username, string role)
    {
        Username = username;
        Role = role;
    }

    public void ChangePassword(string newPassword)
    {
        PasswordHash = PasswordHasher.Hash(newPassword);
    }

    public void Delete()
    {
        Status = Status.Deleted;
    }
}
