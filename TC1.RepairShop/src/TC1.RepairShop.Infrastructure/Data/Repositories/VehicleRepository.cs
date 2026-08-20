using Microsoft.EntityFrameworkCore;
using TC1.RepairShop.Domain.Entities.Vehicles;
using TC1.RepairShop.Domain.Interfaces.Vehicles;
using TC1.RepairShop.Domain.Vehicles;

namespace TC1.RepairShop.Infrastructure.Data.Repositories;

public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
{
    public VehicleRepository(RepairShopDbContext context) : base(context)
    {
    }

    public async Task<Vehicle?> GetByLicensePlateAsync(string licensePlate)
    {
        if (!LicensePlate.IsValid(licensePlate)) return null;
        var normalized = LicensePlate.Create(licensePlate);
        return await _context.Vehicles.AsNoTracking().FirstOrDefaultAsync(v => v.LicensePlate == normalized);
    }

    public async Task<IEnumerable<Vehicle>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _context.Vehicles.AsNoTracking().Where(v => v.UserId == customerId).ToListAsync();
    }
}
