using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Services.UseCases;

public class GetServiceByIdUseCase(IServiceRepository _serviceRepository) : BaseUseCase<Guid, ServiceResponse?>
{
    protected override async Task<BaseResponse<ServiceResponse?>> HandleAsync(Guid id)
    {
        Service service = await _serviceRepository.GetByIdAsync(id) ?? throw new BusinessException(BusinessErrors.EntityErrors.NotFound);

        return new BaseResponse<ServiceResponse?>(data: ServicesDTO.ToServiceResponse(service), success: true);
    }
}
