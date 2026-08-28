using System.Net;
using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Users.UseCases;


public class DeleteUserUseCase(IUserRepository _userRepository): BaseUseCase<Guid, UserResponse?>
{
    protected override async Task<BaseResponse<UserResponse?>> HandleAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id) ?? throw new BusinessException(BusinessErrors.UserErrors.NotFound);

        user.Delete();

        await _userRepository.UpdateAsync(user);

        return new BaseResponse<UserResponse?>(data: null, success: true, StatusCode: HttpStatusCode.NoContent);
    }
}
