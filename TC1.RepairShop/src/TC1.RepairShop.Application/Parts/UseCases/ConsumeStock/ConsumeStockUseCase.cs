using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Parts.UseCases;

public class ConsumeStockUseCase(IPartRepository _partRepository) : BaseUseCase<ConsumeStockRequest, PartResponse?>
{
    public async Task<BaseResponse<PartResponse?>> ExecuteAsync(ConsumeStockRequest request)
    {
        try
        {
            Part part = await _partRepository.GetByIdAsync(request.Id) ?? throw new BusinessException(BusinessErrors.RequestErrors.NotFound);

            part.ConsumeStock(request.Quantity);

            await _partRepository.UpdateAsync(part);

            return new BaseResponse<PartResponse?>(data: PartDTO.ToPartResponse(part), success: true);
        }
        catch (BusinessException ex)
        {
            return new BaseResponse<PartResponse?>(data: null, success: false, error: ex.Message, StatusCode: ex.StatusCode.ToString());
        }
        catch (Exception)
        {
            return new BaseResponse<PartResponse?>(data: null, success: false);
        }
    }
}
