namespace TC1.RepairShop.Domain.Entities.ServiceOrders;

public enum ServiceOrderStatus
{
    Received,
    UnderDiagnosis,
    AwaitingApproval,
    InProgress,
    Completed,
    Delivered,
}
