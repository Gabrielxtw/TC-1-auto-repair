using TC1.RepairShop.Domain.Entities.Costumers;
using TC1.RepairShop.Domain.Entities.Users;

namespace TC1.RepairShop.Application.Auth;

public interface ITokenService
{
    string GenerateStaffToken(User user);
    string GenerateCustomerToken(Costumer customer);
}
