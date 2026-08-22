using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Users.UseCases;

public record ChangeUserPasswordRequest(Guid Id, string NewPassword);

public record ChangeUserPasswordResult(bool Success, string? Error);

public class ChangeUserPasswordUseCase(IUserRepository _userRepository)
{
    public async Task<BaseResponse<ChangeUserPasswordResult?>> ExecuteAsync(ChangeUserPasswordRequest request)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user is null)
        {
            return new BaseResponse<ChangeUserPasswordResult?>(new ChangeUserPasswordResult(false, "User not found."));
        }

        user.ChangePassword(request.NewPassword);

        await _userRepository.UpdateAsync(user);

        return new BaseResponse<ChangeUserPasswordResult?>(new ChangeUserPasswordResult(true, null));
    }
}
