using Microsoft.EntityFrameworkCore;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Interfaces;
using TC1.RepairShop.Infrastructure.Data;

namespace TC1.RepairShop.Infrastructure.Data.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(RepairShopDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return null;

        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
    }
}
