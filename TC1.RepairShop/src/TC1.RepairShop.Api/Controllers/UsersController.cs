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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _listUsersUseCase.ExecuteAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _getUserUseCase.ExecuteAsync(id);
        if (user is null)
        {
            return NotFound(new { message = "User not found." });
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var result = await _createUserUseCase.ExecuteAsync(request);

        if (!result.success)
        {
            return Conflict(new { message = result.error });
        }

        return CreatedAtAction(nameof(GetById), new { id = result.data?.Id }, result.data);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserRequest request)
    {
        var result = await _updateUserUseCase.ExecuteAsync(request);

        if (!result.success)
        {
            return result.error == "User not found."
                ? NotFound(new { message = result.error })
                : Conflict(new { message = result.error });
        }

        return AcceptedAtAction(nameof(GetById), new { id = result.data?.Id }, result.data);
    }

    [HttpPut("password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangeUserPasswordRequest request)
    {
        var result = await _changeUserPasswordUseCase.ExecuteAsync(request);

        if (!result.success)
        {
            return NotFound(new { message = result.error });
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _deleteUserUseCase.ExecuteAsync(id);

        if (!result.success)
        {
            return NotFound(new { message = result.error });
        }

        return NoContent();
    }
}
