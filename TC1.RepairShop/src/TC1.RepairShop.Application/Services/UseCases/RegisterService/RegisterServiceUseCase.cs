using TC1.RepairShop.Domain.Entities.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Entities.Services.Interfaces;

namespace TC1.RepairShop.Application.Services.UseCases.RegisterService
{
    public class RegisterServiceUseCase(IServiceRepository serviceRepository) : BaseUseCase<RegisterServiceRequest, bool>
    {
        public async Task<BaseResponse<bool>> ExecuteAsync(RegisterServiceRequest request)
        {
            try
            {
                if (!await serviceRepository.Exist(request.name))
                    return new BaseResponse<bool>(data: false, success: false, error: "Serviço já está cadastrado no sistema.");

                Service part = Service.Create(request.name, request.description);

                await serviceRepository.AddAsync(part);

                return new BaseResponse<bool>(true);
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
