using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using TC1.RepairShop.Domain.Entities.Vehicles;
using TC1.RepairShop.Domain.Interfaces;
using TC1.RepairShop.Domain.Vehicles;

namespace TC1.RepairShop.Infrastructure.Data.Repositories;

public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
{
    public VehicleRepository(RepairShopDbContext context) : base(context)
    {
    }

    public async override Task<Vehicle?> GetByIdAsync(Guid id)
    {
        return await _context.Vehicles.Include(v => v.User).FirstOrDefaultAsync(v => v.Id == id);

    }
    public async override Task<IEnumerable<Vehicle>> GetAllAsync()
    {
        return await _context.Vehicles.AsNoTracking().Include(v => v.User).ToListAsync();

    }
    public async Task<Vehicle?> GetByLicensePlateAsync(string licensePlate)
    {
        if (!LicensePlate.IsValid(licensePlate)) return null;
        var normalized = LicensePlate.Create(licensePlate);
        return await _context.Vehicles.AsNoTracking().Include(v => v.User).FirstOrDefaultAsync(v => v.LicensePlate == normalized);
    }

    public async Task<IEnumerable<Vehicle>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _context.Vehicles.AsNoTracking().Include(v => v.User).Where(v => v.UserId == customerId).ToListAsync();
    }
}
