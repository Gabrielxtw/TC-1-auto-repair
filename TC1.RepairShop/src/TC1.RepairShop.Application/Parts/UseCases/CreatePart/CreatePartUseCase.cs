using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Parts.UseCases;

public class CreatePartUseCase(IPartRepository _partRepository) : BaseUseCase<CreatePartRequest, PartResponse?>
{
    public async Task<BaseResponse<PartResponse?>> ExecuteAsync(CreatePartRequest request)
    {
        try
        {
            if (await _partRepository.ExistsByNameAsync(request.Name))
                return new BaseResponse<PartResponse?>(data: null, success: false, error: "Peça já está cadastrada no sistema.");

            Part part = Part.Create(request.Name, request.Price);

            await _partRepository.AddAsync(part);

            return new BaseResponse<PartResponse?>(data: PartDTO.ToPartResponse(part), success: true);
        }
        catch (BusinessException ex)
        {
            return new BaseResponse<PartResponse?>(data: null, success: false, error: ex.Message);
        }
        catch (Exception)
        {
            return new BaseResponse<PartResponse?>(data: null, success: false, error: "Ocorreu um erro ao criar a peça.");
        }
    }
}
