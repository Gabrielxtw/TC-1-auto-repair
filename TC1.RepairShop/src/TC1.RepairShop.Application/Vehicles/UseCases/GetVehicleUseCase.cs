using TC1.RepairShop.Domain.Entities.Vehicles;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Vehicles.UseCases;

public class GetVehicleUseCase
{
    private readonly IVehicleRepository _vehicleRepository;

    public GetVehicleUseCase(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public Task<Vehicle?> ExecuteAsync(Guid id) => _vehicleRepository.GetByIdAsync(id);
}
