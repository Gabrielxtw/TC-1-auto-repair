using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TC1.RepairShop.Application.Registration.UseCases;

namespace TC1.RepairShop.Api.Controllers;

[ApiController]
[Authorize(Policy = "CustomerOnly")]
[Route("api/customers/me")]
public class MyAccountController : ControllerBase
{
    private readonly ChangeCustomerPasswordUseCase _changeCustomerPasswordUseCase;

    public MyAccountController(ChangeCustomerPasswordUseCase changeCustomerPasswordUseCase)
    {
        _changeCustomerPasswordUseCase = changeCustomerPasswordUseCase;
    }

    public record ChangePasswordRequest(string CurrentPassword, string NewPassword);

    [HttpPut("password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var customerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub")!);

        var result = await _changeCustomerPasswordUseCase.ExecuteAsync(
            new ChangeCustomerPasswordRequest(customerId, request.CurrentPassword, request.NewPassword));

        if (!result.Success)
        {
            return result.Error == "Customer not found."
                ? NotFound(new { message = result.Error })
                : BadRequest(new { message = result.Error });
        }

        return NoContent();
    }
}
