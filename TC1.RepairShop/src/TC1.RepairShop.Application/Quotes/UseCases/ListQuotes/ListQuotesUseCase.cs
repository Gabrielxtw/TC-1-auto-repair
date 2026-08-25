using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Interfaces;
using TC1.RepairShop.Application.Quotes.UseCases;

namespace TC1.RepairShop.Application.Quotes.UseCases.ListQuotes;

public class ListQuotesUseCase(IQuoteRepository _quoteRepository): BaseUseCase<ListQuotesResponse>
{
    public async Task<BaseResponse<ListQuotesResponse>> ExecuteAsync()
    {
        try
        {
            var quotes = await _quoteRepository.GetAllAsync();
            var dto = QuotesDTO.ToListQuotesResponse(quotes);
            return new BaseResponse<ListQuotesResponse>(dto);
        }
        catch (Exception ex)
        {
            return new BaseResponse<ListQuotesResponse>(new ListQuotesResponse(Enumerable.Empty<QuoteResponse>()), success: false, error: ex.Message);
        }
    }
}
