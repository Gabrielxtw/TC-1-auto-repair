using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Interfaces.ServiceOrders;
using TC1.RepairShop.Domain.Enums;

namespace TC1.RepairShop.Application.ServiceOrders.UseCases.CancelServiceOrder;

public record CancelServiceOrderRequest(Guid id);

public class CancelServiceOrderUseCase(IServiceOrderRepository serviceOrderRepository)
{
    public async Task<BaseResponse<bool>> ExecuteAsync(CancelServiceOrderRequest request)
    {
        try
        {
            var order = await serviceOrderRepository.GetByIdAsync(request.id);
            if (order is null)
                return new BaseResponse<bool>(data: false, success: false, error: "Service order not found.");

            order.AdvanceTo(ServiceOrderStatus.Cancelled);
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
