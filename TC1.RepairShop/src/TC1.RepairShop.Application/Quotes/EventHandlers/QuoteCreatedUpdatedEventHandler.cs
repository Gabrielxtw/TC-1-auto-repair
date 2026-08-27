using TC1.RepairShop.Application.Notifications;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Events;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Quotes.EventHandlers;

public class QuoteCreatedUpdatedEventHandler(
    IQuoteRepository _quoteRepository,
    IServiceOrderRepository _serviceOrderRepository,
    IEmailSender _emailSender
    ) : IEventHandler<QuoteCreatedUpdatedEvent>
{
    public async Task Handle(QuoteCreatedUpdatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        try
        {
            var quote = await _quoteRepository.GetByIdAsync(domainEvent.QuoteId);
            if (quote is null)
                return;

            var order = await _serviceOrderRepository.GetByIdDetailedAsync(quote.ServiceOrderId);
            if (order is null)
                return;

            var to = order.User.Email.Value;
            if (!string.IsNullOrWhiteSpace(to))
            {
                var subject = "Diagnóstico concluído";
                var body = $"Olá {order.User.Username},<br/><br/>O diagnóstico da sua ordem {order.Id} foi concluído. Valor estimado: {quote.Price:C}.<br/><br/>Atenciosamente,<br/>Oficina";
                _emailSender.Enqueue(new EmailMessage(to, subject, body));
            }

            quote.SendToCustomer();
            await _quoteRepository.UpdateAsync(quote);

        }
        catch
        {
            // Swallow exceptions to avoid breaking the publisher; in production, log the error
        }
    }
}
