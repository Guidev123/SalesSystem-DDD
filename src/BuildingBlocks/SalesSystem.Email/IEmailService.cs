using SalesSystem.Email.Models;

namespace SalesSystem.Email;

public interface IEmailService
{
    Task SendAsync(EmailMessage email);
}