using TC1.RepairShop.Domain.CustomExceptions.BusinessException;
using TC1.RepairShop.Domain.Entities.Common;

namespace TC1.RepairShop.Domain.Entities.Services;

public class Service
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public virtual List<ServicePart> Parts { get; private set; } = [];
    public Status Status { get; private set; }

    private bool IsActive() => Status == Status.Active;

    private Service()
    {
    }

    public static Service Create(string name, string description)
    {
        return new Service
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Status = Status.Active,
        };
    }

    public void AddPart(Guid partId, int quantity)
    {
        Parts.Add(ServicePart.Create(Id, partId, quantity));
    }

    public void Deactivate()
    {
        if (!IsActive())
            throw new BusinessException("Não é possível inativar um serviço que não esteja ativo");

        Status = Status.Inactive;
    }

    public void Delete()
    {
        Status = Status.Deleted;
    }
}
