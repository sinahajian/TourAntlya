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

    public static EmailSettingsViewModel FromEntities(string userName, SmtpSettings smtp, IReadOnlyDictionary<string, EmailTemplate> templates)
    {
        EmailTemplate GetTemplate(string key)
        {
            return templates.TryGetValue(key, out var template)
                ? template
                : new EmailTemplate { TemplateKey = key };
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
            ReservationAdmin = EmailTemplateEditModel.FromEntity(GetTemplate(EmailTemplateKeys.ReservationAdmin))
        };
    }

    public (SmtpSettings smtp, List<EmailTemplate> templates) ToEntities()
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

        return (smtp, templates);
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
            Subject = template.Subject,
            Body = template.Body
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
}
