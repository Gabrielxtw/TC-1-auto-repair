using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.ServiceOrders.UseCases;

public record AdvanceServiceOrderRequest(Guid ServiceOrderId, string NewStatus);

public class AdvanceServiceOrderUseCase(IServiceOrderRepository serviceOrderRepository) : BaseUseCase<AdvanceServiceOrderRequest, ServiceOrderListResponse?>
{
    protected override async Task<BaseResponse<ServiceOrderListResponse?>> HandleAsync(AdvanceServiceOrderRequest request)
    {
        var order = await serviceOrderRepository.GetByIdDetailedAsync(request.ServiceOrderId);
        if (order is null)
            throw new BusinessException(BusinessErrors.ServiceOrderErrors.NotFound);

        var newStatus = ServiceOrderStatus.FromName(request.NewStatus);
        order.AdvanceTo(newStatus);
        await serviceOrderRepository.UpdateAsync(order);
        return new BaseResponse<ServiceOrderListResponse?>(ServiceOrdersDTO.ToListResponse(order));
    }

}
