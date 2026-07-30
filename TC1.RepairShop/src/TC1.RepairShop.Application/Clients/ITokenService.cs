using TC1.RepairShop.Domain.Clients;

namespace TC1.RepairShop.Application.Clients;

public interface ITokenService
{
    string GenerateToken(User user);
}
