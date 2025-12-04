using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Models.Entities;

namespace Models.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IEmailConfigurationRepository _configRepository;
        private readonly ILogger<SmtpEmailSender> _logger;

        public SmtpEmailSender(IEmailConfigurationRepository configRepository, ILogger<SmtpEmailSender> logger)
        {
            _configRepository = configRepository;
            _logger = logger;
        }

        public Task SendEmailAsync(
            string toEmail,
            string subject,
            string body,
            string? toName = null,
            bool isBodyHtml = false,
            IEnumerable<EmailAttachment>? attachments = null,
            CancellationToken ct = default)
        {
            var recipient = string.IsNullOrWhiteSpace(toEmail)
                ? Array.Empty<(string Email, string? Name)>()
                : new[] { (toEmail.Trim(), toName) };

            return SendInternalAsync(recipient, subject, body, isBodyHtml, attachments, ct);
        }

        public Task SendEmailAsync(
            IEnumerable<string> toEmails,
            string subject,
            string body,
            bool isBodyHtml = false,
            IEnumerable<EmailAttachment>? attachments = null,
            CancellationToken ct = default)
        {
            var recipients = toEmails?
                .Where(address => !string.IsNullOrWhiteSpace(address))
                .Select(address => (address.Trim(), (string?)null))
                .ToArray() ?? Array.Empty<(string, string?)>();

            return SendInternalAsync(recipients, subject, body, isBodyHtml, attachments, ct);
        }

        private async Task SendInternalAsync(
            IReadOnlyCollection<(string Email, string? Name)> recipients,
            string subject,
            string body,
            bool isBodyHtml,
            IEnumerable<EmailAttachment>? attachments,
            CancellationToken ct)
        {
            if (recipients.Count == 0)
            {
                _logger.LogWarning("Email sender: no recipients supplied.");
                return;
            }

            var smtp = await _configRepository.GetSmtpSettingsAsync(ct);
            if (string.IsNullOrWhiteSpace(smtp.Host) || string.IsNullOrWhiteSpace(smtp.FromEmail))
            {
                _logger.LogWarning("Email sender: SMTP host or from-email not configured. Skipping send.");
                return;
            }

            using var client = new SmtpClient(smtp.Host, smtp.Port)
            {
                EnableSsl = smtp.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            if (!string.IsNullOrWhiteSpace(smtp.Username))
            {
                client.Credentials = new NetworkCredential(smtp.Username, smtp.Password);
            }

            using var message = new MailMessage
            {
                From = new MailAddress(smtp.FromEmail, smtp.FromName),
                Subject = subject,
                BodyEncoding = Encoding.UTF8,
                SubjectEncoding = Encoding.UTF8,
                Body = body ?? string.Empty,
                IsBodyHtml = isBodyHtml
            };

            if (!string.IsNullOrWhiteSpace(smtp.ReplyToEmail))
            {
                message.ReplyToList.Add(new MailAddress(smtp.ReplyToEmail));
            }

            foreach (var (email, name) in recipients)
            {
                message.To.Add(new MailAddress(email, name));
            }

            if (isBodyHtml)
            {
                var plainText = HtmlToPlainText(body);
                if (!string.IsNullOrWhiteSpace(plainText))
                {
                    var alternateView = AlternateView.CreateAlternateViewFromString(
                        plainText,
                        Encoding.UTF8,
                        MediaTypeNames.Text.Plain);
                    message.AlternateViews.Add(alternateView);
                }
            }

            if (attachments is not null)
            {
                foreach (var attachment in attachments)
                {
                    var contentStream = new MemoryStream(attachment.Content, writable: false);
                    var mailAttachment = new Attachment(contentStream, attachment.FileName, attachment.ContentType);
                    message.Attachments.Add(mailAttachment);
                }
            }

            try
            {
                await client.SendMailAsync(message);
                _logger.LogInformation("Email sender: message \"{Subject}\" sent to {Recipients}", subject, string.Join(", ", recipients.Select(r => r.Email)));
            }
            catch (SmtpException ex)
            {
                _logger.LogError(ex, "Email sender: failed to send email.");
            }
        }

        private static string HtmlToPlainText(string? html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                return string.Empty;
            }

            var withoutTags = Regex.Replace(html, "<br\\s*/?>", "\n", RegexOptions.IgnoreCase);
            withoutTags = Regex.Replace(withoutTags, "<p.*?>", string.Empty, RegexOptions.IgnoreCase);
            withoutTags = Regex.Replace(withoutTags, "</p>", "\n\n", RegexOptions.IgnoreCase);
            withoutTags = Regex.Replace(withoutTags, "<.*?>", string.Empty);
            return WebUtility.HtmlDecode(withoutTags).Trim();
        }
    }
}
