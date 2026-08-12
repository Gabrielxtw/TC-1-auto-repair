namespace TC1.RepairShop.Application.Parts.UseCases.GetAllPart
{
    public record GetAllPartViewModel(Guid id, string name, int stockQuantity, decimal UnitPrice);
}
