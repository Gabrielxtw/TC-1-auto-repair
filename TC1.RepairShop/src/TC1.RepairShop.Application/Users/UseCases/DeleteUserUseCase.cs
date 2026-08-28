using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Users.UseCases;


public class DeleteUserUseCase(IUserRepository _userRepository): BaseUseCase<Guid, UserResponse?>
{
    public async Task<BaseResponse<UserResponse?>> ExecuteAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null)
        {
            return new BaseResponse<UserResponse?>(data: null, success: false, error: "User not found.",StatusCode: "404");
        }

        user.Delete();

        await _userRepository.UpdateAsync(user);

        return new BaseResponse<UserResponse?>(data: null, success: true, StatusCode: "204");
    }
}
