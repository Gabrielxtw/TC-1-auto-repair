using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Quotes.UseCases.ListQuotes;

public class ListQuotesUseCase(IQuoteRepository _quoteRepository)
{
    public async Task<BaseResponse<IEnumerable<Quote>>> ExecuteAsync(object? args = null)
    {
        try
        {
            var quotes = await _quoteRepository.GetAllAsync();
            return new BaseResponse<IEnumerable<Quote>>(quotes);
        }
        catch (Exception ex)
        {
            return new BaseResponse<IEnumerable<Quote>>(Enumerable.Empty<Quote>(), success: false, error: ex.Message);
        }
    }
}
