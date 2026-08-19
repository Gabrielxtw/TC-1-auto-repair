using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Interfaces.Services;

namespace TC1.RepairShop.Application.Services.UseCases
{
    public class GetServiceByIdUseCase(IServiceRepository serviceRepository) : BaseUseCase<Guid, GetServiceByIdResponse>
    {
        public async Task<BaseResponse<GetServiceByIdResponse>> ExecuteAsync(Guid id)
        {
            try
            {
                Service service = await serviceRepository.GetByIdAsync(id);

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
