using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Services.UseCases;

public class DeactiveServiceUseCase(IServiceRepository _serviceRepository) : BaseUseCase<DeactiveServiceRequest, ServiceResponse?>
{
    protected override async Task<BaseResponse<ServiceResponse?>> HandleAsync(DeactiveServiceRequest request)
    {
        Service service = await _serviceRepository.GetByIdAsync(request.Id) ?? throw new BusinessException(BusinessErrors.EntityErrors.NotFound);
        if(service.DeletedAt is not null) throw new BusinessException(BusinessErrors.EntityErrors.CannotDoActionInactiveEntity);

        service.Deactivate();

        await _serviceRepository.UpdateAsync(service);

        return new BaseResponse<ServiceResponse?>(data: ServicesDTO.ToServiceResponse(service), success: true);
    }
}
