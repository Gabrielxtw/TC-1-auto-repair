namespace TC1.RepairShop.Domain.Parts;

public class Part
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int StockQuantity { get; set; }
    public int MinimumQuantity { get; set; }
}
