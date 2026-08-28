using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Users.UseCases;

public class GetUserUseCase(IUserRepository _userRepository): BaseUseCase<Guid,UserDetailedResponse?>
{
    protected override async Task<BaseResponse<UserDetailedResponse?>> HandleAsync(Guid request)
    {
        var user = await _userRepository.GetByIdAsync(request);
        if (user is null)
            throw new BusinessException(BusinessErrors.UserErrors.NotFound);

        return new BaseResponse<UserDetailedResponse?>(data: UsersDTO.ToUserDetailedResponse(user));
    }
}
