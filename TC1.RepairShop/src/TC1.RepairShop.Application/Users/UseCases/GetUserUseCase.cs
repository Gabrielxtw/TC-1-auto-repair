using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Interfaces.Users;

namespace TC1.RepairShop.Application.Users.UseCases;

public class GetUserUseCase(IUserRepository _userRepository)
{
    public Task<User?> ExecuteAsync(Guid id) => _userRepository.GetByIdAsync(id);
}
