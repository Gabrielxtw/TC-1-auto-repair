using TC1.RepairShop.Domain.Entities.Costumers;

namespace TC1.RepairShop.Application.Registration.UseCases;

public class GetCustomerUseCase (ICostumerRepository _customerRepository)
{
    public async Task<Costumer?> ExecuteAsync(Guid id) => await _customerRepository.GetByIdAsync(id);
}
