using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces.Users;
using TC1.RepairShop.Domain.CustomExceptions;

namespace TC1.RepairShop.Application.Users.UseCases;

public record CreateUserRequest(string Username, string Password, string Document, string Email, UserRole Role, string Phone);

public record CreateUserResult(bool Success, string? Error, User? User);

public class CreateUserUseCase(IUserRepository _userRepository)
{
    public async Task<BaseResponse<CreateUserResult?>> ExecuteAsync(CreateUserRequest request)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
        if (existingUser is not null)
        {
            return new BaseResponse<CreateUserResult?>(new CreateUserResult(false, "Username is already taken.", null));
        }

        try
        {
            var user = User.Create(request.Username, request.Password, request.Document, request.Email, request.Role, request.Phone);
            await _userRepository.AddAsync(user);
            return new BaseResponse<CreateUserResult?>(new CreateUserResult(true, null, user));
        }
        catch (BusinessException ex)
        {
            return new BaseResponse<CreateUserResult?>(new CreateUserResult(false, ex.Message, null));
        }
    }
}
