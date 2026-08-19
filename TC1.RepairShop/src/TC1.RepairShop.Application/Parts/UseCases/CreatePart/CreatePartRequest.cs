namespace TC1.RepairShop.Application.Parts.UseCases
{
    public record CreatePartRequest(
        string Name,
        decimal UnitPrice,
        int MinimumQuantity
     );
}
