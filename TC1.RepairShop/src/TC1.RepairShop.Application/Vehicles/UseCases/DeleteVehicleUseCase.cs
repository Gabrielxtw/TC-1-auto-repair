using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Vehicles.UseCases;


public class DeleteVehicleUseCase(IVehicleRepository _vehicleRepository) : BaseUseCase<Guid, VehicleResponse?>
{
    protected override async Task<BaseResponse<VehicleResponse?>> HandleAsync(Guid id)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id);
        if (vehicle is null)
            throw new BusinessException(BusinessErrors.VehicleErrors.NotFound);


        vehicle.Delete();

        await _vehicleRepository.UpdateAsync(vehicle);

        return new BaseResponse<VehicleResponse?>(data: VehiclesDTO.ToVehicleResponse(vehicle));
    }
}
