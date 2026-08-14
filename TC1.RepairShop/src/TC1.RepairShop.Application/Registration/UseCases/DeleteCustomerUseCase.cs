namespace TC1.RepairShop.Application.Registration.UseCases;

public record DeleteCustomerResult(bool Success, string? Error);

public class DeleteCustomerUseCase(ICostumerRepository _customerRepository)
{
    public async Task<DeleteCustomerResult> ExecuteAsync(Guid id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer is null)
        {
            return new DeleteCustomerResult(false, "Customer not found.");
        }

        customer.Delete();

        await _customerRepository.UpdateAsync(customer);

        return new DeleteCustomerResult(true, null);
    }
}
