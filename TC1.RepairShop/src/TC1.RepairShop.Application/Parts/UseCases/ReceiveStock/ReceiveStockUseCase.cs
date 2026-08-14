using TC1.RepairShop.Domain.Entities.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Entities.Parts.Interfaces;

namespace TC1.RepairShop.Application.Parts.UseCases.ReceiveStock
{
    public class ReceiveStockUseCase(IPartRepository partRepository) : BaseUseCase<ReceiveStockRequest, bool>
    {
        public async Task<BaseResponse<bool>> ExecuteAsync(ReceiveStockRequest request)
        {
            try
            {
                Part part = await partRepository.GetByIdsAsync(request.Id);

                part.ReceiveStock(request.Quantity);

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