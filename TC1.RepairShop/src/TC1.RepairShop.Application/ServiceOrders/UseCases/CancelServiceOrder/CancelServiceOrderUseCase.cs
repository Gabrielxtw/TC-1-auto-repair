using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Interfaces;
using TC1.RepairShop.Domain.Enums;

namespace TC1.RepairShop.Application.ServiceOrders.UseCases;

public record CancelServiceOrderRequest(Guid id);

public class CancelServiceOrderUseCase(IServiceOrderRepository serviceOrderRepository)
{
    public async Task<BaseResponse<ServiceOrderListResponse?>> ExecuteAsync(CancelServiceOrderRequest request)
    {
        try
        {
            var order = await serviceOrderRepository.GetByIdAsync(request.id);
            if (order is null)
                return new BaseResponse<ServiceOrderListResponse?>(data: null, success: false, error: "Service order not found.");

            order.AdvanceTo(ServiceOrderStatus.Cancelled);
            await serviceOrderRepository.UpdateAsync(order);

            return new BaseResponse<ServiceOrderListResponse?>(ServiceOrdersDTO.ToListResponse(order));
        }
        catch (BusinessException ex)
        {
            return new BaseResponse<ServiceOrderListResponse?>(data: null, success: false, error: ex.Message, StatusCode: ex.StatusCode.ToString());
        }
        catch (Exception)
        {
            return new BaseResponse<ServiceOrderListResponse?>(data: null, success: false, error: "An unexpected error occurred.");
        }
    }
}
