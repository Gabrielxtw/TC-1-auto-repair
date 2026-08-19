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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateServiceOrderRequest request)
    {
        var result = await _createServiceOrderUseCase.ExecuteAsync(request);
        return Response(result);
    }

    [HttpPut("Advance")]
    public async Task<IActionResult> Advance([FromBody] AdvanceServiceOrderRequest request)
    {
        try
        {
            var result = await _advanceServiceOrderUseCase.ExecuteAsync(request);
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
