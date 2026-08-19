using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Interfaces.ServiceOrders;

namespace TC1.RepairShop.Application.ServiceOrders.UseCases;

public record AttachPartRequest(Guid ServiceOrderId, Guid PartId, int Quantity, bool SuppliedByCustomer);

public class AttachPartUseCase
{
    private readonly IServiceOrderRepository _serviceOrderRepository;

    public AttachPartUseCase(IServiceOrderRepository serviceOrderRepository)
    {
        _serviceOrderRepository = serviceOrderRepository;
    }

    public async Task<BaseResponse<bool>> ExecuteAsync(AttachPartRequest request)
    {
        try
        {
            var order = await _serviceOrderRepository.GetByIdAsync(request.ServiceOrderId);
            if (order is null)
                return new BaseResponse<bool>(data: false, success: false, error: "Service order not found.");

            order.AttachPart(request.PartId, request.Quantity, request.SuppliedByCustomer);
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
