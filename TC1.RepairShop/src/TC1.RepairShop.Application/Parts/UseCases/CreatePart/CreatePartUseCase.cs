using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Parts.UseCases;

public class CreatePartUseCase(IPartRepository _partRepository) : BaseUseCase<CreatePartRequest, PartResponse?>
{
    protected override async Task<BaseResponse<PartResponse?>> HandleAsync(CreatePartRequest request)
    {
        if (await _partRepository.ExistsByNameAsync(request.Name))
            throw new BusinessException(BusinessErrors.PartErrors.DuplicatePart);

        Part part = Part.Create(request.Name, request.Price, request.StockQuantity);

        await _partRepository.AddAsync(part);

        return new BaseResponse<PartResponse?>(data: PartDTO.ToPartResponse(part), success: true);
    }
}
