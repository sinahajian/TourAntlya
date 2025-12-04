using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Models.Entities;
using PdfSharpCore.Pdf;
using HtmlRendererCore.PdfSharp;

namespace Models.Services
{
    public class InvoiceDocumentService : IInvoiceDocumentService
    {
        private readonly IInvoiceSettingsRepository _settingsRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<InvoiceDocumentService> _logger;

        public InvoiceDocumentService(
            IInvoiceSettingsRepository settingsRepository,
            IWebHostEnvironment environment,
            ILogger<InvoiceDocumentService> logger)
        {
            _settingsRepository = settingsRepository;
            _environment = environment;
            _logger = logger;
        }

        public async Task<EmailAttachment?> CreateInvoiceAsync(
            TourDto tour,
            ReservationDetailsDto reservation,
            decimal amount,
            string currency,
            CancellationToken ct = default)
        {
            try
            {
                var settings = await _settingsRepository.GetAsync(ct);
                var tokens = BuildTokens(settings, tour, reservation, amount, currency);
                var html = BuildHtmlDocument(settings, tokens, tour, reservation, amount, currency);

                using var pdf = new PdfDocument();
                PdfGenerator.AddPdfPages(pdf, html, PdfSharpCore.PageSize.A4);

                using var stream = new MemoryStream();
                pdf.Save(stream, closeStream: false);
                var fileName = $"TourAntalya-Receipt-{reservation.Id}.pdf";
                return new EmailAttachment(fileName, "application/pdf", stream.ToArray());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Invoice generator: failed to build PDF for reservation {ReservationId}", reservation.Id);
                return null;
            }
        }

        private Dictionary<string, string> BuildTokens(InvoiceSettings settings, TourDto tour, ReservationDetailsDto reservation, decimal amount, string currency)
        {
            var preferredDate = reservation.PreferredDate?.ToString("D", CultureInfo.InvariantCulture) ?? "Not specified";
            var fullName = string.IsNullOrWhiteSpace(reservation.FullName) ? "Guest" : reservation.FullName;
            var amountFormatted = amount.ToString("N2", CultureInfo.InvariantCulture);
            var totalFormatted = reservation.TotalPrice.ToString("N0", CultureInfo.InvariantCulture);
            var invoiceDate = DateTimeOffset.UtcNow.ToString("D", CultureInfo.InvariantCulture);

            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["CompanyName"] = settings.CompanyName,
                ["CompanyEmail"] = settings.CompanyEmail,
                ["CompanyPhone"] = settings.CompanyPhone,
                ["CompanyAddress"] = settings.CompanyAddress,
                ["LogoPath"] = settings.LogoPath,
                ["AccentColor"] = settings.AccentColor,
                ["FullName"] = fullName,
                ["TourName"] = tour.TourName,
                ["ReservationId"] = reservation.Id.ToString(CultureInfo.InvariantCulture),
                ["PreferredDate"] = preferredDate,
                ["Adults"] = reservation.Adults.ToString(CultureInfo.InvariantCulture),
                ["Children"] = reservation.Children.ToString(CultureInfo.InvariantCulture),
                ["Infants"] = reservation.Infants.ToString(CultureInfo.InvariantCulture),
                ["TotalGuests"] = reservation.TotalGuests.ToString(CultureInfo.InvariantCulture),
                ["PickupLocation"] = reservation.PickupLocation,
                ["HotelName"] = string.IsNullOrWhiteSpace(reservation.HotelName) ? "—" : reservation.HotelName!,
                ["RoomNumber"] = string.IsNullOrWhiteSpace(reservation.RoomNumber) ? "—" : reservation.RoomNumber!,
                ["CustomerEmail"] = reservation.CustomerEmail,
                ["CustomerPhone"] = string.IsNullOrWhiteSpace(reservation.CustomerPhone) ? "—" : reservation.CustomerPhone!,
                ["PaymentMethod"] = reservation.PaymentMethod.ToString(),
                ["PaymentReference"] = reservation.PaymentReference ?? "—",
                ["Amount"] = amountFormatted,
                ["Currency"] = currency,
                ["AmountWithCurrency"] = $"{currency} {amountFormatted}",
                ["TotalPrice"] = totalFormatted,
                ["InvoiceDate"] = invoiceDate,
                ["CreatedAt"] = reservation.CreatedAt.ToLocalTime().ToString("f", CultureInfo.InvariantCulture),
                ["Notes"] = string.IsNullOrWhiteSpace(reservation.Notes) ? "—" : reservation.Notes!
            };
        }

        private string BuildHtmlDocument(
            InvoiceSettings settings,
            IReadOnlyDictionary<string, string> tokens,
            TourDto tour,
            ReservationDetailsDto reservation,
            decimal amount,
            string currency)
        {
            var accentColor = string.IsNullOrWhiteSpace(settings.AccentColor) ? "#FF6B2C" : settings.AccentColor;
            var headerHtml = ApplyTokens(settings.HeaderHtml, tokens);
            var introHtml = ApplyTokens(settings.IntroHtml, tokens);
            var footerHtml = ApplyTokens(settings.FooterHtml, tokens);
            var termsHtml = ApplyTokens(settings.TermsHtml, tokens);
            var logoMarkup = BuildLogoMarkup(tokens, settings.LogoPath);

            var guestSummary = BuildGuestSummary(reservation);
            var bookingTable = BuildBookingTable(tokens, amount, currency);

            var html = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""utf-8"" />
    <style>
        :root {{
            --accent: {accentColor};
            --text-primary: #1f2a44;
            --text-muted: #6b7c93;
            --panel-bg: #ffffff;
            --panel-shadow: 0 20px 60px rgba(15, 32, 68, 0.08);
        }}

        body {{
            font-family: 'Segoe UI', 'Helvetica Neue', Arial, sans-serif;
            background-color: #f4f7fb;
            margin: 0;
            color: var(--text-primary);
        }}

        .invoice-wrapper {{
            max-width: 800px;
            margin: 0 auto;
            padding: 40px 32px 48px;
        }}

        .invoice-panel {{
            background: var(--panel-bg);
            border-radius: 24px;
            box-shadow: var(--panel-shadow);
            padding: 40px;
        }}

        .invoice-header {{
            display: flex;
            align-items: center;
            gap: 20px;
            border-bottom: 2px solid rgba(31, 42, 68, 0.08);
            padding-bottom: 24px;
            margin-bottom: 32px;
        }}

        .brand-logo {{
            height: 64px;
            max-width: 200px;
            object-fit: contain;
        }}

        .brand-fallback {{
            font-size: 26px;
            font-weight: 700;
            color: var(--accent);
        }}

        .header-content p {{
            margin: 0 0 6px;
            color: var(--text-muted);
        }}

        .intro {{
            margin-bottom: 28px;
            line-height: 1.7;
        }}

        .summary-cards {{
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(160px, 1fr));
            gap: 18px;
            margin-bottom: 30px;
        }}

        .summary-card {{
            background: linear-gradient(135deg, rgba(255, 107, 44, 0.08), rgba(13, 110, 253, 0.08));
            border-radius: 16px;
            padding: 18px 20px;
        }}

        .summary-card strong {{
            display: block;
            font-size: 13px;
            letter-spacing: 0.08em;
            text-transform: uppercase;
            color: var(--text-muted);
            margin-bottom: 6px;
        }}

        .summary-card span {{
            font-size: 20px;
            font-weight: 600;
            color: var(--text-primary);
        }}

        .booking-table {{
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 28px;
        }}

        .booking-table th,
        .booking-table td {{
            padding: 14px 16px;
            border-bottom: 1px solid rgba(31, 42, 68, 0.08);
            text-align: left;
            vertical-align: top;
        }}

        .booking-table th {{
            width: 32%;
            font-size: 13px;
            text-transform: uppercase;
            letter-spacing: 0.08em;
            color: var(--text-muted);
        }}

        .booking-table td strong {{
            color: var(--text-primary);
        }}

        .notes-block {{
            background: rgba(13, 110, 253, 0.06);
            border-radius: 16px;
            padding: 20px 22px;
            margin-bottom: 30px;
            line-height: 1.6;
        }}

        .invoice-footer {{
            border-top: 2px solid rgba(31, 42, 68, 0.08);
            padding-top: 22px;
            color: var(--text-muted);
            line-height: 1.6;
            font-size: 13px;
        }}

        .terms {{
            margin-top: 22px;
            font-size: 12px;
            color: var(--text-muted);
            line-height: 1.6;
        }}
    </style>
</head>
<body>
<div class=""invoice-wrapper"">
    <div class=""invoice-panel"">
        <header class=""invoice-header"">
            {logoMarkup}
            <div class=""header-content"">{headerHtml}</div>
        </header>

        <section class=""intro"">
            {introHtml}
        </section>

        <div class=""summary-cards"">
            <div class=""summary-card"">
                <strong>Total paid</strong>
                <span>{tokens["AmountWithCurrency"]}</span>
            </div>
            <div class=""summary-card"">
                <strong>Reservation ID</strong>
                <span>#{tokens["ReservationId"]}</span>
            </div>
            <div class=""summary-card"">
                <strong>Guests</strong>
                <span>{tokens["TotalGuests"]}</span>
            </div>
            <div class=""summary-card"">
                <strong>Tour date</strong>
                <span>{tokens["PreferredDate"]}</span>
            </div>
        </div>

        {guestSummary}
        {bookingTable}

        <div class=""notes-block"">
            <strong>Special notes</strong><br/>
            {tokens["Notes"]}
        </div>

        <footer class=""invoice-footer"">
            {footerHtml}
        </footer>

        {(string.IsNullOrWhiteSpace(termsHtml) ? string.Empty : $@"<div class=""terms"">{termsHtml}</div>")}
    </div>
</div>
</body>
</html>";

            return html;
        }

        private string BuildLogoMarkup(IReadOnlyDictionary<string, string> tokens, string? logoPath)
        {
            var logoUrl = ResolveLogoDataUrl(logoPath);
            if (!string.IsNullOrWhiteSpace(logoUrl))
            {
                return $@"<img src=""{logoUrl}"" alt=""{tokens["CompanyName"]}"" class=""brand-logo"" />";
            }

            return $@"<div class=""brand-fallback"">{tokens["CompanyName"]}</div>";
        }

        private string BuildGuestSummary(ReservationDetailsDto reservation)
        {
            var builder = new StringBuilder();
            builder.Append("<table class=\"booking-table\"><tbody>");
            builder.Append("<tr><th>Lead guest</th><td><strong>")
                .Append(reservation.FullName)
                .Append("</strong><br/>")
                .Append(string.IsNullOrWhiteSpace(reservation.CustomerEmail) ? "—" : reservation.CustomerEmail)
                .Append("<br/>")
                .Append(string.IsNullOrWhiteSpace(reservation.CustomerPhone) ? "—" : reservation.CustomerPhone)
                .Append("</td></tr>");
            builder.Append("<tr><th>Hotel / pickup</th><td>")
                .Append(string.IsNullOrWhiteSpace(reservation.HotelName) ? reservation.PickupLocation : $"{reservation.HotelName}<br/>{reservation.PickupLocation}")
                .Append("</td></tr>");
            builder.Append("<tr><th>Room number</th><td>")
                .Append(string.IsNullOrWhiteSpace(reservation.RoomNumber) ? "—" : reservation.RoomNumber)
                .Append("</td></tr>");
            builder.Append("</tbody></table>");
            return builder.ToString();
        }

        private string BuildBookingTable(IReadOnlyDictionary<string, string> tokens, decimal amount, string currency)
        {
            var builder = new StringBuilder();
            builder.Append("<table class=\"booking-table\"><tbody>");
            builder.Append("<tr><th>Tour</th><td>")
                .Append(tokens["TourName"])
                .Append("</td></tr>");
            builder.Append("<tr><th>Date</th><td>")
                .Append(tokens["PreferredDate"])
                .Append("</td></tr>");
            builder.Append("<tr><th>Guests</th><td>")
                .Append($"Adults {tokens["Adults"]}, Children {tokens["Children"]}, Infants {tokens["Infants"]}")
                .Append("</td></tr>");
            builder.Append("<tr><th>Payment method</th><td>")
                .Append(tokens["PaymentMethod"])
                .Append("</td></tr>");
            builder.Append("<tr><th>Payment reference</th><td>")
                .Append(tokens["PaymentReference"])
                .Append("</td></tr>");
            builder.Append("<tr><th>Amount paid</th><td><strong>")
                .Append(tokens["AmountWithCurrency"])
                .Append("</strong></td></tr>");
            builder.Append("<tr><th>Invoice date</th><td>")
                .Append(tokens["InvoiceDate"])
                .Append("</td></tr>");
            builder.Append("</tbody></table>");
            return builder.ToString();
        }

        private string ApplyTokens(string? template, IReadOnlyDictionary<string, string> tokens)
        {
            if (string.IsNullOrWhiteSpace(template))
            {
                return string.Empty;
            }

            var result = template;
            foreach (var (key, value) in tokens)
            {
                result = result.Replace("{" + key + "}", value ?? string.Empty, StringComparison.OrdinalIgnoreCase);
            }

            return result;
        }

        private string? ResolveLogoDataUrl(string? logoPath)
        {
            if (string.IsNullOrWhiteSpace(logoPath) || string.IsNullOrWhiteSpace(_environment.WebRootPath))
            {
                return null;
            }

            try
            {
                var trimmed = logoPath.Trim().TrimStart('~').TrimStart('/');
                if (string.IsNullOrWhiteSpace(trimmed))
                {
                    return null;
                }

                var fullPath = Path.Combine(_environment.WebRootPath, trimmed.Replace('/', Path.DirectorySeparatorChar));
                if (!File.Exists(fullPath))
                {
                    _logger.LogWarning("Invoice generator: logo not found at {Path}", fullPath);
                    return null;
                }

                var extension = Path.GetExtension(fullPath).ToLowerInvariant();
                if (extension == ".svg")
                {
                    var svg = File.ReadAllText(fullPath);
                    return $"data:image/svg+xml;utf8,{Uri.EscapeDataString(svg)}";
                }

                var bytes = File.ReadAllBytes(fullPath);
                var mime = extension switch
                {
                    ".png" => "image/png",
                    ".jpg" => "image/jpeg",
                    ".jpeg" => "image/jpeg",
                    ".gif" => "image/gif",
                    _ => "image/png"
                };

                return $"data:{mime};base64,{Convert.ToBase64String(bytes)}";
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Invoice generator: failed to load logo from {Path}", logoPath);
                return null;
            }
        }
    }
}
