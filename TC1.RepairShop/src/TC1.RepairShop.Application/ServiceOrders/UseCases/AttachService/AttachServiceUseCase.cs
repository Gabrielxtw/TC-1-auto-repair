using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Interfaces.Services;
using TC1.RepairShop.Domain.Interfaces.ServiceOrders;

namespace TC1.RepairShop.Application.ServiceOrders.UseCases;

public record AttachServiceRequest(Guid ServiceOrderId, Guid ServiceId);

public class AttachServiceUseCase(IServiceOrderRepository _serviceOrderRepository, IServiceRepository _serviceRepository)
{

    public async Task<BaseResponse<ServiceOrder?>> ExecuteAsync(AttachServiceRequest request)
    {
        try
        {
            var order = await _serviceOrderRepository.GetByIdAsync(request.ServiceOrderId);
            if (order is null)
                return new BaseResponse<ServiceOrder?>(data: null, success: false, error: "Service order not found.");

            var service = await _serviceRepository.GetByIdAsync(request.ServiceId);
            if (service is null)
                return new BaseResponse<ServiceOrder?>(data: null, success: false, error: "Service not found.", StatusCode: "404");

            ServiceOrderService? existingService = await _serviceOrderRepository.GetServiceOrderServiceById(request.ServiceOrderId, request.ServiceId);
            if (existingService is not null)
                return new BaseResponse<ServiceOrder?>(data: null, success: false, error: "Service already attached to the service order.", StatusCode: "400");

            order.AttachServices(service);
            await _serviceOrderRepository.UpdateAsync(order);

            return new BaseResponse<ServiceOrder?>(order);
        }
        catch (Exception ex)
        {
            return new BaseResponse<ServiceOrder?>(data: null, success: false, error: ex.Message);
        }
    }
}
