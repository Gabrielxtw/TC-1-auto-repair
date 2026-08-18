using Microsoft.AspNetCore.Mvc;
using TC1.RepairShop.Application.Auth.UseCases;

namespace TC1.RepairShop.Api.Controllers;

public class AuthController : BaseController
{
    private readonly AuthenticateUserUseCase _authenticateUserUseCase;

    public AuthController(
        AuthenticateUserUseCase authenticateUserUseCase)
    {
        _authenticateUserUseCase = authenticateUserUseCase;
    }

    public record LoginRequest(string Username, string Password);

    public record LoginResponse(string Token);

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authenticateUserUseCase.ExecuteAsync(
            new AuthenticateUserRequest(request.Username, request.Password));

        if (!result.Success)
        {
            return Unauthorized(new { message = "Invalid username or password." });
        }

        return Ok(new LoginResponse(result.Token!));
    }
}
