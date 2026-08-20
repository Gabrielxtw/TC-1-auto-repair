using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TC1.RepairShop.Application.ServiceOrders.UseCases;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Entities.Users;
using static TC1.RepairShop.Api.Controllers.UsersController;

namespace TC1.RepairShop.Api.Controllers;

[Authorize(Policy = "StaffOnly")]
public class ServiceOrdersController(CreateServiceOrderUseCase _createServiceOrderUseCase,
                                     AdvanceServiceOrderUseCase _advanceServiceOrderUseCase,
                                     CancelServiceOrderUseCase _cancelServiceOrderUseCase,
                                     ListServiceOrdersUseCase _listServiceOrdersUseCase,
                                     AttachPartUseCase _attachPartUseCase,
                                     AttachServiceUseCase _attachServiceUseCase,
                                     GetServiceOrderUseCase _getServiceOrderUseCase
    ) : BaseController
{
    public record ServiceOrderResponse(string ServiceOrderId, string CustomerName, string Status, string OpenedAt, string CustomerEmail);

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _listServiceOrdersUseCase.ExecuteAsync();
        if (!result.success)
            return StatusCode(int.TryParse(result.StatusCode, out var code) ? code : 500, result.error);

        return Ok(result.data.Select(ToResponse));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _getServiceOrderUseCase.ExecuteAsync(id);
        if (!result.success)
            return StatusCode(int.TryParse(result.StatusCode, out var code) ? code : 500, result.error);

        if (result.data is null)
            return NotFound();

        return Ok(result.data);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateServiceOrderRequest request)
    {
        var result = await _createServiceOrderUseCase.ExecuteAsync(request);
        if (!result.success)
            return StatusCode(int.TryParse(result.StatusCode, out var code) ? code : 500, result.error);

        return CreatedAtAction(nameof(GetById), new { id = result.data.Id }, result);
    }

    [HttpPut("Advance")]
    public async Task<IActionResult> Advance([FromBody] AdvanceServiceOrderRequest request)
    {
        try
        {
            var result = await _advanceServiceOrderUseCase.ExecuteAsync(request);
            return Ok(result);
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
        if (!result.success)
            return StatusCode(int.TryParse(result.StatusCode, out var code) ? code : 500, result.error);

        return Ok(result.data);
    }

    [HttpPost("AttachPart")]
    public async Task<IActionResult> AttachPart([FromBody] AttachPartRequest part)
    {
        var result = await _attachPartUseCase.ExecuteAsync(part);
        if (!result.success)
            return StatusCode(int.TryParse(result.StatusCode, out var code) ? code : 500, result.error);

        return Ok(result.data);
    }

    [HttpPost("AttachService")]
    public async Task<IActionResult> AttachService([FromBody] AttachServiceRequest service)
    {
        var result = await _attachServiceUseCase.ExecuteAsync(service);
        if (!result.success)
            return StatusCode(int.TryParse(result.StatusCode, out var code) ? code : 500, result.error);

        return Ok(result.data);
    }


    private static ServiceOrderResponse ToResponse(ServiceOrder serviceOrder) =>
        new(serviceOrder.Id.ToString(), serviceOrder.User.Username, serviceOrder.OrderStatusValue.ToString(), serviceOrder.OpenedAt.ToString(), serviceOrder.User.Email.Value);
}
