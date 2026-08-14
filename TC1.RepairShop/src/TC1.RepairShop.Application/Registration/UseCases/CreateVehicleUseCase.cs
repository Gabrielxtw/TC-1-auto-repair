using TC1.RepairShop.Domain.Entities.Registration;

namespace TC1.RepairShop.Application.Registration.UseCases;

public record CreateVehicleRequest(Guid CustomerId, string LicensePlate, string Brand, string Model, int Year);

public record CreateVehicleResult(bool Success, string? Error, Vehicle? Vehicle);

public class CreateVehicleUseCase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IVehicleRepository _vehicleRepository;

    public CreateVehicleUseCase(ICustomerRepository customerRepository, IVehicleRepository vehicleRepository)
    {
        _customerRepository = customerRepository;
        _vehicleRepository = vehicleRepository;
    }

    public async Task<CreateVehicleResult> ExecuteAsync(CreateVehicleRequest request)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
        if (customer is null)
        {
            return new CreateVehicleResult(false, "Customer not found.", null);
        }

        var existingVehicle = await _vehicleRepository.GetByLicensePlateAsync(request.LicensePlate);
        if (existingVehicle is not null)
        {
            return new CreateVehicleResult(false, "License plate is already registered.", null);
        }

        var vehicle = Vehicle.Create(request.CustomerId, request.LicensePlate, request.Brand, request.Model, request.Year);

        await _vehicleRepository.AddAsync(vehicle);

        return new CreateVehicleResult(true, null, vehicle);
    }
}
