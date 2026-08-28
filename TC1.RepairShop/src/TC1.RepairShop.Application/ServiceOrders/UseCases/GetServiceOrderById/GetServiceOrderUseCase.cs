using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.ServiceOrders.UseCases;

public class GetServiceOrderUseCase(IServiceOrderRepository _serviceOrderRepository): BaseUseCase<Guid, GetServiceOrderByIdResponse?>
{
    protected override async Task<BaseResponse<GetServiceOrderByIdResponse?>> HandleAsync(Guid id)
    {
        var serviceOrder = await _serviceOrderRepository.GetByIdDetailedAsync(id);
        if (serviceOrder is null)
            throw new BusinessException(BusinessErrors.ServiceOrderErrors.NotFound);
        return new BaseResponse<GetServiceOrderByIdResponse?>(GetServiceOrderByIdResponse.FromDomain(serviceOrder));
    }
}
