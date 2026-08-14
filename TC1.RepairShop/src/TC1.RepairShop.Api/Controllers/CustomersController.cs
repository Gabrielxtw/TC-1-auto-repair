using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TC1.RepairShop.Application.Registration.UseCases;
using TC1.RepairShop.Domain.Entities.Registration;

namespace TC1.RepairShop.Api.Controllers;

[ApiController]
[Authorize(Policy = "StaffOnly")]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly CreateCustomerUseCase _createCustomerUseCase;
    private readonly GetCustomerUseCase _getCustomerUseCase;
    private readonly ListCustomersUseCase _listCustomersUseCase;
    private readonly UpdateCustomerUseCase _updateCustomerUseCase;
    private readonly DeleteCustomerUseCase _deleteCustomerUseCase;

    public CustomersController(
        CreateCustomerUseCase createCustomerUseCase,
        GetCustomerUseCase getCustomerUseCase,
        ListCustomersUseCase listCustomersUseCase,
        UpdateCustomerUseCase updateCustomerUseCase,
        DeleteCustomerUseCase deleteCustomerUseCase)
    {
        _createCustomerUseCase = createCustomerUseCase;
        _getCustomerUseCase = getCustomerUseCase;
        _listCustomersUseCase = listCustomersUseCase;
        _updateCustomerUseCase = updateCustomerUseCase;
        _deleteCustomerUseCase = deleteCustomerUseCase;
    }

    public record CreateRequest(string Name, string NationalId, string Phone, string Email);

    public record UpdateRequest(string Phone, string Email);

    public record CustomerResponse(Guid Id, string Name, string NationalId, string Phone, string Email, string Status);

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _listCustomersUseCase.ExecuteAsync();
        return Ok(customers.Select(ToResponse));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var customer = await _getCustomerUseCase.ExecuteAsync(id);
        if (customer is null)
        {
            return NotFound(new { message = "Customer not found." });
        }

        return Ok(ToResponse(customer));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRequest request)
    {
        var result = await _createCustomerUseCase.ExecuteAsync(
            new CreateCustomerRequest(request.Name, request.NationalId, request.Phone, request.Email));

        if (!result.Success)
        {
            return Conflict(new { message = result.Error });
        }

        var response = ToResponse(result.Customer!);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRequest request)
    {
        var result = await _updateCustomerUseCase.ExecuteAsync(
            new UpdateCustomerRequest(id, request.Phone, request.Email));

        if (!result.Success)
        {
            return NotFound(new { message = result.Error });
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _deleteCustomerUseCase.ExecuteAsync(id);

        if (!result.Success)
        {
            return NotFound(new { message = result.Error });
        }

        return NoContent();
    }

    private static CustomerResponse ToResponse(Customer customer) =>
        new(customer.Id, customer.Name, customer.NationalId, customer.Phone, customer.Email, customer.Status.ToString());
}
