using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.ServiceOrders.UseCases;

public record AttachServiceRequest(Guid ServiceOrderId, Guid ServiceId, decimal Price);

public class AttachServiceUseCase(IServiceOrderRepository _serviceOrderRepository, IServiceRepository _serviceRepository, IServiceOrderServiceRepository _serviceOrderServiceRepository)
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


            ServiceOrderService serviceOrderService = ServiceOrderService.Create(request.ServiceOrderId, request.ServiceId, request.Price);

            await _serviceOrderServiceRepository.AddAsync(serviceOrderService);

            return new BaseResponse<ServiceOrder?>(order);
        }
        catch (Exception ex)
        {
            return new BaseResponse<ServiceOrder?>(data: null, success: false, error: ex.Message);
        }
    }
}
