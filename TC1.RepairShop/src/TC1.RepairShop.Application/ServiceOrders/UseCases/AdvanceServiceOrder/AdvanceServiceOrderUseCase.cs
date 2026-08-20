using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces.ServiceOrders;

namespace TC1.RepairShop.Application.ServiceOrders.UseCases;

public record AdvanceServiceOrderRequest(Guid ServiceOrderId, string NewStatus);

public class AdvanceServiceOrderUseCase(IServiceOrderRepository serviceOrderRepository)
{
    public async Task<BaseResponse<ServiceOrder?>> ExecuteAsync(AdvanceServiceOrderRequest request)
    {
        try
        {
            var order = await serviceOrderRepository.GetByIdAsync(request.ServiceOrderId);
            if (order is null)
                return new BaseResponse<ServiceOrder?>(data: null, success: false, error: "Service order not found.");

            var newStatus = ServiceOrderStatus.FromName(request.NewStatus);
            order.AdvanceTo(newStatus);
            await serviceOrderRepository.UpdateAsync(order);
            return new BaseResponse<ServiceOrder?>(order);
        }
        catch (BusinessException ex)
        {
            return new BaseResponse<ServiceOrder?>(data: null, success: false, error: ex.Message);
        }
        catch (Exception)
        {
            return new BaseResponse<ServiceOrder?>(data: null, success: false);
        }
    }
}
