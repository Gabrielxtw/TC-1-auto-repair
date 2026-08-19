namespace TC1.RepairShop.Application.Vehicles.UseCases;
using TC1.RepairShop.Domain.Interfaces.Vehicles;

public record DeleteVehicleResult(bool Success, string? Error);

public class DeleteVehicleUseCase
{
    private readonly IVehicleRepository _vehicleRepository;

    public DeleteVehicleUseCase(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<DeleteVehicleResult> ExecuteAsync(Guid id)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id);
        if (vehicle is null)
        {
            return new DeleteVehicleResult(false, "Vehicle not found.");
        }

        vehicle.Delete();

        await _vehicleRepository.UpdateAsync(vehicle);

        return new DeleteVehicleResult(true, null);
    }
}
