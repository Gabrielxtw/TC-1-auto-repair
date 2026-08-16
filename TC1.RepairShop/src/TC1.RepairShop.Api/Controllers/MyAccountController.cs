using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TC1.RepairShop.Application.Clients.UseCases;
using TC1.RepairShop.Application.Registration.UseCases;

namespace TC1.RepairShop.Api.Controllers;

[ApiController]
[Authorize(Policy = "UserOnly")]
[Route("api/users/me")]
public class MyAccountController : ControllerBase
{
    private readonly ChangeUserPasswordUseCase _changeUserPasswordUseCase;

    public MyAccountController(ChangeUserPasswordUseCase changeUserPasswordUseCase)
    {
        _changeUserPasswordUseCase = changeUserPasswordUseCase;
    }

    public record ChangePasswordRequest(string CurrentPassword, string NewPassword);

    [HttpPut("password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub")!);

        var result = await _changeUserPasswordUseCase.ExecuteAsync(
            new ChangeUserPasswordRequest(userId, request.NewPassword));

        if (!result.Success)
        {
            return result.Error == "User not found."
                ? NotFound(new { message = result.Error })
                : BadRequest(new { message = result.Error });
        }

        return NoContent();
    }
}
