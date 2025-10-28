using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models.Entities;

public interface IEmailConfigurationRepository
{
    Task<SmtpSettings> GetSmtpSettingsAsync(CancellationToken ct = default);
    Task UpdateSmtpSettingsAsync(SmtpSettings settings, CancellationToken ct = default);
    Task<IReadOnlyDictionary<string, EmailTemplate>> GetTemplatesAsync(CancellationToken ct = default);
    Task UpdateTemplatesAsync(IEnumerable<EmailTemplate> templates, CancellationToken ct = default);
    Task<EmailTemplate?> GetTemplateAsync(string templateKey, string? language = null, CancellationToken ct = default);
}
