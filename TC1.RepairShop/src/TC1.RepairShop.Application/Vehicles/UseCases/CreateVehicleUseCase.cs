using System.Net;
using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Vehicles;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Vehicles.UseCases;

public record CreateVehicleRequest(Guid CustomerId, string LicensePlate, string Brand, string Model, int Year);

public class CreateVehicleUseCase(IUserRepository _userRepository, IVehicleRepository _vehicleRepository) : BaseUseCase<CreateVehicleRequest, VehicleResponse?>
{
    protected override async Task<BaseResponse<VehicleResponse?>> HandleAsync(CreateVehicleRequest request)
    {
        var customer = await _userRepository.GetByIdAsync(request.CustomerId);
        if (customer is null)
            throw new BusinessException(BusinessErrors.UserErrors.NotFound);

        var existingVehicle = await _vehicleRepository.GetByLicensePlateAsync(request.LicensePlate);
        if (existingVehicle is not null)
            throw new BusinessException(BusinessErrors.LicensePlateErrors.DuplicateLicensePlate);


        var vehicle = Vehicle.Create(request.CustomerId, request.LicensePlate, request.Brand, request.Model, request.Year);

        await _vehicleRepository.AddAsync(vehicle);

        return new BaseResponse<VehicleResponse?>(data: VehiclesDTO.ToVehicleResponse(vehicle), StatusCode: HttpStatusCode.Created);
    }
}
