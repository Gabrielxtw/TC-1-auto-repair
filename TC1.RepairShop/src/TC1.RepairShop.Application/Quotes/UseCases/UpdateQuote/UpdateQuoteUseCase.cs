using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Quotes.UseCases;

public class UpdateQuoteUseCase(IQuoteRepository quoteRepository): BaseUseCase<UpdateQuoteRequest, QuoteResponse?>
{
    public async Task<BaseResponse<QuoteResponse?>> ExecuteAsync(UpdateQuoteRequest request)
    {
        try
        {
            var quote = await quoteRepository.GetByIdAsync(request.Id);
            if (quote is null)
                return new BaseResponse<QuoteResponse?>(data: null, success: false, error: "Quote not found.");

            quote.UpdatePrice(request.Price);
            await quoteRepository.UpdateAsync(quote);

            return new BaseResponse<QuoteResponse?>(QuotesDTO.ToQuoteResponse(quote));
        }
        catch (BusinessException ex)
        {
            return new BaseResponse<QuoteResponse?>(data: null, success: false, error: ex.Message);
        }
        catch (Exception)
        {
            return new BaseResponse<QuoteResponse?>(data: null, success: false);
        }
    }
}
