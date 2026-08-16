using Microsoft.EntityFrameworkCore;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Interfaces.ServiceOrders;
using TC1.RepairShop.Infrastructure.Data;

namespace TC1.RepairShop.Infrastructure.Data.Repositories;

public class ServiceOrderRepository : GenericRepository<ServiceOrder>, IServiceOrderRepository
{
    public ServiceOrderRepository(RepairShopDbContext context) : base(context)
    {
    }

    public async Task<ServiceOrder> GetByUserId(Guid userId)
    {
        return await _context.ServiceOrders.FirstOrDefaultAsync(o => o.UserId == userId)
               ?? throw new InvalidOperationException("ServiceOrder not found for user.");
    }

    public async Task<ServiceOrder> GetByServiceId(Guid serviceId)
    {
        // If ServiceOrder has a collection of services, adapt accordingly. For now, try by QuoteId or throw.
        return await _context.ServiceOrders.FirstOrDefaultAsync(o => o.QuoteId == serviceId)
               ?? throw new InvalidOperationException("ServiceOrder not found for service.");
    }
}
