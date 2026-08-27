using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.ServiceOrders.UseCases;

public record AdvanceServiceOrderRequest(Guid ServiceOrderId, string NewStatus);

public class AdvanceServiceOrderUseCase(IServiceOrderRepository serviceOrderRepository)
{
    public async Task<BaseResponse<ServiceOrderListResponse?>> ExecuteAsync(AdvanceServiceOrderRequest request)
    {
        try
        {
            var order = await serviceOrderRepository.GetByIdDetailedAsync(request.ServiceOrderId);
            if (order is null)
                return new BaseResponse<ServiceOrderListResponse?>(data: null, success: false, error: "Service order not found.");

            var newStatus = ServiceOrderStatus.FromName(request.NewStatus);
            order.AdvanceTo(newStatus);
            await serviceOrderRepository.UpdateAsync(order);
            return new BaseResponse<ServiceOrderListResponse?>(ServiceOrdersDTO.ToListResponse(order));
        }
        catch (BusinessException ex)
        {
            return new BaseResponse<ServiceOrderListResponse?>(data: null, success: false, error: ex.Message, StatusCode: ex.StatusCode.ToString());
        }
        catch (System.InvalidOperationException)
        {
            return new BaseResponse<ServiceOrderListResponse?>(data: null, success: false, error: "Invalid Status");
        }
        catch (Exception ex)
        {
            return new BaseResponse<ServiceOrderListResponse?>(data: null, success: false, error: ex.Message);
        }
    }

}
