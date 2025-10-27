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
    public class ReservationRepository : IReservationRepository
    {
        private readonly TourDbContext _context;

        public ReservationRepository(TourDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(Reservation reservation, CancellationToken ct = default)
        {
            reservation.CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            reservation.CreatedAt = DateTimeOffset.UtcNow;
            await _context.Reservations.AddAsync(reservation, ct);
            await _context.SaveChangesAsync(ct);

            return reservation.Id;
        }

        public async Task<IReadOnlyList<Reservation>> ListAsync(int? tourId = null, CancellationToken ct = default)
        {
            var query = _context.Reservations
                .AsNoTracking()
                .Include(r => r.Tour)
                .AsQueryable();

            if (tourId.HasValue)
            {
                query = query.Where(r => r.TourId == tourId.Value);
            }

            return await query
                .OrderByDescending(r => r.CreatedAt)
                .ThenByDescending(r => r.Id)
                .ToListAsync(ct);
        }

        public async Task<Reservation?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Reservations
                .AsNoTracking()
                .Include(r => r.Tour)
                .FirstOrDefaultAsync(r => r.Id == id, ct);
        }

        public async Task UpdateStatusAsync(int id, ReservationStatus status, PaymentStatus paymentStatus, string? paymentReference, CancellationToken ct = default)
        {
            var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == id, ct);
            if (reservation is null)
            {
                return;
            }

            reservation.Status = status;
            reservation.PaymentStatus = paymentStatus;
            reservation.PaymentReference = paymentReference;
            reservation.UpdatedAt = DateTimeOffset.UtcNow;

            await _context.SaveChangesAsync(ct);
        }
    }
}
