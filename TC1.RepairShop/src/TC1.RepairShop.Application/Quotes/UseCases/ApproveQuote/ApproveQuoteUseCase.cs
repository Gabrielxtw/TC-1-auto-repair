using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Quotes.UseCases;

public record ApproveQuoteRequest(Guid QuoteId);
public record ApproveQuoteResult(Guid id, decimal price, QuoteStatus status);

public class ApproveQuoteUseCase(IQuoteRepository quoteRepository)
{
    public async Task<BaseResponse<ApproveQuoteResult?>> ExecuteAsync(Guid request)
    {
        try
        {
            var quote = await quoteRepository.GetByIdAsync(request);
            if (quote is null)
                return new BaseResponse<ApproveQuoteResult?>(data: null, success: false, error: "Quote not found.");

            quote.Approve();
            await quoteRepository.UpdateAsync(quote);
            return new BaseResponse<ApproveQuoteResult?>(new ApproveQuoteResult(quote.Id, quote.Price, quote.QuoteStatusValue));
        }
        catch (BusinessException ex)
        {
            return new BaseResponse<ApproveQuoteResult?>(data: null, success: false, error: ex.Message);
        }
        catch (Exception)
        {
            return new BaseResponse<ApproveQuoteResult?>(data: null, success: false);
        }
    }
}
