using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Users.UseCases;

public record GetUserResponse(Guid Id, string Username,string document, string email, string Role, string Status);
public class GetUserUseCase(IUserRepository _userRepository)//: BaseUseCase<GetUserResponse>
{
    public async Task<BaseResponse<GetUserResponse?>> ExecuteAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null)
            return new BaseResponse<GetUserResponse?>(data: null, success: false, error: "User not found.");
        return new BaseResponse<GetUserResponse?>(new GetUserResponse(user.Id, user.Username, user.Document.Value, user.Email.Value, user.Role.ToString(), user.Status.ToString()));
    }
}
