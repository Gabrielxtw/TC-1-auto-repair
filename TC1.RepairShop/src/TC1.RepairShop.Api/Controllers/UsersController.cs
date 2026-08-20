using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TC1.RepairShop.Application.Users.UseCases;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Enums;

namespace TC1.RepairShop.Api.Controllers;

[Authorize(Policy = "AdminOnly")]
public class UsersController(CreateUserUseCase _createUserUseCase,
                            GetUserUseCase _getUserUseCase,
                            ListUsersUseCase _listUsersUseCase,
                            UpdateUserUseCase _updateUserUseCase,
                            ChangeUserPasswordUseCase _changeUserPasswordUseCase,
                            DeleteUserUseCase _deleteUserUseCase
    ) : BaseController
{

    public record UpdateRequest(string Username, UserRole Role);

    public record ChangePasswordRequest(string NewPassword);

    public record UserResponse(Guid Id, string Username, string Role, string Status);

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _listUsersUseCase.ExecuteAsync();
        return Ok(users.Select(ToResponse));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _getUserUseCase.ExecuteAsync(id);
        if (user is null)
        {
            return NotFound(new { message = "User not found." });
        }

        return Ok(ToResponse(user));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var result = await _createUserUseCase.ExecuteAsync(request);

        if (!result.Success)
        {
            return Conflict(new { message = result.Error });
        }

        var response = ToResponse(result.User!);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRequest request)
    {
        var result = await _updateUserUseCase.ExecuteAsync(
            new UpdateUserRequest(id, request.Username, request.Role));

        if (!result.Success)
        {
            return result.Error == "User not found."
                ? NotFound(new { message = result.Error })
                : Conflict(new { message = result.Error });
        }

        return NoContent();
    }

    [HttpPut("{id:guid}/password")]
    public async Task<IActionResult> ChangePassword(Guid id, [FromBody] ChangePasswordRequest request)
    {
        var result = await _changeUserPasswordUseCase.ExecuteAsync(
            new ChangeUserPasswordRequest(id, request.NewPassword));

        if (!result.Success)
        {
            return NotFound(new { message = result.Error });
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _deleteUserUseCase.ExecuteAsync(id);

        if (!result.Success)
        {
            return NotFound(new { message = result.Error });
        }

        return NoContent();
    }

    private static UserResponse ToResponse(User user) =>
        new(user.Id, user.Username, user.Role.ToString(), user.Status.ToString());
}
