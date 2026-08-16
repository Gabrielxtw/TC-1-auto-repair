using TC1.RepairShop.Domain.Entities.Vehicles;

namespace TC1.RepairShop.Domain.Interfaces.Vehicles
{
    public interface IVehicleRepository : IRepository<Vehicle, Guid>
    {
        Task<Vehicle?> GetByLicensePlateAsync(string licensePlate);
        Task<IEnumerable<Vehicle>> GetByCustomerIdAsync(Guid customerId);
    }
}
