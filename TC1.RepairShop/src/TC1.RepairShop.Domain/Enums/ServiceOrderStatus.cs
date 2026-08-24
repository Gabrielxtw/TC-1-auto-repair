namespace TC1.RepairShop.Domain.Enums;

public abstract class ServiceOrderStatus : Enumeration<ServiceOrderStatus>
{
    public static readonly ServiceOrderStatus Received = new ReceivedStatus();
    public static readonly ServiceOrderStatus UnderDiagnosis = new UnderDiagnosisStatus();
    public static readonly ServiceOrderStatus AwaitingApproval = new AwaitingApprovalStatus();
    public static readonly ServiceOrderStatus InProgress = new InProgressStatus();
    public static readonly ServiceOrderStatus Completed = new CompletedStatus();
    public static readonly ServiceOrderStatus Delivered = new DeliveredStatus();
    public static readonly ServiceOrderStatus Cancelled = new CancelledStatus();

    private ServiceOrderStatus(int id, string name) : base(id, name) { }

    public abstract bool CanTransitionTo(ServiceOrderStatus next);

    private sealed class ReceivedStatus : ServiceOrderStatus
    {
        public ReceivedStatus() : base(1, "Received") { }

        public override bool CanTransitionTo(ServiceOrderStatus next) =>
            next == UnderDiagnosis || next == Cancelled;
    }

    private sealed class UnderDiagnosisStatus : ServiceOrderStatus
    {
        public UnderDiagnosisStatus() : base(2, "Under Diagnosis") { }

        public override bool CanTransitionTo(ServiceOrderStatus next) =>
            next == AwaitingApproval || next == Cancelled;
    }

    private sealed class AwaitingApprovalStatus : ServiceOrderStatus
    {
        public AwaitingApprovalStatus() : base(3, "Awaiting Approval") { }

        public override bool CanTransitionTo(ServiceOrderStatus next) =>
            next == InProgress || next == UnderDiagnosis;
    }

    private sealed class InProgressStatus : ServiceOrderStatus
    {
        public InProgressStatus() : base(4, "In Progress") { }

        public override bool CanTransitionTo(ServiceOrderStatus next) =>
            next == Completed;
    }

    private sealed class CompletedStatus : ServiceOrderStatus
    {
        public CompletedStatus() : base(5, "Completed") { }

        public override bool CanTransitionTo(ServiceOrderStatus next) =>
            next == Delivered;
    }

    private sealed class DeliveredStatus : ServiceOrderStatus
    {
        public DeliveredStatus() : base(6, "Delivered") { }

        public override bool CanTransitionTo(ServiceOrderStatus next) => false;
    }

    private sealed class CancelledStatus : ServiceOrderStatus
    {
        public CancelledStatus() : base(7, "Cancelled") { }

        public override bool CanTransitionTo(ServiceOrderStatus next) => false;
    }
}
