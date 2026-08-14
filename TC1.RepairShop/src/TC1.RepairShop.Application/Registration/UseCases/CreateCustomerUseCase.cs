using TC1.RepairShop.Application.Registration;
using TC1.RepairShop.Domain.Entities.Registration;

namespace TC1.RepairShop.Application.Registration.UseCases;

public record CreateCustomerRequest(string Name, string NationalId, string Phone, string Email);

public record CreateCustomerResult(bool Success, string? Error, Customer? Customer);

public class CreateCustomerUseCase
{
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerUseCase(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CreateCustomerResult> ExecuteAsync(CreateCustomerRequest request)
    {
        var existingCustomer = await _customerRepository.GetByNationalIdAsync(request.NationalId);
        if (existingCustomer is not null)
        {
            return new CreateCustomerResult(false, "National ID is already registered.", null);
        }

        var customer = Customer.Create(request.Name, request.NationalId, request.Phone, request.Email);

        await _customerRepository.AddAsync(customer);

        return new CreateCustomerResult(true, null, customer);
    }
}
