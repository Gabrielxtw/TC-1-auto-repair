using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Interfaces.ServiceOrders;

namespace TC1.RepairShop.Application.ServiceOrders.UseCases;

public class ListServiceOrdersUseCase(IServiceOrderRepository serviceOrderRepository)
{
    public async Task<BaseResponse<IEnumerable<ServiceOrder>>> ExecuteAsync()
    {
        try
        {
            var orders = await serviceOrderRepository.GetAllAsync();
            return new BaseResponse<IEnumerable<ServiceOrder>>(orders);
        }
        catch (Exception ex)
        {
            return new BaseResponse<IEnumerable<ServiceOrder>>(Enumerable.Empty<ServiceOrder>(), success: false, error: ex.Message);
        }
    }
}
