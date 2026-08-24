using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.ServiceOrders.UseCases;
public record ListServiceOrderResponse(string ServiceOrderId, string CustomerName, string Status, string OpenedAt, string CustomerEmail);

public class ListServiceOrdersUseCase(IServiceOrderRepository serviceOrderRepository)
{

    public async Task<BaseResponse<IEnumerable<ListServiceOrderResponse>>> ExecuteAsync()
    {
        try
        {
            var orders = await serviceOrderRepository.GetAllAsync();
            return new BaseResponse<IEnumerable<ListServiceOrderResponse>>(orders.Select(ToResponse));
        }
        catch (Exception ex)
        {
            return new BaseResponse<IEnumerable<ListServiceOrderResponse>>(Enumerable.Empty<ListServiceOrderResponse>(), success: false, error: ex.Message);
        }
    }


    private static ListServiceOrderResponse ToResponse(ServiceOrder serviceOrder) =>
        new(serviceOrder.Id.ToString(), serviceOrder.User.Username, serviceOrder.OrderStatusValue.ToString(), serviceOrder.OpenedAt.ToString(), serviceOrder.User.Email.Value);
}
