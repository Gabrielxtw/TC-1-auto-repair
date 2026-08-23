using System.Threading.Channels;
using TC1.RepairShop.Application.Notifications;

namespace TC1.RepairShop.Infrastructure.Email;

public class EmailQueue : IEmailSender
{
    private readonly Channel<EmailMessage> _channel = Channel.CreateUnbounded<EmailMessage>();

    public ChannelReader<EmailMessage> Reader => _channel.Reader;

    public void Enqueue(EmailMessage message)
    {
        _channel.Writer.TryWrite(message);
    }
}
