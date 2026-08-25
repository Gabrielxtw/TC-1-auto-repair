using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Services.UseCases;

public class DeactiveServiceUseCase(IServiceRepository _serviceRepository) : BaseUseCase<DeactiveServiceRequest, ServiceResponse?>
{
    public async Task<BaseResponse<ServiceResponse?>> ExecuteAsync(DeactiveServiceRequest request)
    {
        try
        {
            Service service = await _serviceRepository.GetByIdAsync(request.Id) ?? throw new BusinessException(BusinessErrors.RequestErrors.NotFound);

            service.Deactivate();

            await _serviceRepository.UpdateAsync(service);

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
