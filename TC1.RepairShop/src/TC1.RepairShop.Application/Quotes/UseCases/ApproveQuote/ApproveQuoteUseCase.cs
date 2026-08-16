using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Interfaces.Quotes;

namespace TC1.RepairShop.Application.Quotes.UseCases.ApproveQuote;

public record ApproveQuoteRequest(Guid QuoteId);

public class ApproveQuoteUseCase(IQuoteRepository quoteRepository)
{
    public async Task<BaseResponse<bool>> ExecuteAsync(ApproveQuoteRequest request)
    {
        try
        {
            var quote = await quoteRepository.GetByIdAsync(request.QuoteId);
            if (quote is null)
                return new BaseResponse<bool>(data: false, success: false, error: "Quote not found.");

            quote.Approve();
            await quoteRepository.UpdateAsync(quote);
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
