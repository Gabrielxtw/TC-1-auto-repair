using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Services.UseCases
{
    public class CreateServiceUseCase(IServiceRepository serviceRepository) : BaseUseCase<CreateServiceRequest, bool>
    {
        public async Task<BaseResponse<bool>> ExecuteAsync(CreateServiceRequest request)
        {
            try
            {
                if (await serviceRepository.ExistsByNameAsync(request.name))
                    return new BaseResponse<bool>(data: false, success: false, error: "Serviço já está cadastrado no sistema.");

                Service part = Service.Create(request.name, request.description, request.price);

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
