using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.DbContexts;
using Models.Entities;

namespace Models.Repository
{
    public class ContactMessageRepository : IContactMessageRepository
    {
        private readonly TourDbContext _context;

        public ContactMessageRepository(TourDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(ContactMessage message, CancellationToken ct = default)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            message.CreationTime = now;
            message.UpdatedTime = now;
            _context.ContactMessages.Add(message);
            await _context.SaveChangesAsync(ct);
            return message.Id;
        }

        public async Task<ContactMessage?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.ContactMessages.FirstOrDefaultAsync(m => m.Id == id, ct);
        }

        public async Task<IReadOnlyList<ContactMessage>> ListAsync(ContactMessageStatus? status = null, CancellationToken ct = default)
        {
            var query = _context.ContactMessages.AsQueryable();
            if (status.HasValue)
            {
                query = query.Where(m => m.Status == status.Value);
            }

            return await query
                .OrderByDescending(m => m.CreationTime)
                .ToListAsync(ct);
        }

        public async Task UpdateStatusAsync(int id, ContactMessageStatus status, CancellationToken ct = default)
        {
            var message = await _context.ContactMessages.FirstOrDefaultAsync(m => m.Id == id, ct);
            if (message is null) return;

            message.Status = status;
            message.UpdatedTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            await _context.SaveChangesAsync(ct);
        }
    }
}
