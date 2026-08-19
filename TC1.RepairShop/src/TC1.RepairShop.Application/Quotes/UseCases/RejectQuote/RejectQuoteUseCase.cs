using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Interfaces.Quotes;

namespace TC1.RepairShop.Application.Quotes.UseCases;

public record RejectQuoteRequest(Guid QuoteId);

public class RejectQuoteUseCase(IQuoteRepository quoteRepository)
{
    public async Task<BaseResponse<bool>> ExecuteAsync(RejectQuoteRequest request)
    {
        try
        {
            var quote = await quoteRepository.GetByIdAsync(request.QuoteId);
            if (quote is null)
                return new BaseResponse<bool>(data: false, success: false, error: "Quote not found.");

            quote.Reject();
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
