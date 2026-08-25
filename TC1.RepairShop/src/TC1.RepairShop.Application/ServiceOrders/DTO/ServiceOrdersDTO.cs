using System.Runtime.CompilerServices;
using TC1.RepairShop.Application.Parts.UseCases;
using TC1.RepairShop.Application.Quotes.UseCases;
using TC1.RepairShop.Application.Services.UseCases;
using TC1.RepairShop.Application.Users.UseCases;
using TC1.RepairShop.Application.Vehicles.UseCases;
using TC1.RepairShop.Domain.Entities.ServiceOrders;

namespace TC1.RepairShop.Application.ServiceOrders.UseCases
{
    public record ServiceOrderListResponse(Guid Id, string? CustomerName, string Status, DateTime OpenedAt, string? CustomerEmail);
    public record ListServiceOrdersResponse(IEnumerable<ServiceOrderListResponse> Orders);
    public record ServiceOrderPartResponse(Guid Id, Guid PartId, PartResponse Part, decimal price, int Quantity, bool SuppliedByCustomer);
    public record ServiceOrderServiceResponse(Guid Id, Guid ServiceId, ServiceResponse Service, decimal Price);

    public static class ServiceOrdersDTO
    {
        public static ServiceOrderListResponse ToListResponse(ServiceOrder order)
        {
            return new ServiceOrderListResponse(order.Id, order.User?.Username, order.OrderStatusValue.ToString(), order.OpenedAt, order.User?.Email.Value);
        }

        public static ListServiceOrdersResponse ToListServiceOrdersResponse(IEnumerable<ServiceOrder> orders)
        {
            var responses = orders.Select(o => ToListResponse(o)).ToList();
            return new ListServiceOrdersResponse(responses);
        }
    }
    public record GetServiceOrderByIdResponse(
    Guid Id,
    UserResponse User,
    VehicleResponse Vehicle,
    string OrderStatus,
    DateTime OpenedAt,
    DateTime? CompletedAt,
    QuoteResponse? Quote,
    IEnumerable<ServiceOrderServiceResponse> Services,
    IEnumerable<ServiceOrderPartResponse> Parts
)
    {
        public static GetServiceOrderByIdResponse FromDomain(ServiceOrder order)
        {
            var user = order.User;
            var userResp = new UserResponse(
                user.Id,
                user.Username,
                user.Role.ToString(),
                user.Status.ToString()
            );

            var vehicle = order.Vehicle;
            var vehicleResp = new VehicleResponse(
                vehicle.Id,
                user.Username,
                vehicle.LicensePlate.ToString(),
                vehicle.Brand,
                vehicle.Model,
                vehicle.Year,
                vehicle.Status.ToString()
            );

            var quote = order.Quote;
            QuoteResponse? quoteResp = quote is null ? null : new QuoteResponse(
                quote.Id,
                order.Id,
                quote.Price,
                quote.QuoteStatusValue.ToString(),
                quote.RejectionCount
            );

            var services = order.ServiceOrderServices
                .Select(sp => new ServiceOrderServiceResponse(
                    sp.Id,
                    sp.ServiceId,
                    new ServiceResponse(sp.Service.Id, sp.Service.Name, sp.Service.Description,sp.Price,sp.Service.Status),
                    sp.Price
                ))
                .ToList();

            var parts = order.ServiceOrderParts
                .Select(sp => new ServiceOrderPartResponse(
                    sp.Id,
                    sp.PartId,
                    new PartResponse(sp.Part.Id, sp.Part.Name, sp.Part.StockQuantity,sp.Part.Price,sp.Part.Status),
                    sp.Price,
                    sp.Quantity,
                    sp.SuppliedByCustomer
                ))
                .ToList();

            return new GetServiceOrderByIdResponse(
                order.Id,
                userResp,
                vehicleResp,
                order.OrderStatusValue.ToString(),
                order.OpenedAt,
                order.CompletedAt,
                quoteResp,
                services,
                parts
            );
        }
    }
}
