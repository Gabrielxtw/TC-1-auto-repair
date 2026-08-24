using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.ServiceOrders.UseCases;

public record AdvanceServiceOrderRequest(Guid ServiceOrderId, string NewStatus);

public class AdvanceServiceOrderUseCase(IServiceOrderRepository serviceOrderRepository)
{
    public async Task<BaseResponse<ListServiceOrderResponse?>> ExecuteAsync(AdvanceServiceOrderRequest request)
    {
        try
        {
            var order = await serviceOrderRepository.GetByIdDetailedAsync(request.ServiceOrderId);
            if (order is null)
                return new BaseResponse<ListServiceOrderResponse?>(data: null, success: false, error: "Service order not found.");

            var newStatus = ServiceOrderStatus.FromName(request.NewStatus);
            order.AdvanceTo(newStatus);
            await serviceOrderRepository.UpdateAsync(order);
            return new BaseResponse<ListServiceOrderResponse?>(ToResponse(order));
        }
        catch (BusinessException ex)
        {
            return new BaseResponse<ListServiceOrderResponse?>(data: null, success: false, error: ex.Message);
        }
        catch (System.InvalidOperationException)
        {
            return new BaseResponse<ListServiceOrderResponse?>(data: null, success: false, error: "Invalid Status");
        }
        catch (Exception ex)
        {
            return new BaseResponse<ListServiceOrderResponse?>(data: null, success: false, error: ex.Message);
        }
    }
    private static ListServiceOrderResponse ToResponse(ServiceOrder serviceOrder) =>
        new(serviceOrder.Id.ToString(), serviceOrder.User.Username, serviceOrder.OrderStatusValue.ToString(), serviceOrder.OpenedAt.ToString(), serviceOrder.User.Email.Value);
}
