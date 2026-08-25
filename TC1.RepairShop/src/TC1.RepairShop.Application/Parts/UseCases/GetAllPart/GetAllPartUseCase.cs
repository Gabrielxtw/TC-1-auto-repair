using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Parts.UseCases;

public class GetAllPartUseCase(IPartRepository _partRepository) : BaseUseCase<ListPartsResponse>
{
    public async Task<BaseResponse<ListPartsResponse>> ExecuteAsync()
    {
        try
        {
            var parts = await _partRepository.GetAllAsync();

            return new BaseResponse<ListPartsResponse>(
                data: PartDTO.ToListPartsResponse(parts),
                success: true
            );
        }
        catch (Exception)
        {
            return new BaseResponse<ListPartsResponse>(data: new ListPartsResponse(new List<PartResponse>()), success: false);
        }
    }
}
