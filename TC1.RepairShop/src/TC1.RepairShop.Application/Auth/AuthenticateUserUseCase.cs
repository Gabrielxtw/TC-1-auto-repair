namespace TC1.RepairShop.Application.Auth;

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

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return new AuthenticateUserResult(false, null);
        }

        var token = _tokenService.GenerateToken(user);
        return new AuthenticateUserResult(true, token);
    }
}
