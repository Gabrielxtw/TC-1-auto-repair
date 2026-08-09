using TC1.RepairShop.Application.Clients;

namespace TC1.RepairShop.Application.Auth.UseCases;

public record AuthenticateUserRequest(string Username, string Password);

public record AuthenticateUserResult(bool Success, string? Token);

public class AuthenticateUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthenticateUserUseCase(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthenticateUserResult> ExecuteAsync(AuthenticateUserRequest request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);

        if (user is null || !user.VerifyPassword(request.Password))
        {
            return new AuthenticateUserResult(false, null);
        }

        var token = _tokenService.GenerateStaffToken(user);
        return new AuthenticateUserResult(true, token);
    }
}
