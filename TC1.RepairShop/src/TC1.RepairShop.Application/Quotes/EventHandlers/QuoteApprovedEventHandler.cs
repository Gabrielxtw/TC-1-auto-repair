using TC1.RepairShop.Application.Notifications;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Events;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Quotes.EventHandlers;

public class QuoteApprovedEventHandler(
    IServiceOrderRepository _serviceOrderRepository,
    IPartRepository _partRepository,
    IServiceOrderPartRepository _serviceOrderPartRepository,
    IQuoteRepository _quoteRepository,
    IEmailSender _emailSender
    ) : IEventHandler<QuoteApprovedEvent>
{
    public async Task Handle(QuoteApprovedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await _serviceOrderRepository.GetByIdDetailedAsync(domainEvent.ServiceOrderId);
            if (order is null)
            {
                return;
            }
            var serviceOrderParts = await _serviceOrderPartRepository.GetByServiceOrderIdAsync(order.Id);
            bool allPartsAvailable = true;
            ICollection<(Part, int)> partsToConsume = new List<(Part, int)>();

            foreach (var servicePart in serviceOrderParts.Where(p => !p.SuppliedByCustomer))
            {
                var part = await _partRepository.GetByIdAsync(servicePart.PartId);
                if (part is null)
                    throw new InvalidOperationException("Part not found.");
                if (part.StockQuantity < servicePart.Quantity)
                {
                    allPartsAvailable = false;
                    break;
                }
                partsToConsume.Add((part, servicePart.Quantity));
            }

            if (!order.QuoteId.HasValue)
                throw new InvalidOperationException("QuoteId is null.");

            var quote = await _quoteRepository.GetByIdAsync(order.QuoteId.Value);
            if (quote is null)
                throw new InvalidOperationException("Quote not found.");


            if (!allPartsAvailable)
            {
                quote.MarkUnderReview();

                var to = order.User?.Email?.Value ?? "";
                if (!string.IsNullOrWhiteSpace(to))
                {
                    var subject = "Partes Em falta";
                    var body = $"Olá {order.User?.Username},<br/><br/>Estamos com algumas peças em falta para a sua ordem {order.Id}.\n" +
                        $" Assim que elas entrarem em nosso estoque, daremos inicio ao trabalho.<br/><br/>Atenciosamente,<br/>Oficina";
                    _emailSender.Enqueue(new EmailMessage(to, subject, body));
                }

                await _quoteRepository.UpdateAsync(quote);
                return;

            }


            foreach (var (part, quantity) in partsToConsume)
            {
                part.ConsumeStock(quantity);
                await _partRepository.Update(part);
            }
            await _partRepository.SaveChangesAsync();

            order.AdvanceTo(ServiceOrderStatus.InProgress);
            await _serviceOrderRepository.UpdateAsync(order);
        }
        catch
        {
            // suppress exceptions to avoid breaking the publisher
        }
    }
}
