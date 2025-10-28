using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

        public Task SendEmailAsync(string toEmail, string subject, string body, string? toName = null, CancellationToken ct = default)
        {
            return SendEmailAsync(new[] { toEmail }, subject, body, ct, toName);
        }

        public async Task SendEmailAsync(IEnumerable<string> toEmails, string subject, string body, CancellationToken ct = default)
        {
            await SendEmailAsync(toEmails, subject, body, ct, null);
        }

        private async Task SendEmailAsync(IEnumerable<string> toEmails, string subject, string body, CancellationToken ct, string? singleName)
        {
            var recipients = toEmails.Where(e => !string.IsNullOrWhiteSpace(e)).Select(e => e.Trim()).Distinct().ToList();
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
                Body = body,
                IsBodyHtml = false
            };

            if (!string.IsNullOrWhiteSpace(smtp.ReplyToEmail))
            {
                message.ReplyToList.Add(new MailAddress(smtp.ReplyToEmail));
            }

            foreach (var recipient in recipients)
            {
                message.To.Add(new MailAddress(recipient, singleName));
            }

            try
            {
                await client.SendMailAsync(message);
                _logger.LogInformation("Email sender: message sent to {Recipients}", string.Join(", ", recipients));
            }
            catch (SmtpException ex)
            {
                _logger.LogError(ex, "Email sender: failed to send email.");
            }
        }
    }
}
