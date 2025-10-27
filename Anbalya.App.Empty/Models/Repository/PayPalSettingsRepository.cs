using System;
using Microsoft.EntityFrameworkCore;
using Models.DbContexts;
using Models.Entities;
using Models.Interface;

namespace Models.Repository
{
    public class PayPalSettingsRepository : IPayPalSettingsRepository
    {
        private readonly TourDbContext _context;

        public PayPalSettingsRepository(TourDbContext context)
        {
            _context = context;
        }

        public async Task<PayPalSettings> GetAsync(CancellationToken ct = default)
        {
            var settings = await _context.PayPalSettings.FirstOrDefaultAsync(ct);
            if (settings is not null) return settings;

            settings = new PayPalSettings
            {
                BusinessEmail = string.Empty,
                Currency = "EUR",
                ReturnUrl = string.Empty,
                CancelUrl = string.Empty,
                UseSandbox = true,
                CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };
            await _context.PayPalSettings.AddAsync(settings, ct);
            await _context.SaveChangesAsync(ct);
            return settings;
        }

        public async Task UpdateAsync(PayPalSettings settings, CancellationToken ct = default)
        {
            var existing = await _context.PayPalSettings.FirstOrDefaultAsync(ct);
            if (existing is null)
            {
                settings.CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                await _context.PayPalSettings.AddAsync(settings, ct);
            }
            else
            {
                existing.BusinessEmail = settings.BusinessEmail;
                existing.Currency = settings.Currency;
                existing.ReturnUrl = settings.ReturnUrl;
                existing.CancelUrl = settings.CancelUrl;
                existing.UseSandbox = settings.UseSandbox;
            }

            await _context.SaveChangesAsync(ct);
        }
    }
}
