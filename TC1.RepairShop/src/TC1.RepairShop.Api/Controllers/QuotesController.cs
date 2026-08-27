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
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateQuoteRequest request)
    {
        var result = await _createQuoteUseCase.ExecuteAsync(request);
        if (result.success)
            return CreatedAtAction(nameof(GetMyQuotes), new { id = result.data?.Id }, result);

        return BadRequest(result);
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

    [HttpPut]
    public async Task<IActionResult> UpdateQuote(UpdateQuoteRequest request)
    {
        var result = await _updateQuoteUseCase.ExecuteAsync(request);
        return Ok(result);
    }

}
