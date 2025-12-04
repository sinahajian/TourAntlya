using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Models.Entities;
using Models.Helper;

namespace Models.Services
{
    public class AppDataSeeder
    {
        private readonly IEmailConfigurationRepository _emailRepository;
        private readonly IInvoiceSettingsRepository _invoiceRepository;
        private readonly ILogger<AppDataSeeder> _logger;

        public AppDataSeeder(
            IEmailConfigurationRepository emailRepository,
            IInvoiceSettingsRepository invoiceRepository,
            ILogger<AppDataSeeder> logger)
        {
            _emailRepository = emailRepository;
            _invoiceRepository = invoiceRepository;
            _logger = logger;
        }

        public async Task SeedAsync(CancellationToken ct = default)
        {
            await SeedSmtpSettingsAsync(ct);
            await SeedEmailTemplatesAsync(ct);
            await SeedInvoiceSettingsAsync(ct);
        }

        private async Task SeedSmtpSettingsAsync(CancellationToken ct)
        {
            var smtp = await _emailRepository.GetSmtpSettingsAsync(ct);
            var updated = false;

            if (string.IsNullOrWhiteSpace(smtp.Host))
            {
                smtp.Host = "smtp.gmail.com";
                updated = true;
            }

            if (smtp.Port == 0)
            {
                smtp.Port = 587;
                updated = true;
            }

            if (string.IsNullOrWhiteSpace(smtp.Username))
            {
                smtp.Username = "spbesmtp@gmail.com";
                updated = true;
            }

            if (string.IsNullOrWhiteSpace(smtp.Password))
            {
                smtp.Password = "ygwlpdforvafxtnb";
                updated = true;
            }

            if (string.IsNullOrWhiteSpace(smtp.FromEmail))
            {
                smtp.FromEmail = "spbesmtp@gmail.com";
                updated = true;
            }

            if (string.IsNullOrWhiteSpace(smtp.FromName))
            {
                smtp.FromName = "Tour Antalya";
                updated = true;
            }

            if (string.IsNullOrWhiteSpace(smtp.NotificationEmail))
            {
                smtp.NotificationEmail = "spbesmtp@gmail.com";
                updated = true;
            }

            if (string.IsNullOrWhiteSpace(smtp.ReplyToEmail))
            {
                smtp.ReplyToEmail = "support@tourantalya.com";
                updated = true;
            }

            if (!smtp.EnableSsl)
            {
                smtp.EnableSsl = true;
                updated = true;
            }

            if (updated)
            {
                _logger.LogInformation("Seeding SMTP defaults.");
                await _emailRepository.UpdateSmtpSettingsAsync(smtp, ct);
            }
        }

        private async Task SeedEmailTemplatesAsync(CancellationToken ct)
        {
            var existing = await _emailRepository.GetTemplatesAsync(ct);
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var templatesToUpsert = new List<EmailTemplate>();

            void EnsureTemplate(string key, string subject, string body)
            {
                if (!existing.TryGetValue(key, out var current) ||
                    string.IsNullOrWhiteSpace(current.Subject) ||
                    string.IsNullOrWhiteSpace(current.Body))
                {
                    templatesToUpsert.Add(new EmailTemplate
                    {
                        Id = current?.Id ?? 0,
                        TemplateKey = key,
                        Subject = subject.Trim(),
                        Body = body.Trim(),
                        CreationTime = current?.CreationTime ?? now
                    });
                }
            }

            EnsureTemplate(
                EmailTemplateKeys.ContactUser,
                "We received your message – Tour Antalya",
                "<p>Hi {FullName},</p><p>Thank you for contacting <strong>Tour Antalya</strong>. Our support team will review your message and reply within 24 hours.</p><p style=\"margin:16px 0; padding:12px 16px; background:#f7f9fc; border-radius:8px;\">Your message:<br/><em>{Message}</em></p><p style=\"margin:0;\">Warm regards,<br/>Tour Antalya Team</p>");

            EnsureTemplate(
                EmailTemplateKeys.ContactAdmin,
                "New contact message from {FullName}",
                "<p><strong>New contact request received.</strong></p><ul style=\"padding-left:18px;margin:12px 0;\">"
                + "<li>Name: {FullName}</li>"
                + "<li>Email: {Email}</li>"
                + "<li>Language: {Language}</li>"
                + "<li>Submitted: {CreatedAt}</li>"
                + "</ul><p style=\"margin:0 0 12px;\">Message:</p><blockquote style=\"margin:0; padding:12px 16px; background:#f1f5fb; border-left:3px solid #FF6B2C;\">{Message}</blockquote>");

            EnsureTemplate(
                EmailTemplateKeys.ReservationUser,
                "Reservation confirmed – {TourName}",
                "<p>Dear {FullName},</p>"
                + "<p>We are excited to confirm your reservation for <strong>{TourName}</strong>. Below is a summary of your booking:</p>"
                + "<ul style=\"padding-left:18px;margin:12px 0;\">"
                + "<li>Reservation ID: #{ReservationId}</li>"
                + "<li>Preferred date: {PreferredDate}</li>"
                + "<li>Guests: Adults {Adults}, Children {Children}, Infants {Infants}</li>"
                + "<li>Payment method: {PaymentMethod}</li>"
                + "</ul>"
                + "<p style=\"margin:16px 0;\">We will contact you with pickup information soon. For any changes, reply to this email or call us at {CompanyPhone}.</p>"
                + "<p style=\"margin:0;\">See you soon!<br/>Tour Antalya Team</p>");

            EnsureTemplate(
                EmailTemplateKeys.ReservationAdmin,
                "New reservation – {TourName}",
                "<p><strong>A new reservation has been placed.</strong></p>"
                + "<table style=\"width:100%;border-collapse:collapse;margin:12px 0;\">"
                + "<tr><td style=\"padding:6px 8px;border-bottom:1px solid #e6ecf2;\">Tour</td><td style=\"padding:6px 8px;border-bottom:1px solid #e6ecf2;\">{TourName}</td></tr>"
                + "<tr><td style=\"padding:6px 8px;border-bottom:1px solid #e6ecf2;\">Guest</td><td style=\"padding:6px 8px;border-bottom:1px solid #e6ecf2;\">{FullName} ({Email}, {Phone})</td></tr>"
                + "<tr><td style=\"padding:6px 8px;border-bottom:1px solid #e6ecf2;\">Reservation ID</td><td style=\"padding:6px 8px;border-bottom:1px solid #e6ecf2;\">#{ReservationId}</td></tr>"
                + "<tr><td style=\"padding:6px 8px;border-bottom:1px solid #e6ecf2;\">Preferred date</td><td style=\"padding:6px 8px;border-bottom:1px solid #e6ecf2;\">{PreferredDate}</td></tr>"
                + "<tr><td style=\"padding:6px 8px;\">Guests</td><td style=\"padding:6px 8px;\">Adults {Adults} · Children {Children} · Infants {Infants}</td></tr>"
                + "</table>"
                + "<p style=\"margin:0;\">Notes: {Notes}</p>"
                + "<p style=\"margin:12px 0 0;\">Payment reference: {PaymentReference}</p>");

            if (templatesToUpsert.Count > 0)
            {
                _logger.LogInformation("Seeding {Count} email templates.", templatesToUpsert.Count);
                await _emailRepository.UpdateTemplatesAsync(templatesToUpsert, ct);
            }
        }

        private async Task SeedInvoiceSettingsAsync(CancellationToken ct)
        {
            var settings = await _invoiceRepository.GetAsync(ct);
            var updated = false;

            if (string.IsNullOrWhiteSpace(settings.HeaderHtml) ||
                string.IsNullOrWhiteSpace(settings.IntroHtml) ||
                string.IsNullOrWhiteSpace(settings.FooterHtml))
            {
                updated = true;
            }

            if (updated)
            {
                _logger.LogInformation("Ensuring invoice settings defaults.");
                await _invoiceRepository.UpdateAsync(settings, ct);
            }
        }
    }
}
