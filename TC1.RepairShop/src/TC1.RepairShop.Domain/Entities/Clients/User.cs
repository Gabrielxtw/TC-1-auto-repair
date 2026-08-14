using TC1.RepairShop.Domain.Entities.Common;

namespace TC1.RepairShop.Domain.Entities.Clients;

public class User: BaseEntity
{
    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public Document Document { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public Role Role { get; private set; }
    public Status Status { get; private set; }

    private User()
    {
    }

    public static User Create(string username, string password, string document, string email, Role role)
    {
        return new User
        {
            Username = username,
            PasswordHash = PasswordHasher.Hash(password),
            Document = Document.Create(document),
            Email = Email.Create(email),
            Role = role,
            Status = Status.Active,
        };
    }

    public bool VerifyPassword(string password) => PasswordHasher.Verify(password, PasswordHash);

    public void UpdateProfile(string username, Role role)
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
