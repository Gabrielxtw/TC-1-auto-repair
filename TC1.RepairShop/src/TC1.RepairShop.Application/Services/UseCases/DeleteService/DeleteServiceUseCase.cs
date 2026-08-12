using TC1.RepairShop.Domain.CustomExceptions.BusinessException;
using TC1.RepairShop.Domain.Services;

namespace TC1.RepairShop.Application.Services.UseCases.DeleteService
{
    public class DeleteServiceUseCase(IServiceRepository serviceRepository) : BaseUseCase<DeleteServiceRequest, bool>
    {
        public async Task<BaseResponse<bool>> ExecuteAsync(DeleteServiceRequest request)
        {
            try
            {
                Service service = await serviceRepository.GetByIdsAsync(request.id);

                service.Delete();

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
