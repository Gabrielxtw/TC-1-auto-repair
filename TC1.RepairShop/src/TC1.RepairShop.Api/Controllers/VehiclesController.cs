using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TC1.RepairShop.Application.Vehicles.UseCases;
using TC1.RepairShop.Domain.Entities.Vehicles;

namespace TC1.RepairShop.Api.Controllers;

[Authorize(Policy = "StaffOnly")]
public class VehiclesController(CreateVehicleUseCase _createVehicleUseCase,
        GetVehicleUseCase _getVehicleUseCase,
        ListVehiclesUseCase _listVehiclesUseCase,
        ListVehiclesByCustomerUseCase _listVehiclesByCustomerUseCase,
        DeleteVehicleUseCase _deleteVehicleUseCase) : BaseController
{

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? customerId)
    {
        var result = customerId.HasValue
            ? await _listVehiclesByCustomerUseCase.ExecuteAsync(customerId.Value)
            : await _listVehiclesUseCase.ExecuteAsync();

        return Response(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _getVehicleUseCase.ExecuteAsync(id);
        return Response(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVehicleRequest request)
    {
        var result = await _createVehicleUseCase.ExecuteAsync(request);

        return Response(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _deleteVehicleUseCase.ExecuteAsync(id);
        return Response(result);
    }
}
