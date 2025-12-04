using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.DbContexts;
using Models.Entities;
using Models.Helper;

namespace Models.Repository
{
    public class EmailConfigurationRepository : IEmailConfigurationRepository
    {
        private const string DefaultSmtpHost = "smtp.gmail.com";
        private const int DefaultSmtpPort = 587;
        private const string DefaultSmtpUsername = "spbesmtp@gmail.com";
        private const string DefaultSmtpPassword = "ygwlpdforvafxtnb";
        private const string DefaultFromEmail = "spbesmtp@gmail.com";
        private const string DefaultFromName = "Tour Antalya";
        private const string DefaultNotificationEmail = "spbesmtp@gmail.com";
        private const string DefaultReplyTo = "support@tourantalya.com";

        private static readonly string[] LegacyHosts =
        {
            "",
            "smtp.example.com",
            "smtp.mailtrap.io"
        };

        private static readonly string[] LegacyFromEmails =
        {
            "",
            "no-reply@tourantalya.com",
            "noreply@example.com"
        };

        private static readonly string[] LegacyFromNames =
        {
            "",
            "Tour Antalya",
            "Tour Manager"
        };

        private static readonly string[] LegacyNotificationEmails =
        {
            "",
            "alerts@tourantalya.com",
            "info@example.com"
        };

        private readonly TourDbContext _context;

        public EmailConfigurationRepository(TourDbContext context)
        {
            _context = context;
        }

        public async Task<SmtpSettings> GetSmtpSettingsAsync(CancellationToken ct = default)
        {
            var settings = await _context.SmtpSettings.FirstOrDefaultAsync(ct);
            if (settings is null)
            {
                settings = CreateDefaultSmtpSettings();
                settings.CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                _context.SmtpSettings.Add(settings);
                await _context.SaveChangesAsync(ct);
                return settings;
            }

            if (ApplySmtpDefaults(settings))
            {
                await _context.SaveChangesAsync(ct);
            }

            return settings;
        }

        public async Task UpdateSmtpSettingsAsync(SmtpSettings settings, CancellationToken ct = default)
        {
            var existing = await _context.SmtpSettings.FirstOrDefaultAsync(ct);
            if (existing is null)
            {
                settings.CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                _context.SmtpSettings.Add(settings);
            }
            else
            {
                existing.Host = settings.Host?.Trim() ?? string.Empty;
                existing.Port = settings.Port;
                existing.EnableSsl = settings.EnableSsl;
                existing.Username = settings.Username?.Trim() ?? string.Empty;
                existing.Password = settings.Password ?? string.Empty;
                existing.FromEmail = settings.FromEmail?.Trim() ?? string.Empty;
                existing.FromName = settings.FromName?.Trim() ?? string.Empty;
                existing.NotificationEmail = settings.NotificationEmail?.Trim() ?? string.Empty;
                existing.ReplyToEmail = string.IsNullOrWhiteSpace(settings.ReplyToEmail) ? null : settings.ReplyToEmail.Trim();
            }

            await _context.SaveChangesAsync(ct);
        }

        public async Task<IReadOnlyDictionary<string, EmailTemplate>> GetTemplatesAsync(CancellationToken ct = default)
        {
            var templates = await _context.EmailTemplates
                .ToDictionaryAsync(t => t.TemplateKey, ct);

            var changed = false;

            void EnsureTemplate(string key, string subject, string body)
            {
                if (templates.TryGetValue(key, out var existing))
                {
                    if (string.IsNullOrWhiteSpace(existing.Subject) || string.IsNullOrWhiteSpace(existing.Body))
                    {
                        existing.Subject = subject;
                        existing.Body = body;
                        changed = true;
                    }
                }
                else
                {
                    var template = new EmailTemplate
                    {
                        TemplateKey = key,
                        Subject = subject,
                        Body = body,
                        CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                    };
                    _context.EmailTemplates.Add(template);
                    templates[key] = template;
                    changed = true;
                }
            }

            EnsureTemplate(
                EmailTemplateKeys.ContactUser,
                "We received your message – Tour Antalya",
                "<p>Hi {FullName},</p><p>Thank you for contacting <strong>Tour Antalya</strong>. Our support team will respond within 24 hours.</p><p style=\"margin:16px 0; padding:12px 16px; background:#f5f7fb; border-radius:8px;\">Your message:<br/><em>{Message}</em></p><p style=\"margin:0;\">Warm regards,<br/>Tour Antalya Concierge</p>");

            EnsureTemplate(
                EmailTemplateKeys.ContactAdmin,
                "New contact message from {FullName}",
                "<p><strong>New contact request received.</strong></p><table style=\"width:100%;border-collapse:collapse;\">"
                + "<tr><th align=\"left\" style=\"padding:6px 8px;border-bottom:1px solid #dbe3f0;\">Guest</th><td style=\"padding:6px 8px;border-bottom:1px solid #dbe3f0;\">{FullName}</td></tr>"
                + "<tr><th align=\"left\" style=\"padding:6px 8px;border-bottom:1px solid #dbe3f0;\">Email</th><td style=\"padding:6px 8px;border-bottom:1px solid #dbe3f0;\">{Email}</td></tr>"
                + "<tr><th align=\"left\" style=\"padding:6px 8px;border-bottom:1px solid #dbe3f0;\">Language</th><td style=\"padding:6px 8px;border-bottom:1px solid #dbe3f0;\">{Language}</td></tr>"
                + "<tr><th align=\"left\" style=\"padding:6px 8px;\">Received</th><td style=\"padding:6px 8px;\">{CreatedAt}</td></tr></table>"
                + "<p style=\"margin:12px 0 0;\">Message:</p><blockquote style=\"margin:0; padding:12px 16px; background:#f1f5fb; border-left:3px solid #FF6B2C;\">{Message}</blockquote>");

            EnsureTemplate(
                EmailTemplateKeys.ReservationUser,
                "Reservation confirmed – {TourName}",
                "<p>Dear {FullName},</p><p>Your reservation for <strong>{TourName}</strong> is confirmed.</p>"
                + "<ul style=\"padding-left:18px;margin:12px 0;\">"
                + "<li>Reservation ID: #{ReservationId}</li>"
                + "<li>Preferred date: {PreferredDate}</li>"
                + "<li>Guests: Adults {Adults}, Children {Children}, Infants {Infants}</li>"
                + "<li>Payment method: {PaymentMethod}</li>"
                + "</ul>"
                + "<p>We will share pickup instructions shortly. For changes, reply to this email or call {CompanyPhone}.</p>"
                + "<p style=\"margin:0;\">Warm regards,<br/>Tour Antalya Concierge</p>");

            EnsureTemplate(
                EmailTemplateKeys.ReservationAdmin,
                "New reservation – {TourName}",
                "<p><strong>A new reservation has been placed.</strong></p>"
                + "<table style=\"width:100%;border-collapse:collapse;\">"
                + "<tr><th align=\"left\" style=\"padding:6px 8px;border-bottom:1px solid #dbe3f0;\">Tour</th><td style=\"padding:6px 8px;border-bottom:1px solid #dbe3f0;\">{TourName}</td></tr>"
                + "<tr><th align=\"left\" style=\"padding:6px 8px;border-bottom:1px solid #dbe3f0;\">Guest</th><td style=\"padding:6px 8px;border-bottom:1px solid #dbe3f0;\">{FullName} ({Email})</td></tr>"
                + "<tr><th align=\"left\" style=\"padding:6px 8px;border-bottom:1px solid #dbe3f0;\">Preferred date</th><td style=\"padding:6px 8px;border-bottom:1px solid #dbe3f0;\">{PreferredDate}</td></tr>"
                + "<tr><th align=\"left\" style=\"padding:6px 8px;border-bottom:1px solid #dbe3f0;\">Guests</th><td style=\"padding:6px 8px;border-bottom:1px solid #dbe3f0;\">Adults {Adults} · Children {Children} · Infants {Infants}</td></tr>"
                + "<tr><th align=\"left\" style=\"padding:6px 8px;\">Payment reference</th><td style=\"padding:6px 8px;\">{PaymentReference}</td></tr></table>"
                + "<p style=\"margin:12px 0 0;\">Notes: {Notes}</p>");

            if (changed)
            {
                await _context.SaveChangesAsync(ct);
            }

            return new ReadOnlyDictionary<string, EmailTemplate>(templates);
        }

        public async Task UpdateTemplatesAsync(IEnumerable<EmailTemplate> templates, CancellationToken ct = default)
        {
            var templateList = templates.ToList();
            if (templateList.Count == 0) return;

            var keys = templateList.Select(t => t.TemplateKey).ToList();
            var existing = await _context.EmailTemplates
                .Where(t => keys.Contains(t.TemplateKey))
                .ToDictionaryAsync(t => t.TemplateKey, ct);

            foreach (var template in templateList)
            {
                if (existing.TryGetValue(template.TemplateKey, out var current))
                {
                    current.Subject = template.Subject?.Trim() ?? string.Empty;
                    current.Body = template.Body?.Trim() ?? string.Empty;
                }
                else
                {
                    template.CreationTime = template.CreationTime == 0
                        ? DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                        : template.CreationTime;
                    _context.EmailTemplates.Add(template);
                }
            }

            await _context.SaveChangesAsync(ct);
        }

        public async Task<EmailTemplate?> GetTemplateAsync(string templateKey, string? language = null, CancellationToken ct = default)
        {
            return await _context.EmailTemplates
                .AsNoTracking()
                .FirstOrDefaultAsync(t =>
                    t.TemplateKey == templateKey &&
                    (language == null || t.Language == language), ct);
        }

        private static SmtpSettings CreateDefaultSmtpSettings()
        {
            return new SmtpSettings
            {
                Id = 1,
                Host = DefaultSmtpHost,
                Port = DefaultSmtpPort,
                EnableSsl = true,
                Username = DefaultSmtpUsername,
                Password = DefaultSmtpPassword,
                FromEmail = DefaultFromEmail,
                FromName = DefaultFromName,
                NotificationEmail = DefaultNotificationEmail,
                ReplyToEmail = DefaultReplyTo
            };
        }

        private static bool ApplySmtpDefaults(SmtpSettings settings)
        {
            var updated = false;

            if (IsLegacyValue(settings.Host, LegacyHosts))
            {
                settings.Host = DefaultSmtpHost;
                updated = true;
            }

            if (settings.Port == 0)
            {
                settings.Port = DefaultSmtpPort;
                updated = true;
            }

            if (IsLegacyValue(settings.Username))
            {
                settings.Username = DefaultSmtpUsername;
                updated = true;
            }

            if (IsLegacyValue(settings.Password))
            {
                settings.Password = DefaultSmtpPassword;
                updated = true;
            }

            if (IsLegacyValue(settings.FromEmail, LegacyFromEmails))
            {
                settings.FromEmail = DefaultFromEmail;
                updated = true;
            }

            if (IsLegacyValue(settings.FromName, LegacyFromNames))
            {
                settings.FromName = DefaultFromName;
                updated = true;
            }

            if (IsLegacyValue(settings.NotificationEmail, LegacyNotificationEmails))
            {
                settings.NotificationEmail = DefaultNotificationEmail;
                updated = true;
            }

            if (settings.EnableSsl == false)
            {
                settings.EnableSsl = true;
                updated = true;
            }

            if (IsLegacyValue(settings.ReplyToEmail))
            {
                settings.ReplyToEmail = DefaultReplyTo;
                updated = true;
            }

            if (settings.CreationTime == 0)
            {
                settings.CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                updated = true;
            }

            return updated;
        }

        private static bool IsLegacyValue(string? value, params string[]? legacyValues)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return true;
            }

            if (legacyValues is null || legacyValues.Length == 0)
            {
                return false;
            }

            foreach (var legacy in legacyValues)
            {
                if (string.IsNullOrWhiteSpace(legacy)) continue;
                if (string.Equals(value.Trim(), legacy.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
