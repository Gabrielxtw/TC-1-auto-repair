namespace TC1.RepairShop.Application.Parts.UseCases
{
    public record GetAllPartViewModel(Guid id, string name, int stockQuantity, decimal UnitPrice);
}
