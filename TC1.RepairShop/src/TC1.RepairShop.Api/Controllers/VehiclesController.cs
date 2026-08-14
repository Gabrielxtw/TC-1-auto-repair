using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TC1.RepairShop.Application.Registration.UseCases;
using TC1.RepairShop.Domain.Entities.Registration;

namespace TC1.RepairShop.Api.Controllers;

[ApiController]
[Authorize(Policy = "StaffOnly")]
[Route("api/vehicles")]
public class VehiclesController : ControllerBase
{
    private readonly CreateVehicleUseCase _createVehicleUseCase;
    private readonly GetVehicleUseCase _getVehicleUseCase;
    private readonly ListVehiclesUseCase _listVehiclesUseCase;
    private readonly ListVehiclesByCustomerUseCase _listVehiclesByCustomerUseCase;
    private readonly DeleteVehicleUseCase _deleteVehicleUseCase;

    public VehiclesController(
        CreateVehicleUseCase createVehicleUseCase,
        GetVehicleUseCase getVehicleUseCase,
        ListVehiclesUseCase listVehiclesUseCase,
        ListVehiclesByCustomerUseCase listVehiclesByCustomerUseCase,
        DeleteVehicleUseCase deleteVehicleUseCase)
    {
        _createVehicleUseCase = createVehicleUseCase;
        _getVehicleUseCase = getVehicleUseCase;
        _listVehiclesUseCase = listVehiclesUseCase;
        _listVehiclesByCustomerUseCase = listVehiclesByCustomerUseCase;
        _deleteVehicleUseCase = deleteVehicleUseCase;
    }

    public record CreateRequest(Guid CustomerId, string LicensePlate, string Brand, string Model, int Year);

    public record VehicleResponse(Guid Id, Guid CustomerId, string LicensePlate, string Brand, string Model, int Year, string Status);

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? customerId)
    {
        var vehicles = customerId.HasValue
            ? await _listVehiclesByCustomerUseCase.ExecuteAsync(customerId.Value)
            : await _listVehiclesUseCase.ExecuteAsync();

        return Ok(vehicles.Select(ToResponse));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var vehicle = await _getVehicleUseCase.ExecuteAsync(id);
        if (vehicle is null)
        {
            return NotFound(new { message = "Vehicle not found." });
        }

        return Ok(ToResponse(vehicle));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRequest request)
    {
        var result = await _createVehicleUseCase.ExecuteAsync(
            new CreateVehicleRequest(request.CustomerId, request.LicensePlate, request.Brand, request.Model, request.Year));

        if (!result.Success)
        {
            return result.Error == "Customer not found."
                ? NotFound(new { message = result.Error })
                : Conflict(new { message = result.Error });
        }

        var response = ToResponse(result.Vehicle!);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _deleteVehicleUseCase.ExecuteAsync(id);

        if (!result.Success)
        {
            return NotFound(new { message = result.Error });
        }

        return NoContent();
    }

    private static VehicleResponse ToResponse(Vehicle vehicle) =>
        new(vehicle.Id, vehicle.CustomerId, vehicle.LicensePlate, vehicle.Brand, vehicle.Model, vehicle.Year, vehicle.Status.ToString());
}
