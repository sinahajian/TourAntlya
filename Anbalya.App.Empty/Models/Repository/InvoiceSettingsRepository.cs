using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.DbContexts;
using Models.Entities;

namespace Models.Repository
{
    public class InvoiceSettingsRepository : IInvoiceSettingsRepository
    {
        private readonly TourDbContext _context;

        public InvoiceSettingsRepository(TourDbContext context)
        {
            _context = context;
        }

        public async Task<InvoiceSettings> GetAsync(CancellationToken ct = default)
        {
            var settings = await _context.InvoiceSettings.FirstOrDefaultAsync(ct);
            if (settings is null)
            {
                settings = CreateDefaultSettings();
                settings.CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                _context.InvoiceSettings.Add(settings);
                await _context.SaveChangesAsync(ct);
                return settings;
            }

            if (ApplyDefaults(settings))
            {
                await _context.SaveChangesAsync(ct);
            }

            return settings;
        }

        public async Task UpdateAsync(InvoiceSettings settings, CancellationToken ct = default)
        {
            var existing = await _context.InvoiceSettings.FirstOrDefaultAsync(ct);
            if (existing is null)
            {
                settings.CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                _context.InvoiceSettings.Add(settings);
            }
            else
            {
                existing.CompanyName = settings.CompanyName?.Trim() ?? string.Empty;
                existing.CompanyEmail = settings.CompanyEmail?.Trim() ?? string.Empty;
                existing.CompanyPhone = settings.CompanyPhone?.Trim() ?? string.Empty;
                existing.CompanyAddress = settings.CompanyAddress?.Trim() ?? string.Empty;
                existing.LogoPath = settings.LogoPath?.Trim() ?? string.Empty;
                existing.AccentColor = string.IsNullOrWhiteSpace(settings.AccentColor)
                    ? "#FF6B2C"
                    : settings.AccentColor.Trim();
                existing.HeaderHtml = settings.HeaderHtml ?? string.Empty;
                existing.IntroHtml = settings.IntroHtml ?? string.Empty;
                existing.FooterHtml = settings.FooterHtml ?? string.Empty;
                existing.TermsHtml = settings.TermsHtml ?? string.Empty;
            }

            await _context.SaveChangesAsync(ct);
        }

        private static InvoiceSettings CreateDefaultSettings()
        {
            return new InvoiceSettings
            {
                CompanyName = "Tour Antalya",
                CompanyEmail = "support@tourantalya.com",
                CompanyPhone = "+90 242 000 00 00",
                CompanyAddress = "Antalya, Turkey",
                LogoPath = "/image/logo.png",
                AccentColor = "#FF6B2C",
                HeaderHtml = "<p style=\"margin:0;font-weight:600;font-size:20px;\">Payment Receipt</p><p style=\"margin:4px 0 0;\">Thank you for choosing Tour Antalya.</p>",
                IntroHtml = "<p>Dear {FullName},</p><p>We have successfully received your payment for <strong>{TourName}</strong>. Please find your reservation and payment summary below.</p>",
                FooterHtml = "<p style=\"margin:0;\">If you have any questions, reply to this email or call us at {CompanyPhone}.</p><p style=\"margin:4px 0 0;\">We look forward to hosting you in Antalya!</p>",
                TermsHtml = "<p style=\"margin:0; font-size:12px; line-height:1.6;\">• Please present this invoice upon arrival.<br/>• Cancellations made 48 hours before departure receive a full refund.<br/>• Pickup details will be shared 24 hours before your tour.</p>"
            };
        }

        private static bool ApplyDefaults(InvoiceSettings settings)
        {
            var changed = false;

            if (string.IsNullOrWhiteSpace(settings.CompanyName))
            {
                settings.CompanyName = "Tour Antalya";
                changed = true;
            }

            if (string.IsNullOrWhiteSpace(settings.CompanyEmail))
            {
                settings.CompanyEmail = "support@tourantalya.com";
                changed = true;
            }

            if (string.IsNullOrWhiteSpace(settings.CompanyPhone))
            {
                settings.CompanyPhone = "+90 242 000 00 00";
                changed = true;
            }

            if (string.IsNullOrWhiteSpace(settings.CompanyAddress))
            {
                settings.CompanyAddress = "Antalya, Turkey";
                changed = true;
            }

            if (string.IsNullOrWhiteSpace(settings.LogoPath))
            {
                settings.LogoPath = "/image/logo.png";
                changed = true;
            }

            if (string.IsNullOrWhiteSpace(settings.AccentColor))
            {
                settings.AccentColor = "#FF6B2C";
                changed = true;
            }

            if (string.IsNullOrWhiteSpace(settings.HeaderHtml))
            {
                settings.HeaderHtml = "<p style=\"margin:0;font-weight:600;font-size:20px;\">Payment Receipt</p><p style=\"margin:4px 0 0;\">Thank you for choosing Tour Antalya.</p>";
                changed = true;
            }

            if (string.IsNullOrWhiteSpace(settings.IntroHtml))
            {
                settings.IntroHtml = "<p>Dear {FullName},</p><p>We have successfully received your payment for <strong>{TourName}</strong>. Please find your reservation and payment summary below.</p>";
                changed = true;
            }

            if (string.IsNullOrWhiteSpace(settings.FooterHtml))
            {
                settings.FooterHtml = "<p style=\"margin:0;\">If you have any questions, reply to this email or call us at {CompanyPhone}.</p><p style=\"margin:4px 0 0;\">We look forward to hosting you in Antalya!</p>";
                changed = true;
            }

            if (string.IsNullOrWhiteSpace(settings.TermsHtml))
            {
                settings.TermsHtml = "<p style=\"margin:0; font-size:12px; line-height:1.6;\">• Please present this invoice upon arrival.<br/>• Cancellations made 48 hours before departure receive a full refund.<br/>• Pickup details will be shared 24 hours before your tour.</p>";
                changed = true;
            }

            if (settings.CreationTime == 0)
            {
                settings.CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                changed = true;
            }

            return changed;
        }
    }
}
