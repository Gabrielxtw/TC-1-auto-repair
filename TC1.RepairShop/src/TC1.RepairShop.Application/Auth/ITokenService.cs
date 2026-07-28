using TC1.RepairShop.Domain.Auth;

namespace TC1.RepairShop.Application.Auth;

public interface ITokenService
{
    string GenerateToken(User user);
}
