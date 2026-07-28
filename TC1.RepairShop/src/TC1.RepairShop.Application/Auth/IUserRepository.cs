using TC1.RepairShop.Domain.Auth;

namespace TC1.RepairShop.Application.Auth;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
}
