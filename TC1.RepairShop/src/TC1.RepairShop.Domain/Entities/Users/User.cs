using TC1.RepairShop.Domain.Entities.Common;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.ValueObjects;

namespace TC1.RepairShop.Domain.Entities.Users;

public class User: BaseEntity
{
    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public Document Document { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public UserRole Role { get; private set; }

    private User()
    {
    }

    public static User Create(string username, string password, string document, string email, UserRole role)
    {
        return new User
        {
            Username = username,
            PasswordHash = PasswordHasher.Hash(password),
            Document = Document.Create(document),
            Email = Email.Create(email),
            Role = role,
        };
    }

    public bool VerifyPassword(string password) => PasswordHasher.Verify(password, PasswordHash);

    public void UpdateProfile(string username, UserRole role)
    {
        Username = username;
        Role = role;
    }

    public void ChangePassword(string newPassword)
    {
        PasswordHash = PasswordHasher.Hash(newPassword);
    }
}
