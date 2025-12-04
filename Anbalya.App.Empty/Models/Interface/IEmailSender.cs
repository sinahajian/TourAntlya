using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models.Services;

public interface IEmailSender
{
    Task SendEmailAsync(string toEmail, string subject, string body, string? toName = null, bool isBodyHtml = false, IEnumerable<EmailAttachment>? attachments = null, CancellationToken ct = default);
    Task SendEmailAsync(IEnumerable<string> toEmails, string subject, string body, bool isBodyHtml = false, IEnumerable<EmailAttachment>? attachments = null, CancellationToken ct = default);
}
