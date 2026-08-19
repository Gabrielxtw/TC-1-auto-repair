using TC1.RepairShop.Domain.Entities.Vehicles;
using TC1.RepairShop.Domain.Interfaces.Vehicles;

namespace TC1.RepairShop.Application.Vehicles.UseCases;

public class ListVehiclesUseCase
{
    private readonly IVehicleRepository _vehicleRepository;

    public ListVehiclesUseCase(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public Task<IEnumerable<Vehicle>> ExecuteAsync() => _vehicleRepository.GetAllAsync();
}
