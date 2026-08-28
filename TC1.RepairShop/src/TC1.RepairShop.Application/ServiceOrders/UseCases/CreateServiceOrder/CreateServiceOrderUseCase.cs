using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.ServiceOrders.UseCases;

public record CreateServiceOrderRequest(Guid UserId, Guid VehicleId);

public record CreateServiceOrderResponse(Guid Id);

public class CreateServiceOrderUseCase(IServiceOrderRepository serviceOrderRepository): BaseUseCase<CreateServiceOrderRequest, CreateServiceOrderResponse>
{
    protected override async Task<BaseResponse<CreateServiceOrderResponse>> HandleAsync(CreateServiceOrderRequest request)
    {
        var order = ServiceOrder.Create(request.UserId, request.VehicleId);
        await serviceOrderRepository.AddAsync(order);
        return new BaseResponse<CreateServiceOrderResponse>(new CreateServiceOrderResponse(order.Id));
    }
}
