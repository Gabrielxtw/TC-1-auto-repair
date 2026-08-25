using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Parts.UseCases;

public class UpdatePartUseCase(IPartRepository _partRepository) : BaseUseCase<UpdatePartRequest, PartResponse?>
{
    public async Task<BaseResponse<PartResponse?>> ExecuteAsync(UpdatePartRequest request)
    {
        try
        {
            var part = await _partRepository.GetByIdAsync(request.Id);
            if (part is null)
                return new BaseResponse<PartResponse?>(data: null, success: false, error: "Part not found.");

            part.Update(request.Name, request.Price);

            await _partRepository.UpdateAsync(part);

            return new BaseResponse<PartResponse?>(data: PartDTO.ToPartResponse(part), success: true);
        }
        catch (BusinessException ex)
        {
            return new BaseResponse<PartResponse?>(data: null, success: false, error: ex.Message);
        }
        catch (Exception)
        {
            return new BaseResponse<PartResponse?>(data: null, success: false);
        }
    }
}
