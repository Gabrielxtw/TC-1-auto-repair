using Microsoft.EntityFrameworkCore;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Interfaces;
using TC1.RepairShop.Infrastructure.Data;

namespace TC1.RepairShop.Infrastructure.Data.Repositories;

public class ServiceOrderPartRepository : GenericRepository<ServiceOrderPart>, IServiceOrderPartRepository
{
    public ServiceOrderPartRepository(RepairShopDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ServiceOrderPart>> GetByServiceOrderIdAsync(Guid serviceOrderId)
    {
        return await _context.ServiceOrderParts
            .Where(p => p.ServiceOrderId == serviceOrderId)
            .Include(p => p.Part)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ServiceOrderPart?> GetByServiceOrderAndPartIdAsync(Guid serviceOrderId, Guid partId)
    {
        return await _context.ServiceOrderParts
            .Include(p => p.Part)
            .FirstOrDefaultAsync(p => p.ServiceOrderId == serviceOrderId && p.PartId == partId);
    }

    public async Task<IEnumerable<ServiceOrderPart>> GetByPartIdAsync(Guid partId)
    {
        return await _context.ServiceOrderParts.Where(p => p.PartId == partId)
            .Include(p => p.ServiceOrder)
            .ToListAsync();
    }
}
