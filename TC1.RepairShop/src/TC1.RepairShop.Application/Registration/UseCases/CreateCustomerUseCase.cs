using TC1.RepairShop.Domain.Entities.Costumers;

namespace TC1.RepairShop.Application.Registration.UseCases;

public record CreateCustomerRequest(string Name, string NationalId, string Phone, string Email);

public record CreateCustomerResult(bool Success, string? Error, Costumer? Customer);

public class CreateCustomerUseCase (ICostumerRepository _customerRepository)
{
    public async Task<CreateCustomerResult> ExecuteAsync(CreateCustomerRequest request)
    {
        var existingCustomer = await _customerRepository.GetByNationalIdAsync(request.NationalId);
        if (existingCustomer is not null)
        {
            return new CreateCustomerResult(false, "National ID is already registered.", null);
        }

        var customer = Costumer.Create(request.Name, request.NationalId, request.Phone, request.Email);

        await _customerRepository.AddAsync(customer);

        return new CreateCustomerResult(true, null, customer);
    }
}
