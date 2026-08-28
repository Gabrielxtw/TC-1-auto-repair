using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.ServiceOrders.UseCases;

public record AttachPartRequest(Guid ServiceOrderId, Guid PartId, int Quantity, decimal Price, bool SuppliedByCustomer);

public class AttachPartUseCase(IServiceOrderRepository _serviceOrderRepository, IPartRepository _partRepository, IServiceOrderPartRepository _serviceOrderPartRepository) : BaseUseCase<AttachPartRequest, ServiceOrderListResponse?>
{

    protected override async Task<BaseResponse<ServiceOrderListResponse?>> HandleAsync(AttachPartRequest request)
    {
        var order = await _serviceOrderRepository.GetByIdAsync(request.ServiceOrderId);
        if (order is null)
            throw new BusinessException(BusinessErrors.ServiceOrderErrors.NotFound);

        var part = await _partRepository.GetByIdAsync(request.PartId);
        if (part is null)
            throw new BusinessException(BusinessErrors.PartErrors.NotFound);

        ServiceOrderPart? existingPart = await _serviceOrderRepository.GetServiceOrderPartById(request.ServiceOrderId, request.PartId);
        if (existingPart is not null)
            throw new BusinessException(BusinessErrors.PartErrors.DuplicatePart);

        ServiceOrderPart serviceOrderPart = ServiceOrderPart.Create(request.ServiceOrderId, request.PartId, request.Quantity, request.Price, request.SuppliedByCustomer);

        await _serviceOrderPartRepository.AddAsync(serviceOrderPart);

        var detailed = await _serviceOrderRepository.GetByIdDetailedAsync(request.ServiceOrderId);
        if (detailed is null)
            throw new BusinessException(BusinessErrors.EntityErrors.NotFound);
        return new BaseResponse<ServiceOrderListResponse?>(ServiceOrdersDTO.ToListResponse(detailed));
    }
}
