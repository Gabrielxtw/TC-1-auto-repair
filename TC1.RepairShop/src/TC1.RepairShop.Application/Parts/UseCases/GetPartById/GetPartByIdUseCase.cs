using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Parts.UseCases;

public class GetPartByIdUseCase(IPartRepository _partRepository) : BaseUseCase<Guid, PartResponse?>
{
    protected override async Task<BaseResponse<PartResponse?>> HandleAsync(Guid id)
    {
        Part part = await _partRepository.GetByIdAsync(id) ?? throw new BusinessException(BusinessErrors.EntityErrors.NotFound);

        return new BaseResponse<PartResponse?>(
            data: PartDTO.ToPartResponse(part),
            success: true
        );
    }
}
