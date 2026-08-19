using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Interfaces.Parts;

namespace TC1.RepairShop.Application.Parts.UseCases
{
    public class CreatePartUseCase(IPartRepository partRepository) : BaseUseCase<CreatePartRequest, bool>
    {
        public async Task<BaseResponse<bool>> ExecuteAsync(CreatePartRequest request)
        {
            try
            {
                if (await partRepository.ExistsByNameAsync(request.Name))
                    return new BaseResponse<bool>(data: false, success: false, error: "Peça já está cadastrada no sistema.");

                Part part = Part.Create(request.Name, request.UnitPrice);

                await partRepository.AddAsync(part);

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
