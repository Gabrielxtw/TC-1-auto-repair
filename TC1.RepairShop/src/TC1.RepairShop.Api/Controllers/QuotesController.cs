using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TC1.RepairShop.Application.Quotes.UseCases;
using TC1.RepairShop.Application.Quotes.UseCases.ListQuotes;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Api.Controllers;

[Authorize(Policy = "StaffOnly")]
public class QuotesController(ListQuotesUseCase _listQuotesUseCase,
                                RejectQuoteUseCase _rejectQuoteUseCase,
                                ApproveQuoteUseCase _approveQuoteUseCase,
                                CreateQuoteUseCase _createQuoteUseCase,
                                UpdateQuoteUseCase _updateQuoteUseCase) : BaseController
{
    //TODO get records from path

    [HttpGet]
    public async Task<IActionResult> GetMyQuotes()
    {
        var result = await _listQuotesUseCase.ExecuteAsync();
        return Response(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateQuoteRequest request)
    {
        var result = await _createQuoteUseCase.ExecuteAsync(request);

        return Response(result);
    }

    [HttpPut("Reject/{id}")]
    public async Task<IActionResult> RejectQuote(Guid id)
    {
        var result = await _rejectQuoteUseCase.ExecuteAsync(id);
        return Response(result);
    }

    [HttpPut("Approve/{id}")]
    public async Task<IActionResult> ApproveQuote(Guid id)
    {
        var result = await _approveQuoteUseCase.ExecuteAsync(id);
        return Response(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateQuote(UpdateQuoteRequest request)
    {
        var result = await _updateQuoteUseCase.ExecuteAsync(request);
        return Response(result);
    }

}
