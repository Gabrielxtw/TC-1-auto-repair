using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace TC1.RepairShop.Infrastructure.Email;

public class EmailQueueBackgroundService : BackgroundService
{
    private readonly EmailQueue _queue;
    private readonly SendGridOptions _options;
    private readonly ILogger<EmailQueueBackgroundService> _logger;

    public EmailQueueBackgroundService(
        EmailQueue queue,
        IOptions<SendGridOptions> options,
        ILogger<EmailQueueBackgroundService> logger)
    {
        _queue = queue;
        _options = options.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var client = new SendGridClient(_options.ApiKey);

        await foreach (var message in _queue.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                var from = new EmailAddress(_options.FromEmail, _options.FromName);
                var to = new EmailAddress(message.To);
                var sendGridMessage = MailHelper.CreateSingleEmail(from, to, message.Subject, plainTextContent: null, message.HtmlBody);

                var response = await client.SendEmailAsync(sendGridMessage, stoppingToken);

                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Body.ReadAsStringAsync(stoppingToken);
                    _logger.LogError(
                        "Falha ao enviar email para {To}. Status: {StatusCode}. Body: {Body}",
                        message.To, response.StatusCode, body);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar email para {To}", message.To);
            }
        }
    }
}
