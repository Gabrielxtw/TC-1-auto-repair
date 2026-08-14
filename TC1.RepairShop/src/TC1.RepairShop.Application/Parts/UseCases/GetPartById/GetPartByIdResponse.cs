using TC1.RepairShop.Domain.Enums;

namespace TC1.RepairShop.Application.Parts.UseCases.GetPartById
{
    public class GetPartByIdResponse(Guid id, string name, decimal price, int stockQuantity, Status status);
}
