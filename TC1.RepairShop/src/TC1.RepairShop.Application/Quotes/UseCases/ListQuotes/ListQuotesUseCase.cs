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
    protected override async Task<BaseResponse<ListQuotesResponse>> HandleAsync()
    {
        var quotes = await _quoteRepository.GetAllAsync();
        var dto = QuotesDTO.ToListQuotesResponse(quotes);
        return new BaseResponse<ListQuotesResponse>(dto);
    }
}
