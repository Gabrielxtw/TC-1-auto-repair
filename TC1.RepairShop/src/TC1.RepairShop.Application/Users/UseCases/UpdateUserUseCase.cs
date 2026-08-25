using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Users.UseCases;


public class UpdateUserUseCase(IUserRepository _userRepository): BaseUseCase<UpdateUserRequest, UserResponse?>
{
    public async Task<BaseResponse<UserResponse?>> ExecuteAsync(UpdateUserRequest request)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user is null)
        {
            return new BaseResponse<UserResponse?>(data: null, success: false, error: "User not found.");
        }

        var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
        if (existingUser is not null && existingUser.Id != request.Id)
        {
            return new BaseResponse<UserResponse?>(data: null, success: false, error: "Username is already taken.");
        }

        user.UpdateProfile(request.Username, request.Role);

        await _userRepository.UpdateAsync(user);

        return new BaseResponse<UserResponse?>(UsersDTO.ToUserResponse(user));
    }
}
