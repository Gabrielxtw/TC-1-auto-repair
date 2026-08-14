using TC1.RepairShop.Domain.Entities.Registration;

namespace TC1.RepairShop.Application.Registration.UseCases;

public class ListCustomersUseCase
{
    private readonly ICustomerRepository _customerRepository;

    public ListCustomersUseCase(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public Task<IEnumerable<Customer>> ExecuteAsync() => _customerRepository.GetAllAsync();
}
