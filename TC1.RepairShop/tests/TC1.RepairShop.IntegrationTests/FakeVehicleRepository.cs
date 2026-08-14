using System.Collections.Concurrent;
using TC1.RepairShop.Application.Registration;
using TC1.RepairShop.Domain.Entities.Common;
using TC1.RepairShop.Domain.Entities.Registration;

namespace TC1.RepairShop.IntegrationTests;

public class FakeVehicleRepository : IVehicleRepository
{
    private static readonly ConcurrentDictionary<Guid, Vehicle> Vehicles = new();

    public Task<Vehicle?> GetByLicensePlateAsync(string licensePlate)
    {
        var vehicle = Vehicles.Values.SingleOrDefault(v => v.LicensePlate == licensePlate && v.Status != Status.Deleted);
        return Task.FromResult(vehicle);
    }

    public Task<Vehicle?> GetByIdAsync(Guid id)
    {
        Vehicles.TryGetValue(id, out var vehicle);
        return Task.FromResult(vehicle is not null && vehicle.Status != Status.Deleted ? vehicle : null);
    }

    public Task<IEnumerable<Vehicle>> GetByCustomerIdAsync(Guid customerId)
    {
        return Task.FromResult(Vehicles.Values.Where(v => v.CustomerId == customerId && v.Status != Status.Deleted));
    }

    public Task<IEnumerable<Vehicle>> GetAllAsync()
    {
        return Task.FromResult(Vehicles.Values.Where(v => v.Status != Status.Deleted));
    }

    public Task AddAsync(Vehicle vehicle)
    {
        Vehicles[vehicle.Id] = vehicle;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Vehicle vehicle)
    {
        Vehicles[vehicle.Id] = vehicle;
        return Task.CompletedTask;
    }
}
