using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Services.UseCases;

public class CreateServiceUseCase(IServiceRepository _serviceRepository) : BaseUseCase<CreateServiceRequest, ServiceResponse?>
{
    protected override async Task<BaseResponse<ServiceResponse?>> HandleAsync(CreateServiceRequest request)
    {
        if (await _serviceRepository.ExistsByNameAsync(request.name))
            throw new BusinessException(BusinessErrors.ServiceErrors.DuplicateService);

        Service service = Service.Create(request.name, request.description, request.price);

        await _serviceRepository.AddAsync(service);

        return new BaseResponse<ServiceResponse?>(data: ServicesDTO.ToServiceResponse(service), success: true);
    }
}
