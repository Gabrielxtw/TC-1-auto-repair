using TC1.RepairShop.Domain.Entities.Vehicles;
using TC1.RepairShop.Domain.Interfaces;
using TC1.RepairShop.Domain.CustomExceptions;

namespace TC1.RepairShop.Application.Vehicles.UseCases;

public class GetVehicleUseCase(IVehicleRepository _vehicleRepository) : BaseUseCase<Guid, VehicleResponse?>
{
    protected override async Task<BaseResponse<VehicleResponse?>> HandleAsync(Guid id)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id);
        if (vehicle is null)
            throw new BusinessException(BusinessErrors.EntityErrors.NotFound);

        return new BaseResponse<VehicleResponse?>(data: VehiclesDTO.ToVehicleResponse(vehicle));
    }
}
