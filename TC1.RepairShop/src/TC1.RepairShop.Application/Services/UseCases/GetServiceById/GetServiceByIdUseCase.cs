using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Services.UseCases;

public class GetServiceByIdUseCase(IServiceRepository _serviceRepository) : BaseUseCase<Guid, ServiceResponse?>
{
    public async Task<BaseResponse<ServiceResponse?>> ExecuteAsync(Guid id)
    {
        try
        {
            Service service = await _serviceRepository.GetByIdAsync(id) ?? throw new BusinessException(BusinessErrors.RequestErrors.NotFound);

            return new BaseResponse<ServiceResponse?>(data: ServicesDTO.ToServiceResponse(service), success: true);
        }
        catch (BusinessException ex)
        {
            return new BaseResponse<ServiceResponse?>(data: null, success: false, error: ex.Message);
        }
        catch (Exception)
        {
            return new BaseResponse<ServiceResponse?>(data: null, success: false);
        }
    }
}
