using System.Threading;
using System.Threading.Tasks;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Events;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Quotes.EventHandlers;

public class QuoteApprovedEventHandler(
    IServiceOrderRepository _serviceOrderRepository
    ) : IEventHandler<QuoteApprovedEvent>
{
    public async Task Handle(QuoteApprovedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await _serviceOrderRepository.GetByIdAsync(domainEvent.ServiceOrderId);
            if (order is null)
            {
                return;
            }
                order.AdvanceTo(ServiceOrderStatus.InProgress);
                await _serviceOrderRepository.UpdateAsync(order);
        }
        catch
        {
            // suppress exceptions to avoid breaking the publisher
        }
    }
}
