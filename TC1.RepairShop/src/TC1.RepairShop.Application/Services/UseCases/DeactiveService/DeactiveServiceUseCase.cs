using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Interfaces.Services;

namespace TC1.RepairShop.Application.Services.UseCases
{
    public class DeactiveServiceUseCase(IServiceRepository serviceRepository) : BaseUseCase<DeactiveServiceRequest, bool>
    {
        public async Task<BaseResponse<bool>> ExecuteAsync(DeactiveServiceRequest request)
        {
            try
            {
                Service service = await serviceRepository.GetByIdAsync(request.id) ?? throw new BusinessException(BusinessErrors.RequestErrors.NotFound);

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
