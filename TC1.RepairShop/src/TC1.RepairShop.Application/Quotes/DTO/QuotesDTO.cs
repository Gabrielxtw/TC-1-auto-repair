using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Enums;

namespace TC1.RepairShop.Application.Quotes.UseCases
{
    public record QuoteResponse(Guid Id, Guid ServiceOrderId, decimal Price, string Status, int RejectionCount);
    public record ListQuotesResponse(IEnumerable<QuoteResponse> Quotes);
    public record ApproveQuoteResult(Guid id, decimal price, QuoteStatus status);
    public record CreateQuoteRequest(Guid ServiceOrderId, decimal Price);
    public record CreateQuoteResponse(Guid Id);
    public record UpdateQuoteRequest(Guid Id, decimal Price);

    public static class QuotesDTO
    {
        public static QuoteResponse ToQuoteResponse(Quote q)
        {
            return new QuoteResponse(q.Id, q.ServiceOrderId, q.Price, q.QuoteStatusValue.ToString(), q.RejectionCount);
        }

        public static ListQuotesResponse ToListQuotesResponse(IEnumerable<Quote> quotes)
        {
            var responses = quotes.Select(q => ToQuoteResponse(q)).ToList();
            return new ListQuotesResponse(responses);
        }
    }
}
