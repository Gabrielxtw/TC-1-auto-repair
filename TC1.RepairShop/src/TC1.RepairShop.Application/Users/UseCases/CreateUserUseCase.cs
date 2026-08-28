using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;
using TC1.RepairShop.Domain.CustomExceptions;
using System.Net;

namespace TC1.RepairShop.Application.Users.UseCases;


public class CreateUserUseCase(IUserRepository _userRepository): BaseUseCase<CreateUserRequest, UserResponse?>
{
    protected override async Task<BaseResponse<UserResponse?>> HandleAsync(CreateUserRequest request)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
        if (existingUser is not null)
        {
            throw new BusinessException(BusinessErrors.UserErrors.DuplicateUsername);
        }

        var user = User.Create(request.Username, request.Password, request.Document, request.Email, request.Role, request.Phone);
        await _userRepository.AddAsync(user);
        return new BaseResponse<UserResponse?>(UsersDTO.ToUserResponse(user), StatusCode: HttpStatusCode.Created);
    }
}
