using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Interfaces.Users;

namespace TC1.RepairShop.Application.Users.UseCases;

public class GetUserUseCase(IUserRepository _userRepository)
{
    public async Task<BaseResponse<User?>> ExecuteAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return new BaseResponse<User?>(user);
    }
}
