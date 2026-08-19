namespace TC1.RepairShop.Application.Notifications;

public record EmailMessage(string To, string Subject, string HtmlBody);
