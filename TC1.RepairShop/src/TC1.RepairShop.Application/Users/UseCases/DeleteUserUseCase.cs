using TC1.RepairShop.Domain.Interfaces.Users;

namespace TC1.RepairShop.Application.Users.UseCases;

public record DeleteUserResult(bool Success, string? Error);

public class DeleteUserUseCase(IUserRepository _userRepository)
{
    public async Task<DeleteUserResult> ExecuteAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null)
        {
            return new DeleteUserResult(false, "User not found.");
        }

        user.Delete();

        await _userRepository.UpdateAsync(user);

        return new DeleteUserResult(true, null);
    }
}
