using Microsoft.AspNetCore.Mvc;
using TC1.RepairShop.Application.Auth.UseCases;

namespace TC1.RepairShop.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthenticateUserUseCase _authenticateUserUseCase;
    private readonly AuthenticateCustomerUseCase _authenticateCustomerUseCase;

    public AuthController(
        AuthenticateUserUseCase authenticateUserUseCase,
        AuthenticateCustomerUseCase authenticateCustomerUseCase)
    {
        _authenticateUserUseCase = authenticateUserUseCase;
        _authenticateCustomerUseCase = authenticateCustomerUseCase;
    }

    public record LoginRequest(string Username, string Password);

    public record CustomerLoginRequest(string NationalId, string Password);

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

    [HttpPost("customer-login")]
    public async Task<IActionResult> CustomerLogin([FromBody] CustomerLoginRequest request)
    {
        var result = await _authenticateCustomerUseCase.ExecuteAsync(
            new AuthenticateCustomerRequest(request.NationalId, request.Password));

        if (!result.Success)
        {
            return Unauthorized(new { message = "Invalid national ID or password." });
        }

        return Ok(new LoginResponse(result.Token!));
    }
}
