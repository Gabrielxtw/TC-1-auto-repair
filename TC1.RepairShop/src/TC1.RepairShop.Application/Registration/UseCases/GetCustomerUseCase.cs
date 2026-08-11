using TC1.RepairShop.Domain.Registration;

namespace TC1.RepairShop.Application.Registration.UseCases;

public class GetCustomerUseCase
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerUseCase(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public Task<Customer?> ExecuteAsync(Guid id) => _customerRepository.GetByIdAsync(id);
}
