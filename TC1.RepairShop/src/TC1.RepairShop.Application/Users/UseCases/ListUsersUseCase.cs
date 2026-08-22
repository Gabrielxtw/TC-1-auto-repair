using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Users.UseCases;

public record ListUsersResponse(Guid id, string username, string status, string role);
public class ListUsersUseCase(IUserRepository _userRepository)
{
    public async Task<BaseResponse<IEnumerable<ListUsersResponse>>> ExecuteAsync()
    {
        IEnumerable<User> users = await _userRepository.GetAllAsync();
        return new BaseResponse<IEnumerable<ListUsersResponse>>( data: 
            users.Select(u => new ListUsersResponse(u.Id, u.Username,u.Status.ToString(),u.Role.ToString()))
            );
    }
}
