namespace TC1.RepairShop.Application.Notifications;

public interface IEmailSender
{
    void Enqueue(EmailMessage message);
}
