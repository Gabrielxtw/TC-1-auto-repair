using Microsoft.EntityFrameworkCore;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Interfaces;
using TC1.RepairShop.Infrastructure.Data;

namespace TC1.RepairShop.Infrastructure.Data.Repositories;

public class ServiceOrderServiceRepository : GenericRepository<ServiceOrderService>, IServiceOrderServiceRepository
{
    public ServiceOrderServiceRepository(RepairShopDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ServiceOrderService>> GetByServiceOrderIdAsync(Guid serviceOrderId)
    {
        return await _context.ServiceOrderServices
            .Where(s => s.ServiceOrderId == serviceOrderId)
            .Include(s => s.Service)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ServiceOrderService?> GetByServiceOrderAndServiceIdAsync(Guid serviceOrderId, Guid serviceId)
    {
        return await _context.ServiceOrderServices
            .Include(s => s.Service)
            .FirstOrDefaultAsync(s => s.ServiceOrderId == serviceOrderId && s.ServiceId == serviceId);
    }
}
