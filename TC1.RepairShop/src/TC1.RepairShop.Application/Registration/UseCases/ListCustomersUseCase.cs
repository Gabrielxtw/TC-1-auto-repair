using TC1.RepairShop.Domain.Entities.Costumers;

namespace TC1.RepairShop.Application.Registration.UseCases;

public class ListCustomersUseCase
{
    private readonly ICostumerRepository _customerRepository;

    public ListCustomersUseCase(ICostumerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public Task<IEnumerable<Costumer>> ExecuteAsync() => _customerRepository.GetAllAsync();
}
