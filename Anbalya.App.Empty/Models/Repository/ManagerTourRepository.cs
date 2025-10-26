using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.DbContexts;
using Models.Interface;
using Models.Entities;

namespace Models.Repository
{
    public class ManagerTourRepository : IManagerTourRepository
    {
        private readonly TourDbContext _context;

        public ManagerTourRepository(TourDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ManagerTourListItemDto>> ListAsync(CancellationToken ct = default)
        {
            return await _context.Tours
                .AsNoTracking()
                .Include(t => t.Fotos)
                .OrderBy(t => t.TourName)
                .Select(t => new ManagerTourListItemDto(
                    t.Id,
                    t.TourName,
                    t.Category,
                    t.Price,
                    t.KinderPrice,
                    t.InfantPrice,
                    t.Foto,
                    t.Fotos
                        .Select(f => f.Address)
                        .OrderBy(address => address)
                        .ToList()))
                .ToListAsync(ct);
        }

        public async Task DeleteAsync(int tourId, CancellationToken ct = default)
        {
            var tour = await _context.Tours
                .Include(t => t.Fotos)
                .FirstOrDefaultAsync(t => t.Id == tourId, ct);

            if (tour is null)
            {
                return;
            }

            if (tour.Fotos.Any())
            {
                _context.RemoveRange(tour.Fotos);
            }

            _context.Tours.Remove(tour);
            await _context.SaveChangesAsync(ct);
        }

        public async Task SetPrimaryPhotoAsync(int tourId, string photoAddress, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(photoAddress))
            {
                return;
            }

            var tour = await _context.Tours
                .Include(t => t.Fotos)
                .FirstOrDefaultAsync(t => t.Id == tourId, ct);

            if (tour is null)
            {
                return;
            }

            var normalizedPhoto = photoAddress.Trim();

            bool photoExists = tour.Fotos.Any(f => f.Address == normalizedPhoto)
                               || string.Equals(tour.Foto, normalizedPhoto, StringComparison.Ordinal);

            if (!photoExists)
            {
                return;
            }

            tour.Foto = normalizedPhoto;
            await _context.SaveChangesAsync(ct);
        }

        public async Task<ManagerTourEditDto?> GetForEditAsync(int tourId, CancellationToken ct = default)
        {
            var tour = await _context.Tours
                .Include(t => t.Fotos)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == tourId, ct);

            if (tour is null)
            {
                return null;
            }

            return new ManagerTourEditDto
            {
                Id = tour.Id,
                TourName = tour.TourName,
                Category = tour.Category,
                Price = tour.Price,
                KinderPrice = tour.KinderPrice,
                InfantPrice = tour.InfantPrice,
                DurationHours = tour.DurationHours,
                LocLat = tour.LocLat,
                LocLon = tour.LocLon,
                ActiveDay = tour.ActiveDay,
                DescriptionEn = tour.DescriptionEn,
                DescriptionDe = tour.DescriptionDe,
                DescriptionRu = tour.DescriptionRu,
                DescriptionPo = tour.DescriptionPo,
                DescriptionPe = tour.DescriptionPe,
                DescriptionAr = tour.DescriptionAr,
                MiniDescriptionEn = tour.MiniDescriptionEn,
                MiniDescriptionDe = tour.MiniDescriptionDe,
                MiniDescriptionRu = tour.MiniDescriptionRu,
                MiniDescriptionPo = tour.MiniDescriptionPo,
                MiniDescriptionPe = tour.MiniDescriptionPe,
                MiniDescriptionAr = tour.MiniDescriptionAr,
                Services = tour.Services?.ToList() ?? new List<string>(),
                Photos = tour.Fotos.Select(f => f.Address).OrderBy(address => address).ToList(),
                PrimaryPhoto = tour.Foto
            };
        }

        public async Task UpdateAsync(ManagerTourEditDto dto, CancellationToken ct = default)
        {
            var tour = await _context.Tours.FirstOrDefaultAsync(t => t.Id == dto.Id, ct);

            if (tour is null)
            {
                return;
            }

            tour.TourName = dto.TourName;
            tour.Category = dto.Category;
            tour.Price = dto.Price;
            tour.KinderPrice = dto.KinderPrice;
            tour.InfantPrice = dto.InfantPrice;
            tour.DurationHours = dto.DurationHours;
            tour.LocLat = dto.LocLat;
            tour.LocLon = dto.LocLon;
            tour.ActiveDay = dto.ActiveDay;
            tour.DescriptionEn = dto.DescriptionEn;
            tour.DescriptionDe = dto.DescriptionDe;
            tour.DescriptionRu = dto.DescriptionRu;
            tour.DescriptionPo = dto.DescriptionPo;
            tour.DescriptionPe = dto.DescriptionPe;
            tour.DescriptionAr = dto.DescriptionAr;
            tour.MiniDescriptionEn = dto.MiniDescriptionEn;
            tour.MiniDescriptionDe = dto.MiniDescriptionDe;
            tour.MiniDescriptionRu = dto.MiniDescriptionRu;
            tour.MiniDescriptionPo = dto.MiniDescriptionPo;
            tour.MiniDescriptionPe = dto.MiniDescriptionPe;
            tour.MiniDescriptionAr = dto.MiniDescriptionAr;
            tour.Services = dto.Services?.ToList() ?? new List<string>();

            await _context.SaveChangesAsync(ct);
        }

        public async Task<int> AddTourAsync(ManagerTourCreateDto dto, IEnumerable<string> photoAddresses, CancellationToken ct = default)
        {
            var addresses = photoAddresses?
                .Where(a => !string.IsNullOrWhiteSpace(a))
                .Select(a => a.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList() ?? new List<string>();

            var fotos = addresses
                .Select(address => new Foto(address))
                .ToList();

            var tour = new Tour
            {
                TourName = dto.TourName,
                Category = dto.Category,
                Price = dto.Price,
                KinderPrice = dto.KinderPrice,
                InfantPrice = dto.InfantPrice,
                DurationHours = dto.DurationHours,
                LocLat = dto.LocLat,
                LocLon = dto.LocLon,
                Services = dto.Services ?? new List<string>(),
                CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Foto = fotos.FirstOrDefault()?.Address,
                Fotos = fotos,
                ActiveDay = dto.ActiveDay
            };

            foreach (var foto in fotos)
            {
                foto.Tour = tour;
            }

            _context.Tours.Add(tour);
            await _context.SaveChangesAsync(ct);

            return tour.Id;
        }

        public async Task AddPhotosAsync(int tourId, IEnumerable<string> photoAddresses, CancellationToken ct = default)
        {
            var addresses = photoAddresses?
                .Where(a => !string.IsNullOrWhiteSpace(a))
                .Select(a => a.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList() ?? new List<string>();

            if (addresses.Count == 0)
            {
                return;
            }

            var tour = await _context.Tours
                .Include(t => t.Fotos)
                .FirstOrDefaultAsync(t => t.Id == tourId, ct);

            if (tour is null)
            {
                return;
            }

            var existingAddresses = new HashSet<string>(tour.Fotos.Select(f => f.Address), StringComparer.OrdinalIgnoreCase);

            foreach (var address in addresses)
            {
                if (existingAddresses.Contains(address))
                {
                    continue;
                }

                var foto = new Foto(address)
                {
                    TourId = tour.Id
                };
                _context.Fotos.Add(foto);
                tour.Fotos.Add(foto);
                existingAddresses.Add(address);
            }

            if (string.IsNullOrWhiteSpace(tour.Foto))
            {
                tour.Foto = addresses.First();
            }

            await _context.SaveChangesAsync(ct);
        }
    }
}
