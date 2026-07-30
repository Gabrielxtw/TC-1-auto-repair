using TC1.RepairShop.Application.Clients;
using TC1.RepairShop.Domain.Clients;

namespace TC1.RepairShop.Application.Clients.UseCases;

public record CreateUserRequest(string Username, string Password, string Role);

public record CreateUserResult(bool Success, string? Error, User? User);

public class CreateUserUseCase
{
    private readonly IUserRepository _userRepository;

    public CreateUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<CreateUserResult> ExecuteAsync(CreateUserRequest request)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
        if (existingUser is not null)
        {
            return new CreateUserResult(false, "Username is already taken.", null);
        }

        var user = User.Create(request.Username, request.Password, request.Role);

        await _userRepository.AddAsync(user);

        return new CreateUserResult(true, null, user);
    }
}
