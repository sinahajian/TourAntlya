using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.DbContexts;
using Models.Entities;

namespace Models.Repository
{
    public class EmailConfigurationRepository : IEmailConfigurationRepository
    {
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
                settings = new SmtpSettings { Id = 1 };
                settings.CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                _context.SmtpSettings.Add(settings);
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
            return await _context.EmailTemplates
                .AsNoTracking()
                .ToDictionaryAsync(t => t.TemplateKey, ct);
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
    }
}
