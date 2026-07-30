namespace TC1.RepairShop.Domain.Services;

public class ServicePart
{
    public Guid Id { get; private set; }
    public Guid ServiceId { get; private set; }
    public Guid PartId { get; private set; }
    public int Quantity { get; private set; }

    private ServicePart()
    {
    }

    public static ServicePart Create(Guid serviceId, Guid partId, int quantity)
    {
        return new ServicePart
        {
            Id = Guid.NewGuid(),
            ServiceId = serviceId,
            PartId = partId,
            Quantity = quantity,
        };
    }
}
