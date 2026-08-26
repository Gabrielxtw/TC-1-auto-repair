using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.Application.Quotes.UseCases;

public class CreateQuoteUseCase(IQuoteRepository quoteRepository, IServiceOrderRepository serviceOrderRepository): BaseUseCase<CreateQuoteRequest, CreateQuoteResponse>
{
    public async Task<BaseResponse<CreateQuoteResponse>> ExecuteAsync(CreateQuoteRequest request)
    {
        try
        {
            var order = await serviceOrderRepository.GetByIdAsync(request.ServiceOrderId);
            if (order is null)
                return new BaseResponse<CreateQuoteResponse>(data: new CreateQuoteResponse(Guid.Empty), success: false, error: "Service order not found.");

            var quote = Quote.Create(request.ServiceOrderId, request.Price);
            await quoteRepository.AddAsync(quote);
            // attach quote to order
            order.AttachQuote(quote.Id);
            await serviceOrderRepository.UpdateAsync(order);

            return new BaseResponse<CreateQuoteResponse>(new CreateQuoteResponse(quote.Id));
        }
        catch (BusinessException ex)
        {
            return new BaseResponse<CreateQuoteResponse>(data: new CreateQuoteResponse(Guid.Empty), success: false, error: ex.Message, StatusCode: ex.StatusCode.ToString());
        }
        catch (Exception)
        {
            return new BaseResponse<CreateQuoteResponse>(data: new CreateQuoteResponse(Guid.Empty), success: false);
        }
    }
}
