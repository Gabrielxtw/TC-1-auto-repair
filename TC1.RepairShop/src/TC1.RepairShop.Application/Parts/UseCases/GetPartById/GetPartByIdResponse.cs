using TC1.RepairShop.Domain.Enums;

namespace TC1.RepairShop.Application.Parts.UseCases
{
    public class GetPartByIdResponse(Guid id, string name, decimal price, int stockQuantity, Status status);
}
