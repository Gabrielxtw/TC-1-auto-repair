using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Quotes.UseCases;

public class CreateQuoteUseCase(IQuoteRepository quoteRepository, IServiceOrderRepository serviceOrderRepository): BaseUseCase<CreateQuoteRequest, CreateQuoteResponse>
{
    protected override async Task<BaseResponse<CreateQuoteResponse>> HandleAsync(CreateQuoteRequest request)
    {
        var order = await serviceOrderRepository.GetByIdAsync(request.ServiceOrderId) ?? throw new BusinessException(BusinessErrors.ServiceOrderErrors.NotFound);

        var quote = Quote.Create(request.ServiceOrderId, request.Price);
        await quoteRepository.AddAsync(quote);
        // attach quote to order
        order.AttachQuote(quote.Id);
        await serviceOrderRepository.UpdateAsync(order);

        return new BaseResponse<CreateQuoteResponse>(new CreateQuoteResponse(quote.Id));
    }
}
