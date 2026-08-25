using TC1.RepairShop.Domain.Entities.Vehicles;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Vehicles.UseCases;

public class ListVehiclesUseCase(IVehicleRepository _vehicleRepository) : BaseUseCase<ListVehiclesResponse>
{
    public async Task<BaseResponse<ListVehiclesResponse>> ExecuteAsync()
    {
        var vehicles = await _vehicleRepository.GetAllAsync();
        return new BaseResponse<ListVehiclesResponse>(data: VehiclesDTO.ToListVehiclesResponse(vehicles));
    }
}
