using TC1.RepairShop.Domain.Entities.Vehicles;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Vehicles.UseCases;

public class GetVehicleUseCase(IVehicleRepository _vehicleRepository) : BaseUseCase<Guid, VehicleResponse?>
{
    public async Task<BaseResponse<VehicleResponse?>> ExecuteAsync(Guid id)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id);
        if (vehicle is null)
            return new BaseResponse<VehicleResponse?>(data: null, success: false, error: "Vehicle not found.", StatusCode: "404");

        return new BaseResponse<VehicleResponse?>(data: VehiclesDTO.ToVehicleResponse(vehicle));
    }
}
