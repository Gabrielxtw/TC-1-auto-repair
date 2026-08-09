using TC1.RepairShop.Domain.Registration;

namespace TC1.RepairShop.Application.Registration.UseCases;

public class GetVehicleUseCase
{
    private readonly IVehicleRepository _vehicleRepository;

    public GetVehicleUseCase(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public Task<Vehicle?> ExecuteAsync(Guid id) => _vehicleRepository.GetByIdAsync(id);
}
