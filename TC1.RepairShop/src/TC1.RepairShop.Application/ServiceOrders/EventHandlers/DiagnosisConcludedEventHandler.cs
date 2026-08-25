using MediatR;
using System.Diagnostics;
using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Events;
using TC1.RepairShop.Domain.Interfaces;
using TC1.RepairShop.Application.Notifications;

namespace TC1.RepairShop.Application.ServiceOrders.EventHandlers;

public class DiagnosisConcludedEventHandler (
    IQuoteRepository _quoteRepository,
    IServiceOrderRepository _serviceOrderRepository,
    IEmailSender _emailSender
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



            var order = await _serviceOrderRepository.GetByIdDetailedAsync(domainEvent.ServiceOrderId);
            if (order is not null)
            {
                try
                {
                    var to = order.User?.Email?.Value;
                    if (!string.IsNullOrWhiteSpace(to))
                    {
                        var subject = "Diagnóstico concluído";
                        var body = $"Olá {order.User?.Username},<br/><br/>O diagnóstico da sua ordem {order.Id} foi concluído. Valor estimado: {domainEvent.Price:C}.<br/><br/>Atenciosamente,<br/>Oficina";
                        _emailSender.Enqueue(new EmailMessage(to, subject, body));
                    }
                }
                catch
                {
                    // suppress any email errors to avoid breaking the event handler
                }

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
