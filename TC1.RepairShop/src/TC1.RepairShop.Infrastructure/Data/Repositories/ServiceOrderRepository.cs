using Microsoft.EntityFrameworkCore;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Interfaces.ServiceOrders;
using TC1.RepairShop.Infrastructure.Data;

namespace TC1.RepairShop.Infrastructure.Data.Repositories;

public class ServiceOrderRepository : GenericRepository<ServiceOrder>, IServiceOrderRepository
{
    public ServiceOrderRepository(RepairShopDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<ServiceOrder>> GetAllAsync()
    {
        return await _context.ServiceOrders
            .Include(o => o.User)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ServiceOrder?> GetByIdDetailedAsync(Guid id)
    {
        return await _context.ServiceOrders
            .Include(o => o.User)
            .Include(o => o.Services)
            .Include(o => o.ServiceOrderParts).ThenInclude(sop => sop.Part)
            .Include(o => o.Quote)
            .Include(o => o.Vehicle)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<ServiceOrder> GetByUserId(Guid userId)
    {
        return await _context.ServiceOrders.FirstOrDefaultAsync(o => o.UserId == userId)
               ?? throw new InvalidOperationException("ServiceOrder not found for user.");
    }

    public async Task<ServiceOrderService?> GetServiceOrderServiceById(Guid serviceOrderId, Guid serviceId)
    {

        var serviceOrder = await _context.ServiceOrders.Include(o => o.ServiceOrderServices).FirstOrDefaultAsync(o => o.Id == serviceOrderId);

        if (serviceOrder == null)
            throw new InvalidOperationException("ServiceOrder not found.");

        var service = serviceOrder.ServiceOrderServices.FirstOrDefault(p => p.ServiceId == serviceId);

        return service;
    }

    public async Task<ServiceOrderPart?> GetServiceOrderPartById(Guid serviceOrderId, Guid partId)
    {
        var serviceOrder = await _context.ServiceOrders.Include(o => o.ServiceOrderParts).FirstOrDefaultAsync(o => o.Id == serviceOrderId);

        if (serviceOrder == null)
            throw new InvalidOperationException("ServiceOrder not found.");

        var part = serviceOrder.ServiceOrderParts.FirstOrDefault(p => p.PartId == partId);

        return part;
    }
}
