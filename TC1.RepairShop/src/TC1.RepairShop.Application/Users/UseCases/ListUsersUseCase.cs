using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Interfaces.Users;

namespace TC1.RepairShop.Application.Users.UseCases;

public class ListUsersUseCase(IUserRepository _userRepository)
{
    public Task<IEnumerable<User>> ExecuteAsync() => _userRepository.GetAllAsync();
}
