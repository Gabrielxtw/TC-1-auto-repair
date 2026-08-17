using TC1.RepairShop.Domain.Entities.Vehicles;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces.Vehicles;

namespace TC1.RepairShop.UnitTests.Registration;

public class FakeVehicleRepository : IVehicleRepository
{
    private readonly Dictionary<Guid, Vehicle> _vehicles = [];

    public Task<Vehicle?> GetByLicensePlateAsync(string licensePlate)
    {
        var vehicle = _vehicles.Values.SingleOrDefault(v => v.LicensePlate.Value == licensePlate && v.Status != Status.Deleted);
        return Task.FromResult(vehicle);
    }

    public Task<Vehicle?> GetByIdAsync(Guid id)
    {
        _vehicles.TryGetValue(id, out var vehicle);
        return Task.FromResult(vehicle is not null && vehicle.Status != Status.Deleted ? vehicle : null);
    }

    public Task<IEnumerable<Vehicle>> GetByCustomerIdAsync(Guid customerId) =>
        Task.FromResult(_vehicles.Values.Where(v => v.UserId == customerId && v.Status != Status.Deleted));

    public Task<IEnumerable<Vehicle>> GetAllAsync() =>
        Task.FromResult(_vehicles.Values.Where(v => v.Status != Status.Deleted));

    public Task AddAsync(Vehicle vehicle)
    {
        _vehicles[vehicle.Id] = vehicle;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Vehicle vehicle)
    {
        _vehicles[vehicle.Id] = vehicle;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        if (_vehicles.TryGetValue(id, out var vehicle))
        {
            vehicle.Delete();
        }
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(Guid id)
    {

        return Task.FromResult(_vehicles.TryGetValue(id, out var vehicle));
    }
}
