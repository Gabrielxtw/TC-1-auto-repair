using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Interfaces.Parts;
using TC1.RepairShop.Domain.Interfaces.ServiceOrders;

namespace TC1.RepairShop.Application.ServiceOrders.UseCases;

public record AttachPartRequest(Guid ServiceOrderId, Guid PartId, int Quantity, bool SuppliedByCustomer);

public class AttachPartUseCase(IServiceOrderRepository _serviceOrderRepository, IPartRepository _partRepository, IServiceOrderPartRepository _serviceOrderPartRepository)
{

    public async Task<BaseResponse<bool>> ExecuteAsync(AttachPartRequest request)
    {
        try
        {
            var order = await _serviceOrderRepository.GetByIdAsync(request.ServiceOrderId);
            if (order is null)
                return new BaseResponse<bool>(data: false, success: false, error: "Service order not found.");

            var part = await _partRepository.GetByIdAsync(request.PartId);
            if (part is null)
                return new BaseResponse<bool>(data: false, success: false, error: "Part not found.", StatusCode: "404");

            ServiceOrderPart? existingPart = await _serviceOrderRepository.GetServiceOrderPartById(request.ServiceOrderId, request.PartId);
            if (existingPart is not null)
                return new BaseResponse<bool>(data: false, success: false, error: "Part already attached to the service order.", StatusCode: "400");


            ServiceOrderPart serviceOrderPart = ServiceOrderPart.Create(request.ServiceOrderId, request.PartId, request.Quantity, request.SuppliedByCustomer);

            await _serviceOrderPartRepository.UpdateAsync(serviceOrderPart);

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
