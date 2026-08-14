using TC1.RepairShop.Domain.Entities.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Entities.Parts.Interfaces;

namespace TC1.RepairShop.Application.Parts.UseCases.RegisterPart
{
    public class RegisterPartUseCase(IPartRepository partRepository) : BaseUseCase<RegisterPartRequest, bool>
    {
        public async Task<BaseResponse<bool>> ExecuteAsync(RegisterPartRequest request)
        {
            try
            {
                if (!await partRepository.Exist(request.Name))
                    return new BaseResponse<bool>(data: false, success: false, error: "Peça já está cadastrada no sistema.");

                Part part = Part.Create(request.Name, request.UnitPrice, request.MinimumQuantity);

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
