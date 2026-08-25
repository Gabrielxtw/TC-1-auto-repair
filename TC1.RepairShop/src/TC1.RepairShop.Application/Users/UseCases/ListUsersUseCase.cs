using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Users.UseCases;

public class ListUsersUseCase(IUserRepository _userRepository) : BaseUseCase<ListUsersResponse>
{
    public async Task<BaseResponse<ListUsersResponse>> ExecuteAsync()
    {
        IEnumerable<User> users = await _userRepository.GetAllAsync();

        return new BaseResponse<ListUsersResponse>( data: UsersDTO.ToListUsersResponse(users));
    }
}
