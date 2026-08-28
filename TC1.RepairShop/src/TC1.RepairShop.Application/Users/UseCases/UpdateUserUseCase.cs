using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Users.UseCases;


public class UpdateUserUseCase(IUserRepository _userRepository): BaseUseCase<UpdateUserRequest, UserResponse?>
{
    protected override async Task<BaseResponse<UserResponse?>> HandleAsync(UpdateUserRequest request)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user is null)
        {
            throw new BusinessException(BusinessErrors.UserErrors.NotFound);
        }

        var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
        if (existingUser is not null && existingUser.Id != request.Id)
        {
            throw new BusinessException(BusinessErrors.UserErrors.DuplicateUsername);
        }

        user.UpdateProfile(request.Username, request.Role);

        await _userRepository.UpdateAsync(user);

        return new BaseResponse<UserResponse?>(UsersDTO.ToUserResponse(user));
    }
}
