using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Interfaces.ServiceOrders;

namespace TC1.RepairShop.Application.ServiceOrders.UseCases;

public record AttachServiceRequest(Guid ServiceOrderId, ICollection<Guid> ServiceIds);

public class AttachServiceUseCase
{
    private readonly IServiceOrderRepository _serviceOrderRepository;

    public AttachServiceUseCase(IServiceOrderRepository serviceOrderRepository)
    {
        _serviceOrderRepository = serviceOrderRepository;
    }

    public async Task<BaseResponse<bool>> ExecuteAsync(AttachServiceRequest request)
    {
        try
        {
            var order = await _serviceOrderRepository.GetByIdAsync(request.ServiceOrderId);
            if (order is null)
                return new BaseResponse<bool>(data: false, success: false, error: "Service order not found.");

            order.AttachServices(request.ServiceIds);
            await _serviceOrderRepository.UpdateAsync(order);

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
