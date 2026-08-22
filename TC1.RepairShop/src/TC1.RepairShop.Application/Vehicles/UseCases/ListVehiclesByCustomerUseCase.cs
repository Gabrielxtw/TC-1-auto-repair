using TC1.RepairShop.Domain.Entities.Vehicles;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Vehicles.UseCases;

public class ListVehiclesByCustomerUseCase
{
    private readonly IVehicleRepository _vehicleRepository;

    public ListVehiclesByCustomerUseCase(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public Task<IEnumerable<Vehicle>> ExecuteAsync(Guid customerId) => _vehicleRepository.GetByCustomerIdAsync(customerId);
}
