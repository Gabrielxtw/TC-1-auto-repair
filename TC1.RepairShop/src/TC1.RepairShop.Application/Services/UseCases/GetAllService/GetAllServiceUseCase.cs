using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Services.UseCases;

public class GetAllServiceUseCase(IServiceRepository _serviceRepository) : BaseUseCase<ListServicesResponse>
{
    public async Task<BaseResponse<ListServicesResponse>> ExecuteAsync()
    {
        try
        {
            IEnumerable<Service> services = await _serviceRepository.GetAllAsync();

            return new BaseResponse<ListServicesResponse>(
                data: ServicesDTO.ToListServicesResponse(services),
                success: true
            );
        }
        catch (BusinessException ex)
        {
            return new BaseResponse<ListServicesResponse>(data: new ListServicesResponse(new List<ServiceResponse>()), success: false, error: ex.Message, StatusCode: ex.StatusCode.ToString());
        }
        catch (Exception)
        {
            return new BaseResponse<ListServicesResponse>(data: new ListServicesResponse(new List<ServiceResponse>()), success: false);
        }
    }
}
