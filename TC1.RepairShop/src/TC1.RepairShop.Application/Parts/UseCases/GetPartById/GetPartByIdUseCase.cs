using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Parts.UseCases
{
    public class GetPartByIdUseCase(IPartRepository partRepository) : BaseUseCase<Guid, PartResponse?>
    {
        public async Task<BaseResponse<PartResponse?>> ExecuteAsync(Guid id)
        {
            try
            {
                Part part = await partRepository.GetByIdAsync(id) ?? throw new BusinessException(BusinessErrors.RequestErrors.NotFound);

                return new BaseResponse<PartResponse?>(
                    data: PartDTO.ToPartResponse(part),
                    success: true
                );
            }
            catch (BusinessException ex)
            {
                return new BaseResponse<PartResponse?>(
                    data: null,
                    success: false,
                    error: ex.Message
                );
            }
            catch (Exception)
            {
                return new BaseResponse<PartResponse?>(
                    data: null,
                    success: false
                );
            }
        }
    }
}
