using TC1.RepairShop.Domain.CustomExceptions.BusinessException;
using TC1.RepairShop.Domain.Services;

namespace TC1.RepairShop.Application.Services.UseCases.GetServiceById
{
    public class GetServiceByIdUseCase(IServiceRepository serviceRepository) : BaseUseCase<Guid, GetServiceByIdResponse>
    {
        public async Task<BaseResponse<GetServiceByIdResponse>> ExecuteAsync(Guid id)
        {
            try
            {
                Service service = await serviceRepository.GetByIdsAsync(id);

                return new BaseResponse<GetServiceByIdResponse>(data: new GetServiceByIdResponse(service.Id, service.Name, service.Description));
            }
            catch (BusinessException ex)
            {
                return new BaseResponse<GetServiceByIdResponse>(data: new GetServiceByIdResponse(Guid.Empty, "", ""), success: false, error: ex.Message);
            }
            catch (Exception)
            {
                return new BaseResponse<GetServiceByIdResponse>(data: new GetServiceByIdResponse(Guid.Empty, "", ""), success: false);
            }
        }
    }
}
