using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TC1.RepairShop.Application.ServiceOrders.UseCases;

namespace TC1.RepairShop.Api.Controllers;

[Authorize(Policy = "StaffOnly")]
public class ServiceOrdersController : BaseController
{
    private readonly CreateServiceOrderUseCase _createServiceOrderUseCase;
    private readonly AdvanceServiceOrderUseCase _advanceServiceOrderUseCase;
    private readonly CancelServiceOrderUseCase _cancelServiceOrderUseCase;

    public ServiceOrdersController(
        CreateServiceOrderUseCase createServiceOrderUseCase,
        AdvanceServiceOrderUseCase advanceServiceOrderUseCase,
        CancelServiceOrderUseCase cancelServiceOrderUseCase)
    {
        _createServiceOrderUseCase = createServiceOrderUseCase;
        _advanceServiceOrderUseCase = advanceServiceOrderUseCase;
        _cancelServiceOrderUseCase = cancelServiceOrderUseCase;
    }

    public record CreateRequest(Guid UserId, Guid VehicleId);
    public record AdvanceRequest(Guid ServiceOrderId, string NewStatus);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRequest request)
    {
        var result = await _createServiceOrderUseCase.ExecuteAsync(new CreateServiceOrderRequest(request.UserId, request.VehicleId));
        return Response(result);
    }

    [HttpPut("Advance")]
    public async Task<IActionResult> Advance([FromBody] AdvanceRequest request)
    {
        // Parse status from string to ServiceOrderStatus enum-like type
        try
        {
            var status = TC1.RepairShop.Domain.Enums.ServiceOrderStatus.FromName(request.NewStatus);
            var result = await _advanceServiceOrderUseCase.ExecuteAsync(new AdvanceServiceOrderRequest(request.ServiceOrderId, status));
            return Response(result);
        }
        catch
        {
            return BadRequest("Invalid status value.");
        }
    }

    [HttpPut("Cancel")]
    public async Task<IActionResult> Cancel([FromBody] CancelServiceOrderRequest request)
    {
        var result = await _cancelServiceOrderUseCase.ExecuteAsync(request);
        return Response(result);
    }
}
