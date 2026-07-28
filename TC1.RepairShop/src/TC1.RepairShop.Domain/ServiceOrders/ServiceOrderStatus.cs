namespace TC1.RepairShop.Domain.ServiceOrders;

public enum ServiceOrderStatus
{
    Received,
    UnderDiagnosis,
    AwaitingApproval,
    InProgress,
    Completed,
    Delivered,
}
