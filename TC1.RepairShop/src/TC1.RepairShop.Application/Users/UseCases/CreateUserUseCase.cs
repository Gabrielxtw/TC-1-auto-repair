using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces.Users;

namespace TC1.RepairShop.Application.Clients.UseCases;

public record CreateUserRequest(string Username, string Password, string Document, string Email, UserRole Role);

public record CreateUserResult(bool Success, string? Error, User? User);

public class CreateUserUseCase(IUserRepository _userRepository)
{
    public async Task<CreateUserResult> ExecuteAsync(CreateUserRequest request)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
        if (existingUser is not null)
        {
            return new CreateUserResult(false, "Username is already taken.", null);
        }

        var user = User.Create(request.Username, request.Password, request.Document, request.Email, request.Role);

        await _userRepository.AddAsync(user);

        return new CreateUserResult(true, null, user);
    }
}
