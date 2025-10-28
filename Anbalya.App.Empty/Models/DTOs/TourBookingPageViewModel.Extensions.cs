using System;
using System.Collections.Generic;
using System.Linq;
using Models.Helper;

public partial class TourBookingPageViewModel
{
    private IReadOnlyList<string>? _galleryCache;
    private IReadOnlyList<string>? _activeDaysCache;

    public IReadOnlyList<string> Gallery => _galleryCache ??= BuildGallery();

    public IReadOnlyList<string> ActiveDayNames => _activeDaysCache ??= BuildActiveDayNames();

    public string HeroPhotoPath => Gallery.FirstOrDefault() ?? "~/image/about_banner.jpg";

    public bool HasCoordinates => Tour is not null && (Math.Abs(Tour.LocLat) > 0.001 || Math.Abs(Tour.LocLon) > 0.001);

    public bool HasAdditionalPhotos => Gallery.Count > 1;

    public string LanguageLabel => string.IsNullOrWhiteSpace(Tour.LanguageUsed)
        ? "EN"
        : Tour.LanguageUsed.ToUpperInvariant();

    public int EstimatedTotal =>
        Math.Max(0, Form?.Adults ?? 0) * Tour.Price
        + Math.Max(0, Form?.Children ?? 0) * Tour.KinderPrice
        + Math.Max(0, Form?.Infants ?? 0) * Tour.InfantPrice;

    private IReadOnlyList<string> BuildGallery()
    {
        var results = new List<string>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        void Add(string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            var trimmed = path.Trim();
            if (seen.Add(trimmed))
            {
                results.Add(trimmed);
            }
        }

        Add(Tour.Foto);

        if (Tour.Fotos is not null)
        {
            foreach (var foto in Tour.Fotos)
            {
                Add(foto);
            }
        }

        return results;
    }

    private IReadOnlyList<string> BuildActiveDayNames()
    {
        if (Tour is null) return Array.Empty<string>();

        return DayMaskHelper
            .ToSelectedDays(Tour.ActiveDay)
            .Select(index => DayMaskHelper.Options.FirstOrDefault(o => o.Index == index)?.DisplayName ?? $"Day {index + 1}")
            .ToList();
    }
}
