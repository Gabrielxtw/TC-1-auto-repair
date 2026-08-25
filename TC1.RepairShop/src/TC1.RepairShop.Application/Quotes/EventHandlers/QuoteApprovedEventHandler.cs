using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Events;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Quotes.EventHandlers;

public class QuoteApprovedEventHandler(
    IServiceOrderRepository _serviceOrderRepository,
    IPartRepository _partRepository,
    IServiceOrderPartRepository _serviceOrderPartRepository
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
            var serviceOrderParts = await _serviceOrderPartRepository.GetByServiceOrderIdAsync(order.Id);
            foreach (var servicePart in serviceOrderParts)
            {
                var part = await _partRepository.GetByIdAsync(servicePart.PartId);
                // TODO handle error cases
                if (part is null) 
                    throw new InvalidOperationException("Peça não encontrada.");
                if(part.StockQuantity < servicePart.Quantity)
                    throw new InvalidOperationException("Peça não encontrada.");
                
                part.ConsumeStock(servicePart.Quantity);
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
