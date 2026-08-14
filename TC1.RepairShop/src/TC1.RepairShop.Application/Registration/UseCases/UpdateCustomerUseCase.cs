namespace TC1.RepairShop.Application.Registration.UseCases;

public record UpdateCustomerRequest(Guid Id, string Phone, string Email);

public record UpdateCustomerResult(bool Success, string? Error);

public class UpdateCustomerUseCase(ICostumerRepository _customerRepository)
{

    public async Task<UpdateCustomerResult> ExecuteAsync(UpdateCustomerRequest request)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id);
        if (customer is null)
        {
            return new UpdateCustomerResult(false, "Customer not found.");
        }

        customer.UpdateContactInfo(request.Phone, request.Email);

        await _customerRepository.UpdateAsync(customer);

        return new UpdateCustomerResult(true, null);
    }
}
