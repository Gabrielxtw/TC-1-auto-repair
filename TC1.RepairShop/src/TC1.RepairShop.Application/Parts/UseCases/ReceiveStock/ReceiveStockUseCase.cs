using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Parts.UseCases;

public class ReceiveStockUseCase(IPartRepository _partRepository) : BaseUseCase<ReceiveStockRequest, PartResponse?>
{
    protected override async Task<BaseResponse<PartResponse?>> HandleAsync(ReceiveStockRequest request)
    {
        Part part = await _partRepository.GetByIdAsync(request.Id) ?? throw new BusinessException(BusinessErrors.EntityErrors.NotFound);

        part.ReceiveStock(request.Quantity);

        await _partRepository.UpdateAsync(part);

        return new BaseResponse<PartResponse?>(data: PartDTO.ToPartResponse(part), success: true);
    }
}
