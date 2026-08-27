using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Quotes.UseCases;

public record RejectQuoteRequest(Guid QuoteId);
public record RejectQuoteResult(Guid id, decimal price, QuoteStatus status);

public class RejectQuoteUseCase(IQuoteRepository quoteRepository)
{
    public async Task<BaseResponse<RejectQuoteResult?>> ExecuteAsync(Guid request)
    {
        try
        {
            var quote = await quoteRepository.GetByIdAsync(request);
            if (quote is null)
                throw new BusinessException(BusinessErrors.RequestErrors.NotFound);

            quote.Reject();
            await quoteRepository.UpdateAsync(quote);
            return new BaseResponse<RejectQuoteResult?>(new RejectQuoteResult(quote.Id, quote.Price, quote.QuoteStatusValue));
        }
        catch (BusinessException ex)
        {
            return new BaseResponse<RejectQuoteResult?>(data: null, success: false, error: ex.Message, StatusCode: ex.StatusCode.ToString());
        }
        catch (Exception)
        {
            return new BaseResponse<RejectQuoteResult?>(data: null, success: false);
        }
    }
}
