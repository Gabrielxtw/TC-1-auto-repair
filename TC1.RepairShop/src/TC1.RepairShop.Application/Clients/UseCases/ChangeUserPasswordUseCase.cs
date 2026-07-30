using TC1.RepairShop.Application.Clients;

namespace TC1.RepairShop.Application.Clients.UseCases;

public record ChangeUserPasswordRequest(Guid Id, string NewPassword);

public record ChangeUserPasswordResult(bool Success, string? Error);

public class ChangeUserPasswordUseCase
{
    private readonly IUserRepository _userRepository;

    public ChangeUserPasswordUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ChangeUserPasswordResult> ExecuteAsync(ChangeUserPasswordRequest request)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user is null)
        {
            return new ChangeUserPasswordResult(false, "User not found.");
        }

        user.ChangePassword(request.NewPassword);

        await _userRepository.UpdateAsync(user);

        return new ChangeUserPasswordResult(true, null);
    }
}
