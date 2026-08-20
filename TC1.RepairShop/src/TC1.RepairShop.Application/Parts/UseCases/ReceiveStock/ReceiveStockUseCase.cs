using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Interfaces.Parts;

namespace TC1.RepairShop.Application.Parts.UseCases
{
    public class ReceiveStockUseCase(IPartRepository partRepository) : BaseUseCase<ReceiveStockRequest, bool>
    {
        public async Task<BaseResponse<bool>> ExecuteAsync(ReceiveStockRequest request)
        {
            try
            {
                Part part = await partRepository.GetByIdAsync(request.Id) ?? throw new BusinessException(BusinessErrors.RequestErrors.NotFound);

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