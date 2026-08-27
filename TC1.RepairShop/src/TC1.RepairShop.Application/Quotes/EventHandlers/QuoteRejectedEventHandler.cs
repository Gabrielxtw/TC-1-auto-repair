using System.Threading;
using System.Threading.Tasks;
using TC1.RepairShop.Application.Notifications;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Events;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Quotes.EventHandlers;

public class QuoteRejectedEventHandler(
    IServiceOrderRepository _serviceOrderRepository,
    IEmailSender _emailSender
    ) : IEventHandler<QuoteRejectedEvent>
{
    public async Task Handle(QuoteRejectedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await _serviceOrderRepository.GetByIdDetailedAsync(domainEvent.ServiceOrderId);
            if (order is null) return;
            order.AdvanceTo(ServiceOrderStatus.Cancelled);
            var to = order.User?.Email?.Value ?? "";
            if (!string.IsNullOrWhiteSpace(to))
            {
                var subject = "Ordem Cancelada";
                var body = $"Olá {order.User?.Username},<br/><br/>Você atingiu o limite de rejeições para a ordem {order.Id}.<br/>" +
                    $" Ela será cancelada automaticamente.<br/><br/>Atenciosamente,<br/>Oficina";
                _emailSender.Enqueue(new EmailMessage(to, subject, body));
            }
            await _serviceOrderRepository.UpdateAsync(order);

        }
        catch
        {
            // suppress exceptions to avoid breaking the publisher; in production log the error
        }
    }
}
