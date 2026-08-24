using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;
using TC1.RepairShop.Domain.CustomExceptions;

namespace TC1.RepairShop.Application.Users.UseCases;

public record CreateUserRequest(string Username, string Password, string Document, string Email, UserRole Role, string Phone);

public record CreateUserResult(Guid id,string username, string document, string email);

public class CreateUserUseCase(IUserRepository _userRepository)
{
    public async Task<BaseResponse<CreateUserResult?>> ExecuteAsync(CreateUserRequest request)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
        if (existingUser is not null)
        {
            return new BaseResponse<CreateUserResult?>(data: null, success: false, error: "Username is already taken.");
        }

        try
        {
            var user = User.Create(request.Username, request.Password, request.Document, request.Email, request.Role, request.Phone);
            await _userRepository.AddAsync(user);
            return new BaseResponse<CreateUserResult?>(new CreateUserResult(user.Id, user.Username, user.Document.Value, user.Email.Value));
        }
        catch (BusinessException ex)
        {
            return new BaseResponse<CreateUserResult?>(new CreateUserResult(Guid.Empty, string.Empty, string.Empty, string.Empty));
        }
    }
}
