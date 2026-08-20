using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Interfaces.ServiceOrders;

namespace TC1.RepairShop.Application.ServiceOrders.UseCases;

public class GetServiceOrderUseCase(IServiceOrderRepository _serviceOrderRepository)
{
    public async Task<GetServiceOrderByIdResponse?> ExecuteAsync(Guid id) {
        var serviceOrder = await _serviceOrderRepository.GetByIdDetailedAsync(id);
        return serviceOrder is null ? null : GetServiceOrderByIdResponse.FromDomain(serviceOrder);
    }
}
