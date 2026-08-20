using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Interfaces.Users;

namespace TC1.RepairShop.Application.Users.UseCases;

public class ListUsersUseCase(IUserRepository _userRepository)
{
    public async Task<BaseResponse<IEnumerable<User>>> ExecuteAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return new BaseResponse<IEnumerable<User>>(users);
    }
}
