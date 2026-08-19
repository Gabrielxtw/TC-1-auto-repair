using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Interfaces.Parts;

namespace TC1.RepairShop.Application.Parts.UseCases;

public class UpdatePartUseCase(IPartRepository partRepository)
{
    public async Task<BaseResponse<bool>> ExecuteAsync(UpdatePartRequest request)
    {
        try
        {
            var part = await partRepository.GetByIdAsync(request.Id);
            if (part is null)
                return new BaseResponse<bool>(data: false, success: false, error: "Part not found.");

            part.Update(request.Name, request.Price);

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
