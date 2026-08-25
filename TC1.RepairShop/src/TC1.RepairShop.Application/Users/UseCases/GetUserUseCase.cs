using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Users.UseCases;

public class GetUserUseCase(IUserRepository _userRepository): BaseUseCase<Guid,UserDetailedResponse>
{
    public async Task<BaseResponse<UserDetailedResponse>> ExecuteAsync(Guid request)
    {
        var user = await _userRepository.GetByIdAsync(request);
        if (user == null)
            throw new BusinessException(BusinessErrors.RequestErrors.NotFound);
        return new BaseResponse<UserDetailedResponse>(UsersDTO.ToUserDetailedResponse(user));
    }
}
