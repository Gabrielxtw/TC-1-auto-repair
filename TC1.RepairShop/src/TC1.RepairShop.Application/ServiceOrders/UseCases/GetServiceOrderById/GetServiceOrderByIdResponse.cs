using System;
using System.Collections.Generic;
using System.Linq;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Entities.Vehicles;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Entities.Quotes;

namespace TC1.RepairShop.Application.ServiceOrders.UseCases;

// Response types for GetServiceOrderById use case.
// These records include navigation entities but intentionally omit back-references
// that would create cycles when serialized to JSON.
public record GetServiceOrderByIdResponse(
    Guid Id,
    UserResponse User,
    VehicleResponse Vehicle,
    string OrderStatus,
    DateTime OpenedAt,
    DateTime? CompletedAt,
    QuoteResponse? Quote,
    IEnumerable<ServiceResponse> Services,
    IEnumerable<ServiceOrderPartResponse> Parts
)
{
    public static GetServiceOrderByIdResponse FromDomain(ServiceOrder order)
    {
        var user = order.User;
        var userResp = new UserResponse(
            user.Id,
            user.Username,
            user.Email.Value,
            user.Phone,
            user.Role.ToString()
        );

        var vehicle = order.Vehicle;
        var vehicleResp = new VehicleResponse(
            vehicle.Id,
            vehicle.LicensePlate.ToString(),
            vehicle.Brand,
            vehicle.Model,
            vehicle.Year
        );

        var quote = order.Quote;
        QuoteResponse? quoteResp = quote is null ? null : new QuoteResponse(
            quote.Id,
            quote.Price,
            quote.QuoteStatusValue.ToString(),
            quote.RejectionCount
        );

        var services = order.Services
            .Select(s => new ServiceResponse(s.Id, s.Name, s.Description, s.Price))
            .ToList();

        var parts = order.ServiceOrderParts
            .Select(sp => new ServiceOrderPartResponse(
                sp.Id,
                sp.PartId,
                new PartResponse(sp.Part.Id, sp.Part.Name, sp.Part.Price, sp.Part.StockQuantity),
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

public record UserResponse(Guid Id, string Username, string Email, string Phone, string Role);

public record VehicleResponse(Guid Id, string LicensePlate, string Brand, string Model, int Year);

public record QuoteResponse(Guid Id, decimal Price, string QuoteStatus, int RejectionCount);

public record ServiceResponse(Guid Id, string Name, string Description, decimal Price);

public record PartResponse(Guid Id, string Name, decimal Price, int StockQuantity);

public record ServiceOrderPartResponse(Guid Id, Guid PartId, PartResponse Part, int Quantity, bool SuppliedByCustomer);
