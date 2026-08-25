using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;
using TC1.RepairShop.Domain.CustomExceptions;

namespace TC1.RepairShop.Application.Users.UseCases;


public class CreateUserUseCase(IUserRepository _userRepository): BaseUseCase<CreateUserRequest, UserResponse?>
{
    public async Task<BaseResponse<UserResponse?>> ExecuteAsync(CreateUserRequest request)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
        if (existingUser is not null)
        {
            return new BaseResponse<UserResponse?>(data: null, success: false, error: "Username is already taken.");
        }

        try
        {
            var user = User.Create(request.Username, request.Password, request.Document, request.Email, request.Role, request.Phone);
            await _userRepository.AddAsync(user);
            return new BaseResponse<UserResponse?>(UsersDTO.ToUserResponse(user));
        }
        catch (BusinessException ex)
        {
            return new BaseResponse<UserResponse?>(data: null, success: false, error: ex.Message);
        }
    }
}
