using Microsoft.EntityFrameworkCore;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Interfaces.Parts;
using TC1.RepairShop.Infrastructure.Data;

namespace TC1.RepairShop.Infrastructure.Data.Repositories;

public class PartRepository : GenericRepository<Part>, IPartRepository
{
    public PartRepository(RepairShopDbContext context) : base(context)
    {
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return false;
        return await _context.Parts.AnyAsync(p => p.Name == name);
    }
}
