using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Enums;

namespace TC1.RepairShop.Application.Parts.UseCases
{
    public record PartResponse(Guid Id, string Name, int StockQuantity, decimal Price, Status Status);
    public record CreatePartRequest(string Name, decimal Price, int StockQuantity);
    public record ListPartsResponse(ICollection<PartResponse> Parts);
    public record ReceiveStockRequest(Guid Id, int Quantity);
    public record UpdatePartRequest(Guid Id,string Name,decimal Price);
    public record DeactivePartRequest(Guid Id);
    public record ConsumeStockRequest(Guid Id, int Quantity);
    public class PartDTO
    {

        public static PartResponse ToPartResponse(Part part)
        {
            return new PartResponse(part.Id, part.Name, part.StockQuantity, part.Price, part.Status);
        }
        public static ListPartsResponse ToListPartsResponse(IEnumerable<Part> parts)
        {
            var partResponses = parts.Select(part => new PartResponse(part.Id, part.Name, part.StockQuantity, part.Price, part.Status)).ToList();
            return new ListPartsResponse(partResponses);
        }
    }
}
