using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public interface IEmailSender
{
    Task SendEmailAsync(string toEmail, string subject, string body, string? toName = null, CancellationToken ct = default);
    Task SendEmailAsync(IEnumerable<string> toEmails, string subject, string body, CancellationToken ct = default);
}
