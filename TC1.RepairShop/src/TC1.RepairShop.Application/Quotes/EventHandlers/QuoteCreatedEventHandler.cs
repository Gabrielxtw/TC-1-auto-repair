using TC1.RepairShop.Domain.Events;
using TC1.RepairShop.Domain.Interfaces;
using TC1.RepairShop.Domain.Enums;

namespace TC1.RepairShop.Application.Quotes.EventHandlers;

public class QuoteCreatedEventHandler(
    IQuoteRepository _quoteRepository
    ) : IEventHandler<QuoteCreatedUpdatedEvent>
{
    public async Task Handle(QuoteCreatedUpdatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        try
        {
            // Ensure the quote exists
            var quote = await _quoteRepository.GetByIdAsync(domainEvent.QuoteId);
            if (quote is null)
                return;

            // TODO send Quote to customer email
            quote.SendToCustomer();
            await _quoteRepository.UpdateAsync(quote);

        }
        catch
        {
            // Swallow exceptions to avoid breaking the publisher; in production, log the error
        }
    }
}
