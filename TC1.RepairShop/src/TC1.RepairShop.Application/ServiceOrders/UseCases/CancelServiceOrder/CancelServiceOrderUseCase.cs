using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Interfaces;
using TC1.RepairShop.Domain.Enums;

namespace TC1.RepairShop.Application.ServiceOrders.UseCases;

public record CancelServiceOrderRequest(Guid id);

public class CancelServiceOrderUseCase(IServiceOrderRepository serviceOrderRepository): BaseUseCase<CancelServiceOrderRequest, ServiceOrderListResponse?>
{
    protected override async Task<BaseResponse<ServiceOrderListResponse?>> HandleAsync(CancelServiceOrderRequest request)
    {
        var order = await serviceOrderRepository.GetByIdAsync(request.id);
        if (order is null)
            throw new BusinessException(BusinessErrors.ServiceOrderErrors.NotFound);

        order.AdvanceTo(ServiceOrderStatus.Cancelled);
        await serviceOrderRepository.UpdateAsync(order);

        return new BaseResponse<ServiceOrderListResponse?>(ServiceOrdersDTO.ToListResponse(order));
    }
}
