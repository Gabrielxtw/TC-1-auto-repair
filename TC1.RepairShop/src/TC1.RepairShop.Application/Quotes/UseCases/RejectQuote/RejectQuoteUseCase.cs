using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Quotes.UseCases;

public record RejectQuoteRequest(Guid QuoteId);
public record RejectQuoteResult(Guid id, decimal price, QuoteStatus status);

public class RejectQuoteUseCase(IQuoteRepository quoteRepository): BaseUseCase<Guid, RejectQuoteResult?>
{
    protected override async Task<BaseResponse<RejectQuoteResult?>> HandleAsync(Guid request)
    {
        var quote = await quoteRepository.GetByIdAsync(request) ?? throw new BusinessException(BusinessErrors.EntityErrors.NotFound);

        quote.Reject();
        await quoteRepository.UpdateAsync(quote);
        return new BaseResponse<RejectQuoteResult?>(new RejectQuoteResult(quote.Id, quote.Price, quote.QuoteStatusValue));
    }
}
