using Microsoft.Extensions.Options;
using SalesSystem.Email.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace SalesSystem.Email;

public sealed class EmailService(ISendGridClient sendGridClient, IOptions<EmailSettings> emailSettings)
                  : IEmailService
{
    private readonly ISendGridClient _sendGridClient = sendGridClient;
    private readonly EmailSettings _emailSettings = emailSettings.Value;

    public async Task SendAsync(EmailMessage email)
    {
        var sendGridMessage = new SendGridMessage
        {
            From = new EmailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
            Subject = email.Subject
        };

        sendGridMessage.AddContent(MimeType.Text, email.Content);
        sendGridMessage.AddTo(new EmailAddress(email.To));
        await _sendGridClient.SendEmailAsync(sendGridMessage);
    }
}