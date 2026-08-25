using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Users.UseCases;

public class ChangeUserPasswordUseCase(IUserRepository _userRepository) : BaseUseCase<ChangeUserPasswordRequest, UserResponse?>
{
    public async Task<BaseResponse<UserResponse?>> ExecuteAsync(ChangeUserPasswordRequest request)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user is null)
        {
            return new BaseResponse<UserResponse?>(null,success: false, error: "User not found.");
        }

        user.ChangePassword(request.NewPassword);

        await _userRepository.UpdateAsync(user);

        return new BaseResponse<UserResponse?>(UsersDTO.ToUserResponse(user));
    }
}
