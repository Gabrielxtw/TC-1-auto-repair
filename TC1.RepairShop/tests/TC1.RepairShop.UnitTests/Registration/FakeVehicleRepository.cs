using TC1.RepairShop.Application.Registration;
using TC1.RepairShop.Domain.Entities.Common;
using TC1.RepairShop.Domain.Entities.Registration;

namespace TC1.RepairShop.UnitTests.Registration;

public class FakeVehicleRepository : IVehicleRepository
{
    private readonly Dictionary<Guid, Vehicle> _vehicles = [];

    public Task<Vehicle?> GetByLicensePlateAsync(string licensePlate)
    {
        var vehicle = _vehicles.Values.SingleOrDefault(v => v.LicensePlate == licensePlate && v.Status != Status.Deleted);
        return Task.FromResult(vehicle);
    }

    public Task<Vehicle?> GetByIdAsync(Guid id)
    {
        _vehicles.TryGetValue(id, out var vehicle);
        return Task.FromResult(vehicle is not null && vehicle.Status != Status.Deleted ? vehicle : null);
    }

    public Task<IEnumerable<Vehicle>> GetByCustomerIdAsync(Guid customerId) =>
        Task.FromResult(_vehicles.Values.Where(v => v.CustomerId == customerId && v.Status != Status.Deleted));

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
}
