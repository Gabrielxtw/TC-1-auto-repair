using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Services.UseCases
{
    public class GetAllServiceUseCase(IServiceRepository serviceRepository) : BaseUseCase<IEnumerable<GetAllServiceViewModel>>
    {
        public async Task<BaseResponse<IEnumerable<GetAllServiceViewModel>>> ExecuteAsync()
        {
            try
            {
                IEnumerable<Service> services = await serviceRepository.GetAllAsync();

                return new BaseResponse<IEnumerable<GetAllServiceViewModel>>(
                    data: services.Select(s => new GetAllServiceViewModel(id: s.Id, name: s.Name, description: s.Description))
                );
            }
            catch (BusinessException ex)
            {
                return new BaseResponse<IEnumerable<GetAllServiceViewModel>>(data: [], success: false, error: ex.Message);
            }
            catch (Exception)
            {
                return new BaseResponse<IEnumerable<GetAllServiceViewModel>>(data: [], success: false);
            }
        }
    }
}
