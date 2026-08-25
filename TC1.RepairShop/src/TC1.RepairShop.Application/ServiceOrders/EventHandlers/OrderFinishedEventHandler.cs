using TC1.RepairShop.Application.Notifications;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Events;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.ServiceOrders.EventHandlers;

public class OrderFinishedEventHandler(
    IServiceOrderRepository _serviceOrderRepository,
    IEmailSender _emailSender
    ) : IEventHandler<OrderFinishedEvent>
{
    public async Task Handle(OrderFinishedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await _serviceOrderRepository.GetByIdDetailedAsync(domainEvent.ServiceOrderId);
            if (order is null)
                return;
            try
            {
                var to = order.User?.Email?.Value;
                if (!string.IsNullOrWhiteSpace(to))
                {
                    var subject = "Ordem concluída";
                    var body = $"Olá {order.User?.Username},<br/><br/>Sua ordem {order.Id} foi concluída. Você ja pode retirar o veículo.<br/><br/>Atenciosamente,<br/>Oficina";
                    _emailSender.Enqueue(new EmailMessage(to, subject, body));
                }
            }
            catch
            {
                // suppress any email errors to avoid breaking the event handler
            }
        }
        catch
        {
            // Swallow exceptions to avoid breaking the publisher; in a real app log the error
        }
    }
}
