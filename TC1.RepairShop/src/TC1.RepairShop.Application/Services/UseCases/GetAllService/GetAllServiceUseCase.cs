using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Services.UseCases;

public class GetAllServiceUseCase(IServiceRepository _serviceRepository) : BaseUseCase<ListServicesResponse>
{
    protected override async Task<BaseResponse<ListServicesResponse>> HandleAsync()
    {
        IEnumerable<Service> services = await _serviceRepository.GetAllAsync();

        return new BaseResponse<ListServicesResponse>(
            data: ServicesDTO.ToListServicesResponse(services),
            success: true
        );
    }
}
