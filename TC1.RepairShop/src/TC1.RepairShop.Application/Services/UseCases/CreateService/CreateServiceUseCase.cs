using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Services.UseCases;

public class CreateServiceUseCase(IServiceRepository _serviceRepository) : BaseUseCase<CreateServiceRequest, ServiceResponse?>
{
    public async Task<BaseResponse<ServiceResponse?>> ExecuteAsync(CreateServiceRequest request)
    {
        try
        {
            if (await _serviceRepository.ExistsByNameAsync(request.name))
                return new BaseResponse<ServiceResponse?>(data: null, success: false, error: "Serviço já está cadastrado no sistema.");

            Service service = Service.Create(request.name, request.description, request.price);

            await _serviceRepository.AddAsync(service);

            return new BaseResponse<ServiceResponse?>(data: ServicesDTO.ToServiceResponse(service), success: true);
        }
        catch (BusinessException ex)
        {
            return new BaseResponse<ServiceResponse?>(data: null, success: false, error: ex.Message, StatusCode: ex.StatusCode.ToString());
        }
        catch (Exception)
        {
            return new BaseResponse<ServiceResponse?>(data: null, success: false);
        }
    }
}
