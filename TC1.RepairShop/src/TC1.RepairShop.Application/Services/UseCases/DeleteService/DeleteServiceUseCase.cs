using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Services.UseCases
{
    public class DeleteServiceUseCase(IServiceRepository serviceRepository) : BaseUseCase<DeleteServiceRequest, bool>
    {
        public async Task<BaseResponse<bool>> ExecuteAsync(DeleteServiceRequest request)
        {
            try
            {
                Service service = await serviceRepository.GetByIdAsync(request.id) ?? throw new BusinessException(BusinessErrors.RequestErrors.NotFound);

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
