using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Quotes.UseCases;

public class UpdateQuoteUseCase(IQuoteRepository quoteRepository): BaseUseCase<UpdateQuoteRequest, QuoteResponse?>
{
    protected override async Task<BaseResponse<QuoteResponse?>> HandleAsync(UpdateQuoteRequest request)
    {
        var quote = await quoteRepository.GetByIdAsync(request.Id);
        if (quote is null)
            throw new BusinessException(BusinessErrors.EntityErrors.NotFound);

        quote.UpdatePrice(request.Price);
        await quoteRepository.UpdateAsync(quote);

        return new BaseResponse<QuoteResponse?>(QuotesDTO.ToQuoteResponse(quote));
    }
}
