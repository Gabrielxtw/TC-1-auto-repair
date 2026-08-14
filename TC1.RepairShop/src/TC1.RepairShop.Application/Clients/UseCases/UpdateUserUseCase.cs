using TC1.RepairShop.Application.Clients;
using TC1.RepairShop.Domain.Entities.Clients;

namespace TC1.RepairShop.Application.Clients.UseCases;

public record UpdateUserRequest(Guid Id, string Username, Role Role);

public record UpdateUserResult(bool Success, string? Error);

public class UpdateUserUseCase
{
    private readonly IUserRepository _userRepository;

    public UpdateUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UpdateUserResult> ExecuteAsync(UpdateUserRequest request)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user is null)
        {
            return new UpdateUserResult(false, "User not found.");
        }

        var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
        if (existingUser is not null && existingUser.Id != request.Id)
        {
            return new UpdateUserResult(false, "Username is already taken.");
        }

        user.UpdateProfile(request.Username, request.Role);

        await _userRepository.UpdateAsync(user);

        return new UpdateUserResult(true, null);
    }
}
