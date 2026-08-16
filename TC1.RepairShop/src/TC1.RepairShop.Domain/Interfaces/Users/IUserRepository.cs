using TC1.RepairShop.Domain.Entities.Users;

namespace TC1.RepairShop.Domain.Interfaces.Users
{
    public interface IUserRepository: IRepository<User, Guid>
    {
        Task<User?> GetByUsernameAsync(string username);
    }
}
