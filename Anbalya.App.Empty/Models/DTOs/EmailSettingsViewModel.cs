using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Models.Entities;
using Models.Helper;

public class EmailSettingsViewModel
{
    public string UserName { get; set; } = string.Empty;
    public string? FeedbackMessage { get; set; }

    [Display(Name = "SMTP Host")]
    public string Host { get; set; } = string.Empty;

    [Display(Name = "SMTP Port")]
    public int Port { get; set; } = 587;

    [Display(Name = "Enable SSL")]
    public bool EnableSsl { get; set; } = true;

    [Display(Name = "SMTP Username")]
    public string Username { get; set; } = string.Empty;

    [Display(Name = "SMTP Password")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "From Email")]
    [EmailAddress]
    public string FromEmail { get; set; } = string.Empty;

    [Display(Name = "From Name")]
    public string FromName { get; set; } = string.Empty;

    [Display(Name = "Notification Email")]
    [EmailAddress]
    public string NotificationEmail { get; set; } = string.Empty;

    [Display(Name = "Reply-to Email")]
    [EmailAddress]
    public string? ReplyToEmail { get; set; }

    public EmailTemplateEditModel ContactUser { get; set; } = new EmailTemplateEditModel();
    public EmailTemplateEditModel ContactAdmin { get; set; } = new EmailTemplateEditModel();
    public EmailTemplateEditModel ReservationUser { get; set; } = new EmailTemplateEditModel();
    public EmailTemplateEditModel ReservationAdmin { get; set; } = new EmailTemplateEditModel();
    public InvoiceSettingsEditModel Invoice { get; set; } = new InvoiceSettingsEditModel();

    public static EmailSettingsViewModel FromEntities(string userName, SmtpSettings smtp, IReadOnlyDictionary<string, EmailTemplate> templates, InvoiceSettings invoice)
    {
        EmailTemplate GetTemplate(string key)
        {
            return templates.TryGetValue(key, out var template)
                ? template
                : EmailTemplateEditModel.GetDefaultTemplateEntity(key);
        }

        return new EmailSettingsViewModel
        {
            UserName = userName,
            Host = smtp.Host,
            Port = smtp.Port,
            EnableSsl = smtp.EnableSsl,
            Username = smtp.Username,
            Password = smtp.Password,
            FromEmail = smtp.FromEmail,
            FromName = smtp.FromName,
            NotificationEmail = smtp.NotificationEmail,
            ReplyToEmail = smtp.ReplyToEmail,
            ContactUser = EmailTemplateEditModel.FromEntity(GetTemplate(EmailTemplateKeys.ContactUser)),
            ContactAdmin = EmailTemplateEditModel.FromEntity(GetTemplate(EmailTemplateKeys.ContactAdmin)),
            ReservationUser = EmailTemplateEditModel.FromEntity(GetTemplate(EmailTemplateKeys.ReservationUser)),
            ReservationAdmin = EmailTemplateEditModel.FromEntity(GetTemplate(EmailTemplateKeys.ReservationAdmin)),
            Invoice = InvoiceSettingsEditModel.FromEntity(invoice)
        };
    }

    public (SmtpSettings smtp, List<EmailTemplate> templates, InvoiceSettings invoice) ToEntities()
    {
        var smtp = new SmtpSettings
        {
            Id = 1,
            Host = Host?.Trim() ?? string.Empty,
            Port = Port,
            EnableSsl = EnableSsl,
            Username = Username?.Trim() ?? string.Empty,
            Password = Password ?? string.Empty,
            FromEmail = FromEmail?.Trim() ?? string.Empty,
            FromName = FromName?.Trim() ?? string.Empty,
            NotificationEmail = NotificationEmail?.Trim() ?? string.Empty,
            ReplyToEmail = string.IsNullOrWhiteSpace(ReplyToEmail) ? null : ReplyToEmail.Trim()
        };

        var datetime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        smtp.CreationTime = smtp.CreationTime == 0 ? datetime : smtp.CreationTime;

        var templates = new List<EmailTemplate>
        {
            ContactUser.ToEntity(EmailTemplateKeys.ContactUser),
            ContactAdmin.ToEntity(EmailTemplateKeys.ContactAdmin),
            ReservationUser.ToEntity(EmailTemplateKeys.ReservationUser),
            ReservationAdmin.ToEntity(EmailTemplateKeys.ReservationAdmin)
        };

        foreach (var template in templates)
        {
            if (template.CreationTime == 0)
            {
                template.CreationTime = datetime;
            }
        }

        var invoice = Invoice.ToEntity();
        if (invoice.CreationTime == 0)
        {
            invoice.CreationTime = datetime;
        }

        return (smtp, templates, invoice);
    }
}

public class EmailTemplateEditModel
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Subject")]
    public string Subject { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Body")]
    public string Body { get; set; } = string.Empty;

    public static EmailTemplateEditModel FromEntity(EmailTemplate template)
    {
        return new EmailTemplateEditModel
        {
            Id = template.Id,
            Subject = template.Subject ?? string.Empty,
            Body = template.Body ?? string.Empty
        };
    }

    public EmailTemplate ToEntity(string templateKey)
    {
        return new EmailTemplate
        {
            Id = Id == 0 ? 0 : Id,
            TemplateKey = templateKey,
            Subject = Subject?.Trim() ?? string.Empty,
            Body = Body?.Trim() ?? string.Empty
        };
    }

    internal static EmailTemplate GetDefaultTemplateEntity(string templateKey)
    {
        return templateKey switch
        {
            EmailTemplateKeys.ContactUser => new EmailTemplate
            {
                TemplateKey = templateKey,
                Subject = "We received your message – Tour Antalya",
                Body = "<p>Hi {FullName},</p><p>Thank you for contacting <strong>Tour Antalya</strong>. Our support team will respond within 24 hours.</p><p style=\"margin:16px 0; padding:12px 16px; background:#f5f7fb; border-radius:8px;\">Your message:<br/><em>{Message}</em></p><p style=\"margin:0;\">Warm regards,<br/>Tour Antalya Concierge</p>"
            },
            EmailTemplateKeys.ContactAdmin => new EmailTemplate
            {
                TemplateKey = templateKey,
                Subject = "New contact message from {FullName}",
                Body = "<p><strong>New contact request received.</strong></p><table style=\"width:100%;border-collapse:collapse;\"><tr><th align=\"left\" style=\"padding:6px 8px;border-bottom:1px solid #dbe3f0;\">Guest</th><td style=\"padding:6px 8px;border-bottom:1px solid #dbe3f0;\">{FullName}</td></tr><tr><th align=\"left\" style=\"padding:6px 8px;border-bottom:1px solid #dbe3f0;\">Email</th><td style=\"padding:6px 8px;border-bottom:1px solid #dbe3f0;\">{Email}</td></tr><tr><th align=\"left\" style=\"padding:6px 8px;border-bottom:1px solid #dbe3f0;\">Language</th><td style=\"padding:6px 8px;border-bottom:1px solid #dbe3f0;\">{Language}</td></tr><tr><th align=\"left\" style=\"padding:6px 8px;\">Received</th><td style=\"padding:6px 8px;\">{CreatedAt}</td></tr></table><p style=\"margin:12px 0 0;\">Message:</p><blockquote style=\"margin:0; padding:12px 16px; background:#f1f5fb; border-left:3px solid #FF6B2C;\">{Message}</blockquote>"
            },
            EmailTemplateKeys.ReservationUser => new EmailTemplate
            {
                TemplateKey = templateKey,
                Subject = "Reservation confirmed – {TourName}",
                Body = "<p>Dear {FullName},</p><p>Your reservation for <strong>{TourName}</strong> is confirmed.</p><ul style=\"padding-left:18px;margin:12px 0;\"><li>Reservation ID: #{ReservationId}</li><li>Preferred date: {PreferredDate}</li><li>Guests: Adults {Adults}, Children {Children}, Infants {Infants}</li><li>Payment method: {PaymentMethod}</li></ul><p>We will share pickup instructions shortly. For changes, reply to this email or call {CompanyPhone}.</p><p style=\"margin:0;\">Warm regards,<br/>Tour Antalya Concierge</p>"
            },
            EmailTemplateKeys.ReservationAdmin => new EmailTemplate
            {
                TemplateKey = templateKey,
                Subject = "New reservation – {TourName}",
                Body = "<p><strong>A new reservation has been placed.</strong></p><table style=\"width:100%;border-collapse:collapse;\"><tr><th align=\"left\" style=\"padding:6px 8px;border-bottom:1px solid #dbe3f0;\">Tour</th><td style=\"padding:6px 8px;border-bottom:1px solid #dbe3f0;\">{TourName}</td></tr><tr><th align=\"left\" style=\"padding:6px 8px;border-bottom:1px solid #dbe3f0;\">Guest</th><td style=\"padding:6px 8px;border-bottom:1px solid #dbe3f0;\">{FullName} ({Email})</td></tr><tr><th align=\"left\" style=\"padding:6px 8px;border-bottom:1px solid #dbe3f0;\">Preferred date</th><td style=\"padding:6px 8px;border-bottom:1px solid #dbe3f0;\">{PreferredDate}</td></tr><tr><th align=\"left\" style=\"padding:6px 8px;border-bottom:1px solid #dbe3f0;\">Guests</th><td style=\"padding:6px 8px;border-bottom:1px solid #dbe3f0;\">Adults {Adults} · Children {Children} · Infants {Infants}</td></tr><tr><th align=\"left\" style=\"padding:6px 8px;\">Payment reference</th><td style=\"padding:6px 8px;\">{PaymentReference}</td></tr></table><p style=\"margin:12px 0 0;\">Notes: {Notes}</p>"
            },
            _ => new EmailTemplate { TemplateKey = templateKey, Subject = string.Empty, Body = string.Empty }
        };
    }
}

public class InvoiceSettingsEditModel
{
    public int Id { get; set; }

    [Display(Name = "Company name")]
    [Required]
    [MaxLength(200)]
    public string CompanyName { get; set; } = string.Empty;

    [Display(Name = "Company email")]
    [EmailAddress]
    public string CompanyEmail { get; set; } = string.Empty;

    [Display(Name = "Company phone")]
    [MaxLength(100)]
    public string CompanyPhone { get; set; } = string.Empty;

    [Display(Name = "Company address")]
    [MaxLength(400)]
    public string CompanyAddress { get; set; } = string.Empty;

    [Display(Name = "Invoice logo (relative path)")]
    [MaxLength(200)]
    public string LogoPath { get; set; } = string.Empty;

    [Display(Name = "Accent color (HEX)")]
    [RegularExpression("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Please provide a valid HEX color, e.g. #FF6B2C.")]
    public string AccentColor { get; set; } = "#FF6B2C";

    [Display(Name = "Header content")]
    [Required]
    public string HeaderHtml { get; set; } = string.Empty;

    [Display(Name = "Introductory text")]
    [Required]
    public string IntroHtml { get; set; } = string.Empty;

    [Display(Name = "Footer content")]
    [Required]
    public string FooterHtml { get; set; } = string.Empty;

    [Display(Name = "Terms & notes")]
    public string TermsHtml { get; set; } = string.Empty;

    public long CreationTime { get; set; }

    public static InvoiceSettingsEditModel FromEntity(InvoiceSettings settings)
    {
        return new InvoiceSettingsEditModel
        {
            Id = settings.Id,
            CompanyName = settings.CompanyName,
            CompanyEmail = settings.CompanyEmail,
            CompanyPhone = settings.CompanyPhone,
            CompanyAddress = settings.CompanyAddress,
            LogoPath = settings.LogoPath,
            AccentColor = settings.AccentColor,
            HeaderHtml = settings.HeaderHtml,
            IntroHtml = settings.IntroHtml,
            FooterHtml = settings.FooterHtml,
            TermsHtml = settings.TermsHtml,
            CreationTime = settings.CreationTime
        };
    }

    public InvoiceSettings ToEntity()
    {
        return new InvoiceSettings
        {
            Id = Id == 0 ? 0 : Id,
            CompanyName = CompanyName?.Trim() ?? string.Empty,
            CompanyEmail = CompanyEmail?.Trim() ?? string.Empty,
            CompanyPhone = CompanyPhone?.Trim() ?? string.Empty,
            CompanyAddress = CompanyAddress?.Trim() ?? string.Empty,
            LogoPath = LogoPath?.Trim() ?? string.Empty,
            AccentColor = string.IsNullOrWhiteSpace(AccentColor) ? "#FF6B2C" : AccentColor.Trim(),
            HeaderHtml = HeaderHtml ?? string.Empty,
            IntroHtml = IntroHtml ?? string.Empty,
            FooterHtml = FooterHtml ?? string.Empty,
            TermsHtml = TermsHtml ?? string.Empty,
            CreationTime = CreationTime
        };
    }
}
