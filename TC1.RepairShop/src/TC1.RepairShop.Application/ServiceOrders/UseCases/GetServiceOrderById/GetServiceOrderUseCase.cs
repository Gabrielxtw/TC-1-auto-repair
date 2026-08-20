using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Interfaces.ServiceOrders;

namespace TC1.RepairShop.Application.ServiceOrders.UseCases;

public class GetServiceOrderUseCase(IServiceOrderRepository _serviceOrderRepository)
{
    public async Task<BaseResponse<GetServiceOrderByIdResponse?>> ExecuteAsync(Guid id)
    {
        try
        {
            var serviceOrder = await _serviceOrderRepository.GetByIdDetailedAsync(id);
            if (serviceOrder is null)
                return new BaseResponse<GetServiceOrderByIdResponse?>(data: null, success: false, error: "Service order not found.", StatusCode: "404");
            return new BaseResponse<GetServiceOrderByIdResponse?>(GetServiceOrderByIdResponse.FromDomain(serviceOrder));
        }
        catch (Exception ex)
        {
            return new BaseResponse<GetServiceOrderByIdResponse?>(data: null, success: false, error: ex.Message);
        }
    }
}
