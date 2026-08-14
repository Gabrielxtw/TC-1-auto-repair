using TC1.RepairShop.Domain.CustomExceptions.BusinessException;
using TC1.RepairShop.Domain.Entities.Common;

namespace TC1.RepairShop.Domain.Entities.Parts;

public class Part
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal UnitPrice { get; private set; }
    public int StockQuantity { get; private set; }
    public int MinimumQuantity { get; private set; }
    public Status Status { get; private set; }

    private Part()
    {
    }

    public static Part Create(string name, decimal unitPrice, int minimumQuantity)
    {
        return new Part
        {
            Id = Guid.NewGuid(),
            Name = name,
            UnitPrice = unitPrice,
            StockQuantity = 0,
            MinimumQuantity = minimumQuantity,
            Status = Status.Active,
        };
    }

    private bool IsActive() => Status == Status.Active;

    public void ReceiveStock(int quantity)
    {
        if (!IsActive())
            throw new BusinessException("Não é possível alterar o estoque de um peça que não esteja ativa");

        StockQuantity += quantity;
    }

    public void ConsumeStock(int quantity)
    {
        if (!IsActive())
            throw new BusinessException("Não é possível alterar o estoque de um peça que não esteja ativa");

        StockQuantity -= quantity;
    }

    public void Deactivate()
    {
        if (!IsActive())
            throw new BusinessException("Não é possível inativar uma peça que não esteja ativa");

        Status = Status.Inactive;
    }

    public void Delete()
    {
        Status = Status.Deleted;
    }
}
