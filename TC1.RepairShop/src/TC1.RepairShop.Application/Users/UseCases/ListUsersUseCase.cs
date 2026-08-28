using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Users.UseCases;

public class ListUsersUseCase(IUserRepository _userRepository) : BaseUseCase<ListUsersResponse>
{
    protected override async Task<BaseResponse<ListUsersResponse>> HandleAsync()
    {
        IEnumerable<User> users = await _userRepository.GetAllAsync();

        return new BaseResponse<ListUsersResponse>( data: UsersDTO.ToListUsersResponse(users));
    }
}
