using Microsoft.EntityFrameworkCore;
using Models.DbContexts;
using Models.Entities;
using Models.Helper;
using Models.Interface;

namespace Models.Repository
{
    public class TourRepository : ITourRepository
    {
        public TourDbContext _tourDbContext;
        public TourRepository(TourDbContext tourDbContext)
        {
            _tourDbContext = tourDbContext;
        }
        public List<Tour> GetAll()
        {
            return _tourDbContext.Tours.ToList();
        }


        public async Task<TourDto?> GetByIdAsync(int id, string lang, CancellationToken ct = default)
        {
            return await _tourDbContext.Tours
                .Where(t => t.Id == id)
                .Select(t => new TourDto(
                    t.Id,
                    t.TourName,
                    t.Price,
                    t.KinderPrice,
                    t.InfantPrice,
                    t.LocLat,
                    t.LocLon,
                      t.Category,
                    t.Category.GetDisplayName(lang),
                    DescriptionForLang(t, lang),
                    MiniDescriptionForLang(t, lang),
                    t.DurationHours,
                    t.Services,                      // ← اگر ValueConverter برای List<string> داری اوکیه
                    t.Foto,
                    t.Fotos.Select(f => f.Address),
                    lang
                ))
                .FirstOrDefaultAsync(ct);
        }

        public async Task<List<TourDto>> ListAsync(string lang, CancellationToken ct = default)
        {
            return await _tourDbContext.Tours
                .Select(t => new TourDto(
                    t.Id,
                    t.TourName,
                    t.Price,
                    t.KinderPrice,
                    t.InfantPrice,
                    t.LocLat,
                    t.LocLon,
                    t.Category,
                    t.Category.GetDisplayName(lang),
                    DescriptionForLang(t, lang),
                    MiniDescriptionForLang(t, lang),
                    t.DurationHours,
                    t.Services,
                    t.Foto,
                    t.Fotos.Select(f => f.Address),
                    lang
                ))
                .ToListAsync(ct);
        }

        // انتخاب توضیح با fallback: اول زبان موردنظر، اگر خالی بود به انگلیسی، بعد هر کدام که پر بود
        private static string DescriptionForLang(Tour t, string lang)
        {
            string pick = lang switch
            {
                "de" => t.DescriptionDe,
                "fa" => t.DescriptionPe,
                "ru" => t.DescriptionRu,
                "pl" => t.DescriptionPo,
                "ar" => t.DescriptionAr,
                _ => t.DescriptionEn
            };

            if (!string.IsNullOrWhiteSpace(pick)) return pick;

            // fallback chain
            if (!string.IsNullOrWhiteSpace(t.DescriptionEn)) return t.DescriptionEn;
            if (!string.IsNullOrWhiteSpace(t.DescriptionDe)) return t.DescriptionDe;
            if (!string.IsNullOrWhiteSpace(t.DescriptionPe)) return t.DescriptionPe;
            if (!string.IsNullOrWhiteSpace(t.DescriptionRu)) return t.DescriptionRu;
            if (!string.IsNullOrWhiteSpace(t.DescriptionPo)) return t.DescriptionPo;
            if (!string.IsNullOrWhiteSpace(t.DescriptionAr)) return t.DescriptionAr;

            return string.Empty;
        }
        private static string MiniDescriptionForLang(Tour t, string lang)
        {
            string pick = lang switch
            {
                "de" => t.MiniDescriptionDe,
                "fa" => t.MiniDescriptionPe,
                "ru" => t.MiniDescriptionRu,
                "pl" => t.MiniDescriptionPo,
                "ar" => t.MiniDescriptionAr,
                _ => t.MiniDescriptionEn
            };

            if (!string.IsNullOrWhiteSpace(pick)) return pick;

            // fallback chain
            if (!string.IsNullOrWhiteSpace(t.MiniDescriptionEn)) return t.MiniDescriptionEn;
            if (!string.IsNullOrWhiteSpace(t.MiniDescriptionDe)) return t.MiniDescriptionDe;
            if (!string.IsNullOrWhiteSpace(t.MiniDescriptionPe)) return t.MiniDescriptionPe;
            if (!string.IsNullOrWhiteSpace(t.MiniDescriptionRu)) return t.MiniDescriptionRu;
            if (!string.IsNullOrWhiteSpace(t.MiniDescriptionPo)) return t.MiniDescriptionPo;
            if (!string.IsNullOrWhiteSpace(t.MiniDescriptionAr)) return t.MiniDescriptionAr;

            return string.Empty;
        }

    }
}