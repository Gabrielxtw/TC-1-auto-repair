using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Enums;

namespace TC1.RepairShop.Application.Users.UseCases
{
    public record UserResponse(Guid Id, string Username,string Role, string Status);
    public record UserDetailedResponse(Guid Id, string Username, string document, string email, string Phone, string Role, string Status);
    public record ListUsersResponse(ICollection<UserResponse> Users);
    public record ChangeUserPasswordRequest(Guid Id, string NewPassword);
    public record CreateUserRequest(string Username, string Password, string Document, string Email, UserRole Role, string Phone);
    public record UpdateUserRequest(Guid Id, string Username, UserRole Role);
    public static class UsersDTO
    {
        public static UserResponse ToUserResponse(User user)
        {
            return new UserResponse(user.Id, user.Username, user.Role.ToString(), user.Status.ToString());
        }

        public static UserDetailedResponse ToUserDetailedResponse(User user)
        {
            return new UserDetailedResponse(user.Id, user.Username, user.Document.Value, user.Email.Value, user.Phone, user.Role.ToString(), user.Status.ToString());
        }
        public static ListUsersResponse ToListUsersResponse(IEnumerable<User> users)
        {
            var userResponses = users.Select(user => new UserResponse(user.Id, user.Username, user.Role.ToString(), user.Status.ToString())).ToList();
            return new ListUsersResponse(userResponses);
        }
    }
}
