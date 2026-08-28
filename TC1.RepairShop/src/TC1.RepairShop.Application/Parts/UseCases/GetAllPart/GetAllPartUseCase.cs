using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Parts.UseCases;

public class GetAllPartUseCase(IPartRepository _partRepository) : BaseUseCase<ListPartsResponse>
{
    protected override async Task<BaseResponse<ListPartsResponse>> HandleAsync()
    {
        var parts = await _partRepository.GetAllAsync();

        return new BaseResponse<ListPartsResponse>(
            data: PartDTO.ToListPartsResponse(parts),
            success: true
        );
    }
}
