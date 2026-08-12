using TC1.RepairShop.Domain.CustomExceptions.BusinessException;
using TC1.RepairShop.Domain.Services;

namespace TC1.RepairShop.Application.Services.UseCases.DeactiveService
{
    public class DeactiveServiceUseCase(IServiceRepository serviceRepository) : BaseUseCase<DeactiveServiceRequest, bool>
    {
        public async Task<BaseResponse<bool>> ExecuteAsync(DeactiveServiceRequest request)
        {
            try
            {
                Service service = await serviceRepository.GetByIdsAsync(request.id);

                service.Deactivate();

                await serviceRepository.UpdateAsync(service);

                return new BaseResponse<bool>(data: true);
            }
            catch (BusinessException ex)
            {
                return new BaseResponse<bool>(data: false, success: false, error: ex.Message);
            }
            catch (Exception)
            {
                return new BaseResponse<bool>(data: false, success: false);
            }
        }
    }
}
