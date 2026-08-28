using TC1.RepairShop.Domain.Entities.Vehicles;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Vehicles.UseCases;

public class ListVehiclesByCustomerUseCase(IVehicleRepository _vehicleRepository) : BaseUseCase<Guid, ListVehiclesResponse>
{
    protected override async Task<BaseResponse<ListVehiclesResponse>> HandleAsync(Guid customerId)
    {
        var vehicles = await _vehicleRepository.GetByCustomerIdAsync(customerId);
        return new BaseResponse<ListVehiclesResponse>(data: VehiclesDTO.ToListVehiclesResponse(vehicles));
    }
}
