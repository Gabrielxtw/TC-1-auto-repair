using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Users.UseCases;

public class GetUserUseCase(IUserRepository _userRepository): BaseUseCase<Guid,UserDetailedResponse>
{
    public async Task<BaseResponse<UserDetailedResponse?>> ExecuteAsync(Guid request)
    {
        var user = await _userRepository.GetByIdAsync(request);
        if (user is null)
            return new BaseResponse<UserDetailedResponse?>(data: null, success: false, error: "User not found.");
        return new BaseResponse<UserDetailedResponse?>(UsersDTO.ToUserDetailedResponse(user));
    }
}
