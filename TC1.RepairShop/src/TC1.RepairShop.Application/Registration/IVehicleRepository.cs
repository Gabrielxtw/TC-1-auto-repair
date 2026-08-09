using TC1.RepairShop.Domain.Registration;

namespace TC1.RepairShop.Application.Registration;

public interface IVehicleRepository
{
    Task<Vehicle?> GetByLicensePlateAsync(string licensePlate);
    Task<Vehicle?> GetByIdAsync(Guid id);
    Task<IEnumerable<Vehicle>> GetByCustomerIdAsync(Guid customerId);
    Task<IEnumerable<Vehicle>> GetAllAsync();
    Task AddAsync(Vehicle vehicle);
    Task UpdateAsync(Vehicle vehicle);
}
