using TC1.RepairShop.Domain.Entities.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Entities.Parts.Interfaces;

namespace TC1.RepairShop.Application.Parts.UseCases.DeactivePart
{
    public class DeactivatePartUseCase(IPartRepository partRepository)
    {
        public async Task<BaseResponse<bool>> ExecuteAsync(DeactivePartRequest request)
        {
			try
			{
                Part part = await partRepository.GetByIdsAsync(request.Id);

                part.Deactivate();

                await partRepository.UpdateAsync(part);

                return new BaseResponse<bool>(true);
            }
            catch (BusinessException ex) {
                return new BaseResponse<bool>(data: false, success: false, error: ex.Message);
            }
            catch (Exception)
			{
                return new BaseResponse<bool>(data: false, success: false);
			}
        }
    }
}
