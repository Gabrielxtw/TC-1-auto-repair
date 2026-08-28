using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.ServiceOrders.UseCases;

public record AttachServiceRequest(Guid ServiceOrderId, Guid ServiceId, decimal Price);

public class AttachServiceUseCase(IServiceOrderRepository _serviceOrderRepository, IServiceRepository _serviceRepository, IServiceOrderServiceRepository _serviceOrderServiceRepository): BaseUseCase<AttachServiceRequest, ServiceOrderListResponse?>
{

    protected override async Task<BaseResponse<ServiceOrderListResponse?>> HandleAsync(AttachServiceRequest request)
    {
        var order = await _serviceOrderRepository.GetByIdAsync(request.ServiceOrderId);
        if (order is null)
            throw new BusinessException(BusinessErrors.ServiceOrderErrors.NotFound);

        var service = await _serviceRepository.GetByIdAsync(request.ServiceId);
        if (service is null)
            throw new BusinessException(BusinessErrors.ServiceErrors.NotFound);

        ServiceOrderService? existingService = await _serviceOrderRepository.GetServiceOrderServiceById(request.ServiceOrderId, request.ServiceId);
        if (existingService is not null)
            throw new BusinessException(BusinessErrors.ServiceErrors.DuplicateService);


        ServiceOrderService serviceOrderService = ServiceOrderService.Create(request.ServiceOrderId, request.ServiceId, request.Price);

        await _serviceOrderServiceRepository.AddAsync(serviceOrderService);

        var detailed = await _serviceOrderRepository.GetByIdDetailedAsync(request.ServiceOrderId);
        if(detailed is null)
            throw new BusinessException(BusinessErrors.EntityErrors.NotFound);
        return new BaseResponse<ServiceOrderListResponse?>(ServiceOrdersDTO.ToListResponse(detailed));
    }
}
