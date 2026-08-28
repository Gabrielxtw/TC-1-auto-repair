using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Parts.UseCases;

public class UpdatePartUseCase(IPartRepository _partRepository) : BaseUseCase<UpdatePartRequest, PartResponse?>
{
    protected override async Task<BaseResponse<PartResponse?>> HandleAsync(UpdatePartRequest request)
    {
        var part = await _partRepository.GetByIdAsync(request.Id) ?? throw new BusinessException(BusinessErrors.PartErrors.NotFound);

        part.Update(request.Name, request.Price);

        await _partRepository.UpdateAsync(part);

        return new BaseResponse<PartResponse?>(data: PartDTO.ToPartResponse(part), success: true);
    }
}
