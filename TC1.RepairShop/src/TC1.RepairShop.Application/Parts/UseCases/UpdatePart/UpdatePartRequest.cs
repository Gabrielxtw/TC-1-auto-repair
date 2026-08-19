namespace TC1.RepairShop.Application.Parts.UseCases;

public record UpdatePartRequest(
    Guid Id,
    string Name,
    decimal Price
);
