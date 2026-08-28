using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Users.UseCases;

public class ChangeUserPasswordUseCase(IUserRepository _userRepository) : BaseUseCase<ChangeUserPasswordRequest, UserResponse?>
{
    protected override async Task<BaseResponse<UserResponse?>> HandleAsync(ChangeUserPasswordRequest request)
    {
        var user = await _userRepository.GetByIdAsync(request.Id) ?? throw new BusinessException(BusinessErrors.UserErrors.NotFound);

        user.ChangePassword(request.NewPassword);

        await _userRepository.UpdateAsync(user);

        return new BaseResponse<UserResponse?>(UsersDTO.ToUserResponse(user));
    }
}
