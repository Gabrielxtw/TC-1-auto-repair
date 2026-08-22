using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.ServiceOrders.UseCases;

public record AttachPartRequest(Guid ServiceOrderId, Guid PartId, int Quantity, decimal Price, bool SuppliedByCustomer);

public class AttachPartUseCase(IServiceOrderRepository _serviceOrderRepository, IPartRepository _partRepository, IServiceOrderPartRepository _serviceOrderPartRepository)
{

    public async Task<BaseResponse<ServiceOrder?>> ExecuteAsync(AttachPartRequest request)
    {
        try
        {
            var order = await _serviceOrderRepository.GetByIdAsync(request.ServiceOrderId);
            if (order is null)
                return new BaseResponse<ServiceOrder?>(data: null, success: false, error: "Service order not found.");

            var part = await _partRepository.GetByIdAsync(request.PartId);
            if (part is null)
                return new BaseResponse<ServiceOrder?>(data: null, success: false, error: "Part not found.", StatusCode: "404");

            ServiceOrderPart? existingPart = await _serviceOrderRepository.GetServiceOrderPartById(request.ServiceOrderId, request.PartId);
            if (existingPart is not null)
                return new BaseResponse<ServiceOrder?>(data: null, success: false, error: "Part already attached to the service order.", StatusCode: "400");


            ServiceOrderPart serviceOrderPart = ServiceOrderPart.Create(request.ServiceOrderId, request.PartId, request.Quantity, request.Price, request.SuppliedByCustomer);

            await _serviceOrderPartRepository.AddAsync(serviceOrderPart);

            return new BaseResponse<ServiceOrder?>(order);
        }
        catch (Exception ex)
        {
            return new BaseResponse<ServiceOrder?>(data: null, success: false, error: ex.Message);
        }
    }
}
