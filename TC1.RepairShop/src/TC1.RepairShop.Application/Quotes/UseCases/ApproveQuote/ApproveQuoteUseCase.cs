using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Quotes.UseCases;


public class ApproveQuoteUseCase(IQuoteRepository quoteRepository): BaseUseCase<Guid, ApproveQuoteResult?>
{
    protected override async Task<BaseResponse<ApproveQuoteResult?>> HandleAsync(Guid request)
    {
        var quote = await quoteRepository.GetByIdAsync(request) ?? throw new BusinessException(BusinessErrors.QuoteErrors.NotFound);

        quote.Approve();
        await quoteRepository.UpdateAsync(quote);
        return new BaseResponse<ApproveQuoteResult?>(new ApproveQuoteResult(quote.Id, quote.Price, quote.QuoteStatusValue));
    }
}
