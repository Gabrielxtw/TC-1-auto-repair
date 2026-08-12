namespace TC1.RepairShop.Application.Parts.UseCases.RegisterPart
{
    public record RegisterPartRequest(
        string Name,
        decimal UnitPrice,
        int MinimumQuantity
     );
}
