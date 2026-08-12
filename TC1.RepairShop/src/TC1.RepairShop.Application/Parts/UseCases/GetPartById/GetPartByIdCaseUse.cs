using TC1.RepairShop.Domain.Common;
using TC1.RepairShop.Domain.CustomExceptions.BusinessException;
using TC1.RepairShop.Domain.Parts;
using TC1.RepairShop.Domain.Parts.Interfaces;

namespace TC1.RepairShop.Application.Parts.UseCases.GetPartById
{
    public class GetPartByIdCaseUse(IPartRepository partRepository) : BaseUseCase<Guid, GetPartByIdResponse>
    {
        public async Task<BaseResponse<GetPartByIdResponse>> ExecuteAsync(Guid id)
        {
            try
            {
                Part part = await partRepository.GetByIdsAsync(id);

                return new BaseResponse<GetPartByIdResponse>(
                    data: new GetPartByIdResponse(
                        id: part.Id,
                        name: part.Name,
                        unitPrice: part.UnitPrice,
                        stockQuantity: part.StockQuantity,
                        minimumQuantity: part.MinimumQuantity,
                        status: part.Status
                        )
                );
            }
            catch (BusinessException ex)
            {
                return new BaseResponse<GetPartByIdResponse>(
                    data: new GetPartByIdResponse(Guid.Empty, "", 0, 0, 0, Status.Deleted),
                    success: false,
                    error: ex.Message
                );
            }
            catch (Exception)
            {
                return new BaseResponse<GetPartByIdResponse>(
                    data: new GetPartByIdResponse(Guid.Empty, "", 0, 0, 0, Status.Deleted),
                    success: false
                );
            }
        }
    }
}
