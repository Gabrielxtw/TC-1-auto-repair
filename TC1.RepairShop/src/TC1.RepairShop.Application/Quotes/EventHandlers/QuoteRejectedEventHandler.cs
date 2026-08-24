using System.Threading;
using System.Threading.Tasks;
using TC1.RepairShop.Domain.Events;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Quotes.EventHandlers;

public class QuoteRejectedEventHandler(
    IQuoteRepository _quoteRepository
    ) : IEventHandler<QuoteRejectedEvent>
{
    public async Task Handle(QuoteRejectedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        try
        {
            var quote = await _quoteRepository.GetByIdAsync(domainEvent.QuoteId);
            if (quote is null)
                return;

            // TODO check max rejections, update quote and notify customer
            quote.MarkUnderReview();
            await _quoteRepository.UpdateAsync(quote);
        }
        catch
        {
            // suppress exceptions to avoid breaking the publisher; in production log the error
        }
    }
}
