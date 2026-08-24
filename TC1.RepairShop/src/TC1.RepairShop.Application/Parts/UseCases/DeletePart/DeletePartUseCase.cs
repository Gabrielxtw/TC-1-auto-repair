using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Parts.UseCases
{
    public class DeletePartUseCase(IPartRepository partRepository): BaseUseCase<DeletePartRequest,PartResponse?>
    {
        public async Task<BaseResponse<PartResponse?>> ExecuteAsync(DeletePartRequest request)
        {
            try
            {
                Part part = await partRepository.GetByIdAsync(request.Id) ?? throw new BusinessException(BusinessErrors.RequestErrors.NotFound);

                part.Delete();

                await partRepository.UpdateAsync(part);

                return new BaseResponse<PartResponse?>(data: PartDTO.ToPartResponse(part), success: true);
            }
            catch (BusinessException ex)
            {
                return new BaseResponse<PartResponse?>(data: null, success: false, error: ex.Message);
            }
            catch (Exception)
            {
                return new BaseResponse<PartResponse?>(data: null, success: false, error: "Ocorreu um erro ao excluir a peça.");
            }
        }
    }
}
