using Microsoft.EntityFrameworkCore;
using TC1.RepairShop.Domain.Entities.Vehicles;
using TC1.RepairShop.Domain.Interfaces.Vehicles;
using TC1.RepairShop.Infrastructure.Data;

namespace TC1.RepairShop.Infrastructure.Data.Repositories;

public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
{
    public VehicleRepository(RepairShopDbContext context) : base(context)
    {
    }

    public async Task<Vehicle?> GetByLicensePlateAsync(string licensePlate)
    {
        if (string.IsNullOrWhiteSpace(licensePlate)) return null;
        var normalized = new string(licensePlate.Where(char.IsLetterOrDigit).ToArray()).ToUpperInvariant();
        return await _context.Vehicles.AsNoTracking().FirstOrDefaultAsync(v => v.LicensePlate.Value == normalized);
    }

    public async Task<IEnumerable<Vehicle>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _context.Vehicles.AsNoTracking().Where(v => v.UserId == customerId).ToListAsync();
    }
}
