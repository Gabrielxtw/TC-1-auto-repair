namespace TC1.RepairShop.Application.Registration.UseCases;

public record ChangeCustomerPasswordRequest(Guid Id, string CurrentPassword, string NewPassword);

public record ChangeCustomerPasswordResult(bool Success, string? Error);

public class ChangeCustomerPasswordUseCase
{
    private readonly ICustomerRepository _customerRepository;

    public ChangeCustomerPasswordUseCase(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<ChangeCustomerPasswordResult> ExecuteAsync(ChangeCustomerPasswordRequest request)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id);
        if (customer is null)
        {
            return new ChangeCustomerPasswordResult(false, "Customer not found.");
        }

        if (!customer.VerifyPassword(request.CurrentPassword))
        {
            return new ChangeCustomerPasswordResult(false, "Current password is incorrect.");
        }

        customer.ChangePassword(request.NewPassword);

        await _customerRepository.UpdateAsync(customer);

        return new ChangeCustomerPasswordResult(true, null);
    }
}
