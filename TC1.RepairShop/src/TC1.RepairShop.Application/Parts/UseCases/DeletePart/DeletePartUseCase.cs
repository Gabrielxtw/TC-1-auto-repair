using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Parts.UseCases;

public class DeletePartUseCase(IPartRepository _partRepository): BaseUseCase<Guid,PartResponse?>
{
    protected override async Task<BaseResponse<PartResponse?>> HandleAsync(Guid request)
    {
        Part part = await _partRepository.GetByIdAsync(request) ?? throw new BusinessException(BusinessErrors.EntityErrors.NotFound);

        part.Delete();

        await _partRepository.UpdateAsync(part);

        return new BaseResponse<PartResponse?>(data: PartDTO.ToPartResponse(part), success: true);
    }
}
