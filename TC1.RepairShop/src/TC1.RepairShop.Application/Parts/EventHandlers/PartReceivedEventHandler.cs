using TC1.RepairShop.Application.Notifications;
using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Events;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.ServiceOrders.EventHandlers;

public class PartReceivedEventHandler (
    IPartRepository _partRepository,
    IServiceOrderPartRepository _serviceOrderPartRepository,
    IEmailSender _emailSender
    ) : IEventHandler<PartReceivedEvent>
{

    public async Task Handle(PartReceivedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        try
        {
            Part? part = null;
            part = await _partRepository.GetByIdAsync(domainEvent.PartId);
            if (part is null)
            {
                throw new BusinessException(BusinessErrors.EntityErrors.NotFound);
            }
            var orders = await _serviceOrderPartRepository.GetByPartIdAsync(domainEvent.PartId);
            string orderIds = string.Join(", ", 
                orders.Where(o => o.ServiceOrder.OrderStatusValue == ServiceOrderStatus.AwaitingApproval)
                .Where(o => o.Quantity > part.StockQuantity-domainEvent.Quantity)
                .Select(o => o.ServiceOrderId)
                );

            var subject = "Partes Recebidas";
            var body = $"Olá ,<br/><br/>As peças que você solicitou estão disponíveis em nosso estoque.<br/><br/>" +
                $"As seguintes ordens estão prontas para serem atendidas: {orderIds}.<br/><br/>" +
                $">Atenciosamente,<br/>Oficina";

            _emailSender.Enqueue(new EmailMessage("to", subject, body));

        }
        catch
        {
            // Swallow exceptions to avoid breaking the publisher; in a real app log the error
        }
    }
}
