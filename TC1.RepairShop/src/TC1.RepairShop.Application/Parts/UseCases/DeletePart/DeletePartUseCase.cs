using TC1.RepairShop.Domain.CustomExceptions.BusinessException;
using TC1.RepairShop.Domain.Parts;
using TC1.RepairShop.Domain.Parts.Interfaces;

namespace TC1.RepairShop.Application.Parts.UseCases.DeletePart
{
    public class DeletePartUseCase(IPartRepository partRepository)
    {
        public async Task<BaseResponse<bool>> ExecuteAsync(DeletePartRequest request)
        {
            try
            {
                Part part = await partRepository.GetByIdsAsync(request.Id);

                part.Delete();

                await partRepository.UpdateAsync(part);

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
