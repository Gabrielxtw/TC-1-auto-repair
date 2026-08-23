using MediatR;
using System.Diagnostics;
using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Events;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.ServiceOrders.EventHandlers;

public class DiagnosisConcludedEventHandler (
    IQuoteRepository _quoteRepository,
    IServiceOrderRepository _serviceOrderRepository
    ) : IEventHandler<DiagnosisConcludedEvent>
{

    public async Task Handle(DiagnosisConcludedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        try
        {
            Quote? quote = null;
            if (domainEvent.QuoteId != null) {
                quote = await _quoteRepository.GetByIdAsync(domainEvent.QuoteId.Value);
                if (quote is null)
                {
                    throw new BusinessException(BusinessErrors.RequestErrors.NotFound);
                }
            }

            if (quote is not null)
            {
                quote.UpdatePrice(domainEvent.Price);
                await _quoteRepository.UpdateAsync(quote);
                return;
            }
            else
            {
                quote = Quote.Create(domainEvent.ServiceOrderId, domainEvent.Price);
                await _quoteRepository.AddAsync(quote);
            }



            var order = await _serviceOrderRepository.GetByIdAsync(domainEvent.ServiceOrderId);
            if (order is not null)
            {
                order.AttachQuote(quote.Id);
                await _serviceOrderRepository.UpdateAsync(order);
            }
        }
        catch
        {
            // Swallow exceptions to avoid breaking the publisher; in a real app log the error
        }
    }
}
