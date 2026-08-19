using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces.ServiceOrders;

namespace TC1.RepairShop.Application.ServiceOrders.UseCases;

public record AdvanceServiceOrderRequest(Guid ServiceOrderId, ServiceOrderStatus NewStatus);

public class AdvanceServiceOrderUseCase(IServiceOrderRepository serviceOrderRepository)
{
    public async Task<BaseResponse<bool>> ExecuteAsync(AdvanceServiceOrderRequest request)
    {
        try
        {
            var order = await serviceOrderRepository.GetByIdAsync(request.ServiceOrderId);
            if (order is null)
                return new BaseResponse<bool>(data: false, success: false, error: "Service order not found.");

            order.AdvanceTo(request.NewStatus);
            await serviceOrderRepository.UpdateAsync(order);
            return new BaseResponse<bool>(true);
        }
        catch (BusinessException ex)
        {
            return new BaseResponse<bool>(data: false, success: false, error: ex.Message);
        }
        catch (Exception)
        {
            return new BaseResponse<bool>(data: false, success: false);
        }
    }
}
