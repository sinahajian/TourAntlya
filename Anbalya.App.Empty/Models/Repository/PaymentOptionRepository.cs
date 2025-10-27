using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.DbContexts;
using Models.Entities;
using Models.Interface;

namespace Models.Repository
{
    public class PaymentOptionRepository : IPaymentOptionRepository
    {
        private readonly TourDbContext _context;

        public PaymentOptionRepository(TourDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<PaymentOption>> ListAsync(CancellationToken ct = default)
        {
            return await _context.PaymentOptions
                .AsNoTracking()
                .OrderBy(p => p.Method)
                .ToListAsync(ct);
        }

        public async Task<PaymentOption?> GetByMethodAsync(PaymentMethod method, CancellationToken ct = default)
        {
            return await _context.PaymentOptions
                .FirstOrDefaultAsync(p => p.Method == method, ct);
        }

        public async Task UpdateAsync(PaymentOption option, CancellationToken ct = default)
        {
            var existing = await _context.PaymentOptions
                .FirstOrDefaultAsync(p => p.Method == option.Method, ct);

            if (existing is null)
            {
                option.CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                option.UpdatedAt = DateTimeOffset.UtcNow;
                await _context.PaymentOptions.AddAsync(option, ct);
            }
            else
            {
                existing.DisplayName = option.DisplayName;
                existing.AccountIdentifier = option.AccountIdentifier;
                existing.Instructions = option.Instructions;
                existing.IsEnabled = option.IsEnabled;
                existing.UpdatedAt = DateTimeOffset.UtcNow;
            }

            await _context.SaveChangesAsync(ct);
        }
    }
}
