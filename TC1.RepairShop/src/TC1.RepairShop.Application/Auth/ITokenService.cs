using TC1.RepairShop.Domain.Clients;
using TC1.RepairShop.Domain.Registration;

namespace TC1.RepairShop.Application.Auth;

public interface ITokenService
{
    string GenerateStaffToken(User user);
    string GenerateCustomerToken(Customer customer);
}
