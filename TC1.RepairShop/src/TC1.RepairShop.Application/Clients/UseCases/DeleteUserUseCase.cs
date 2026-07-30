using TC1.RepairShop.Application.Clients;

namespace TC1.RepairShop.Application.Clients.UseCases;

public record DeleteUserResult(bool Success, string? Error);

public class DeleteUserUseCase
{
    private readonly IUserRepository _userRepository;

    public DeleteUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

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
