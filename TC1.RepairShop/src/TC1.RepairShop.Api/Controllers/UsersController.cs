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
        return Response(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _getUserUseCase.ExecuteAsync(id);

        return Response(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var result = await _createUserUseCase.ExecuteAsync(request);

        return Response(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserRequest request)
    {
        var result = await _updateUserUseCase.ExecuteAsync(request);

        return Response(result);
    }

    [HttpPut("password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangeUserPasswordRequest request)
    {
        var result = await _changeUserPasswordUseCase.ExecuteAsync(request);

        return Response(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _deleteUserUseCase.ExecuteAsync(id);

        return Response(result);
    }
}
