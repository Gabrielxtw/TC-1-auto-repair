namespace TC1.RepairShop.Domain.ServiceOrders;

public class ServiceOrder
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid VehicleId { get; set; }
    public ServiceOrderStatus Status { get; set; }
    public DateTime OpenedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Guid? QuoteId { get; set; }
}
