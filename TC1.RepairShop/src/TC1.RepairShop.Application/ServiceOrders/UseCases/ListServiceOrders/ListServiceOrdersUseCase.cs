using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Interfaces.ServiceOrders;

namespace TC1.RepairShop.Application.ServiceOrders.UseCases;

public class ListServiceOrdersUseCase(IServiceOrderRepository serviceOrderRepository)
{
    public Task<IEnumerable<ServiceOrder>> ExecuteAsync() => serviceOrderRepository.GetAllAsync();
}
