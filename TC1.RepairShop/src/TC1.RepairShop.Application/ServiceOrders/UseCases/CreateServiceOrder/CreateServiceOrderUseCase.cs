using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Interfaces.ServiceOrders;

namespace TC1.RepairShop.Application.ServiceOrders.UseCases;

public record CreateServiceOrderRequest(Guid UserId, Guid VehicleId);

public record CreateServiceOrderResponse(Guid Id);

public class CreateServiceOrderUseCase(IServiceOrderRepository serviceOrderRepository)
{
    public async Task<BaseResponse<CreateServiceOrderResponse>> ExecuteAsync(CreateServiceOrderRequest request)
    {
        try
        {
            var order = ServiceOrder.Create(request.UserId, request.VehicleId);
            await serviceOrderRepository.AddAsync(order);
            return new BaseResponse<CreateServiceOrderResponse>(new CreateServiceOrderResponse(order.Id));
        }
        catch (Exception ex)
        {
            return new BaseResponse<CreateServiceOrderResponse>(data: new CreateServiceOrderResponse(Guid.Empty), success: false, error: ex.Message);
        }
    }
}
