using TC1.RepairShop.Domain.Registration;

namespace TC1.RepairShop.Application.Registration.UseCases;

public class ListVehiclesUseCase
{
    private readonly IVehicleRepository _vehicleRepository;

    public ListVehiclesUseCase(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public Task<IEnumerable<Vehicle>> ExecuteAsync() => _vehicleRepository.GetAllAsync();
}
