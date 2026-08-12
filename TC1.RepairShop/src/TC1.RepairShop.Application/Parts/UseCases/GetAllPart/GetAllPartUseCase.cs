using TC1.RepairShop.Application.Parts.UseCases.DeletePart;
using TC1.RepairShop.Domain.Parts.Interfaces;

namespace TC1.RepairShop.Application.Parts.UseCases.GetAllPart
{
    public class GetAllPartUseCase(IPartRepository partRepository) : BaseUseCase<IEnumerable<GetAllPartViewModel>>
    {
        public async Task<BaseResponse<IEnumerable<GetAllPartViewModel>>> ExecuteAsync()
        {
            try
            {
                var parts = await partRepository.GetAllAsync();

                return new BaseResponse<IEnumerable<GetAllPartViewModel>>(
                    data: parts.Select(p => new GetAllPartViewModel(id: p.Id, name: p.Name, stockQuantity: p.StockQuantity, UnitPrice: p.UnitPrice)),
                    success: true
                );
            }
            catch (Exception)
            {
                return new BaseResponse<IEnumerable<GetAllPartViewModel>>(data: new List<GetAllPartViewModel>(), success: false);
            }
        }
    }
}
