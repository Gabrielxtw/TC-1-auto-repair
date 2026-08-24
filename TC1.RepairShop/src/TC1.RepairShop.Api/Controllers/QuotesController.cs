using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TC1.RepairShop.Application.Quotes.UseCases;
using TC1.RepairShop.Application.Quotes.UseCases.ListQuotes;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Api.Controllers;

[Authorize(Policy = "StaffOnly")]
public class QuotesController(ListQuotesUseCase _listQuotesUseCase,
                                RejectQuoteUseCase _rejectQuoteUseCase,
                                ApproveQuoteUseCase _approveQuoteUseCase) : BaseController
{
    //TODO get records from path

    [HttpGet]
    public async Task<IActionResult> GetMyQuotes()
    {
        var result = await _listQuotesUseCase.ExecuteAsync();
        return Ok(result);
    }

    [HttpPut("Reject/{id}")]
    public async Task<IActionResult> RejectQuote(Guid id)
    {
        var result = await _rejectQuoteUseCase.ExecuteAsync(id);
        return Ok(result);
    }

    [HttpPut("Approve/{id}")]
    public async Task<IActionResult> ApproveQuote(Guid id)
    {
        var result = await _approveQuoteUseCase.ExecuteAsync(id);
        return Ok(result);
    }

}
