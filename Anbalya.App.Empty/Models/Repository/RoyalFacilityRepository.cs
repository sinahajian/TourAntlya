using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.DbContexts;
using Models.Entities;
using Models.Helper;

namespace Models.Repository
{
    public class RoyalFacilityRepository : IRoyalFacilityRepository
    {
        private readonly TourDbContext _context;
        private readonly ILogger<RoyalFacilityRepository> _logger;

        public RoyalFacilityRepository(TourDbContext context, ILogger<RoyalFacilityRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IReadOnlyList<RoyalFacilityDto>> ListAsync(string language, CancellationToken ct = default)
        {
            var lang = LanguageCatalog.Normalize(language);

            await EnsureSeedAsync(ct);

            var facilities = await _context.RoyalFacilities
                .AsNoTracking()
                .OrderBy(f => f.DisplayOrder)
                .ThenBy(f => f.Id)
                .ToListAsync(ct);

            if (facilities.Count == 0)
            {
                _logger.LogWarning("Facilities repository: no data in table, returning default seed set.");
                return CreateDefaults()
                    .Select(f => ToDisplayDto(f, lang))
                    .ToList();
            }

            var defaultsMap = CreateDefaults().ToDictionary(f => f.Id);

            var mergedDisplay = facilities
                .Select(f =>
                {
                    var resolved = MergeWithDefaults(f, defaultsMap);
                    return ToDisplayDto(resolved, lang);
                })
                .ToList();
            _logger.LogDebug("Facilities repository: returning {Count} display DTOs. First TitleEn='{Title}'", mergedDisplay.Count, mergedDisplay.FirstOrDefault()?.Title);
            return mergedDisplay;
        }

        public async Task<IReadOnlyList<RoyalFacilityEditDto>> ListForEditAsync(CancellationToken ct = default)
        {
            await EnsureSeedAsync(ct);

            var facilities = await _context.RoyalFacilities
                .AsNoTracking()
                .OrderBy(f => f.DisplayOrder)
                .ThenBy(f => f.Id)
                .ToListAsync(ct);

            if (facilities.Count == 0)
            {
                return CreateDefaults()
                    .Select(ToEditDto)
                    .ToList();
            }

            var defaultsMap = CreateDefaults().ToDictionary(f => f.Id);

            var mergedEdit = facilities
                .Select(f => ToEditDto(MergeWithDefaults(f, defaultsMap)))
                .ToList();
            _logger.LogDebug("Facilities repository: returning {Count} edit DTOs. First TitleEn='{Title}'", mergedEdit.Count, mergedEdit.FirstOrDefault()?.TitleEn);
            return mergedEdit;
        }

        public async Task UpdateAsync(IEnumerable<RoyalFacilityEditDto> dtos, CancellationToken ct = default)
        {
            if (dtos is null)
            {
                _logger.LogWarning("Facilities repository: UpdateAsync called with null DTO list.");
                return;
            }

            var list = dtos.ToList();
            if (list.Count == 0)
            {
                _logger.LogWarning("Facilities repository: UpdateAsync called with empty list.");
                return;
            }

            await EnsureSeedAsync(ct);

            var ids = list.Select(d => d.Id).ToList();
            _logger.LogInformation("Facilities repository: incoming IDs {Ids}", string.Join(",", ids));
            var facilities = await _context.RoyalFacilities
                .Where(f => ids.Contains(f.Id))
                .ToListAsync(ct);

            _logger.LogInformation("Facilities repository: updating {Count} facilities.", facilities.Count);
            foreach (var dto in list)
            {
                var facility = facilities.FirstOrDefault(f => f.Id == dto.Id);
                if (facility is null) continue;

                facility.IconClass = dto.IconClass?.Trim() ?? facility.IconClass;
                facility.DisplayOrder = dto.DisplayOrder;
                facility.TitleEn = dto.TitleEn?.Trim() ?? string.Empty;
                facility.TitleDe = dto.TitleDe?.Trim() ?? string.Empty;
                facility.TitleTr = dto.TitleTr?.Trim() ?? string.Empty;
                facility.TitleFa = dto.TitleFa?.Trim() ?? string.Empty;
                facility.TitleRu = dto.TitleRu?.Trim() ?? string.Empty;
                facility.TitlePl = dto.TitlePl?.Trim() ?? string.Empty;
                facility.TitleAr = dto.TitleAr?.Trim() ?? string.Empty;
                facility.DescriptionEn = dto.DescriptionEn?.Trim() ?? string.Empty;
                facility.DescriptionDe = dto.DescriptionDe?.Trim() ?? string.Empty;
                facility.DescriptionTr = dto.DescriptionTr?.Trim() ?? string.Empty;
                facility.DescriptionFa = dto.DescriptionFa?.Trim() ?? string.Empty;
                facility.DescriptionRu = dto.DescriptionRu?.Trim() ?? string.Empty;
                facility.DescriptionPl = dto.DescriptionPl?.Trim() ?? string.Empty;
                facility.DescriptionAr = dto.DescriptionAr?.Trim() ?? string.Empty;
            }

            await _context.SaveChangesAsync(ct);
            _logger.LogInformation("Facilities repository: update saved.");
        }

        private async Task EnsureSeedAsync(CancellationToken ct)
        {
            var facilities = await _context.RoyalFacilities
                .OrderBy(f => f.DisplayOrder)
                .ThenBy(f => f.Id)
                .ToListAsync(ct);

            if (facilities.Count == 0)
            {
                _logger.LogWarning("Facilities repository: table empty, inserting default facilities.");
                var defaults = CreateDefaults();
                _context.RoyalFacilities.AddRange(defaults);
                await _context.SaveChangesAsync(ct);
                return;
            }

            var needsRefresh = facilities.All(f =>
                string.IsNullOrWhiteSpace(f.TitleEn) &&
                string.IsNullOrWhiteSpace(f.DescriptionEn));

            if (!needsRefresh)
            {
                _logger.LogDebug("Facilities repository: table already contains data, seeding skipped.");
                return;
            }

            _logger.LogWarning("Facilities repository: existing rows are blank, applying defaults.");
            var defaultsMap = CreateDefaults().ToDictionary(f => f.Id);
            foreach (var facility in facilities)
            {
                if (!defaultsMap.TryGetValue(facility.Id, out var defaults))
                {
                    continue;
                }

                facility.IconClass = defaults.IconClass;
                facility.DisplayOrder = defaults.DisplayOrder;
                facility.TitleEn = defaults.TitleEn;
                facility.TitleDe = defaults.TitleDe;
                facility.TitleTr = defaults.TitleTr;
                facility.TitleFa = defaults.TitleFa;
                facility.TitleRu = defaults.TitleRu;
                facility.TitlePl = defaults.TitlePl;
                facility.TitleAr = defaults.TitleAr;
                facility.DescriptionEn = defaults.DescriptionEn;
                facility.DescriptionDe = defaults.DescriptionDe;
                facility.DescriptionTr = defaults.DescriptionTr;
                facility.DescriptionFa = defaults.DescriptionFa;
                facility.DescriptionRu = defaults.DescriptionRu;
                facility.DescriptionPl = defaults.DescriptionPl;
                facility.DescriptionAr = defaults.DescriptionAr;
            }

            await _context.SaveChangesAsync(ct);
            _logger.LogInformation("Facilities repository: defaults applied to existing rows.");
        }

        private static List<RoyalFacility> CreateDefaults()
        {
            var timestamp = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero).ToUnixTimeSeconds();

            return new List<RoyalFacility>
            {
                new RoyalFacility
                {
                    Id = 1,
                    IconClass = "lnr lnr-bus",
                    DisplayOrder = 1,
                    TitleEn = "Hotel Pick-Up & Drop-Off",
                    TitleDe = "Hotelabholung & Rücktransfer",
                    TitleTr = "Otelden Alım & Bırakma",
                    TitleFa = "ترنسفر رفت و برگشت هتل",
                    TitleRu = "Трансфер из отеля и обратно",
                    TitlePl = "Transfer z hotelu w cenie",
                    TitleAr = "خدمة نقل من وإلى الفندق",
                    DescriptionEn = "Complimentary transfer from your Antalya hotel to the Kemer yacht harbor and back.",
                    DescriptionDe = "Kostenloser Transfer von Ihrem Hotel in Antalya zum Hafen von Kemer und zurück.",
                    DescriptionTr = "Antalya'daki otelinizden Kemer limanına ve dönüşte tekrar otele ücretsiz transfer.",
                    DescriptionFa = "حمل‌ونقل رایگان از هتل شما در آنتالیا تا بندر یات کمر و بالعکس.",
                    DescriptionRu = "Бесплатный трансфер из вашего отеля в Анталье до яхтенной марины Кемера и обратно.",
                    DescriptionPl = "Bezpłatny dojazd z hotelu w Antalyi do mariny w Kemer i z powrotem.",
                    DescriptionAr = "نقل مجاني من فندقك في أنطاليا إلى مرسى كيمر لليخوت والعودة مرة أخرى.",
                    CreationTime = timestamp
                },
                new RoyalFacility
                {
                    Id = 2,
                    IconClass = "lnr lnr-dinner",
                    DisplayOrder = 2,
                    TitleEn = "Fresh Lunch on Board",
                    TitleDe = "Frisches Mittagessen an Bord",
                    TitleTr = "Teknede Sıcak Öğle Yemeği",
                    TitleFa = "ناهار تازه روی عرشه",
                    TitleRu = "Свежий обед на борту",
                    TitlePl = "Świeży obiad na pokładzie",
                    TitleAr = "غداء طازج على متن القارب",
                    DescriptionEn = "Grilled chicken, salads, and seasonal fruit served during the cruise.",
                    DescriptionDe = "Gegrilltes Hähnchen, Salate und saisonales Obst während der Kreuzfahrt.",
                    DescriptionTr = "Tur boyunca ızgara tavuk, salatalar ve mevsim meyveleri servis edilir.",
                    DescriptionFa = "در طول کروز مرغ کبابی، سالاد و میوه فصل سرو می‌شود.",
                    DescriptionRu = "Во время круиза подают жареную курицу, салаты и сезонные фрукты.",
                    DescriptionPl = "Podczas rejsu serwujemy grillowanego kurczaka, sałatki i sezonowe owoce.",
                    DescriptionAr = "يُقدَّم خلال الرحلة دجاج مشوي وسلطات وفواكه موسمية.",
                    CreationTime = timestamp
                },
                new RoyalFacility
                {
                    Id = 3,
                    IconClass = "lnr lnr-coffee-cup",
                    DisplayOrder = 3,
                    TitleEn = "Unlimited Soft Drinks",
                    TitleDe = "Unbegrenzte Softdrinks",
                    TitleTr = "Sınırsız Alkolsüz İçecekler",
                    TitleFa = "نوشیدنی نامحدود",
                    TitleRu = "Неограниченные напитки",
                    TitlePl = "Nielimitowane napoje",
                    TitleAr = "مشروبات بلا حدود",
                    DescriptionEn = "Tea, coffee, and refreshing soft drinks available throughout the day.",
                    DescriptionDe = "Tee, Kaffee und erfrischende Softdrinks stehen den ganzen Tag bereit.",
                    DescriptionTr = "Gün boyu çay, kahve ve ferahlatıcı içecekler sunulur.",
                    DescriptionFa = "چای، قهوه و نوشیدنی‌های خنک تمام روز در دسترس است.",
                    DescriptionRu = "Чай, кофе и освежающие безалкогольные напитки доступны весь день.",
                    DescriptionPl = "Herbata, kawa i orzeźwiające napoje bezalkoholowe dostępne przez cały dzień.",
                    DescriptionAr = "شاي وقهوة ومشروبات منعشة متاحة طوال اليوم.",
                    CreationTime = timestamp
                },
                new RoyalFacility
                {
                    Id = 4,
                    IconClass = "lnr lnr-sun",
                    DisplayOrder = 4,
                    TitleEn = "Paradise Bays Swim Stops",
                    TitleDe = "Badestopps in Paradiesbuchten",
                    TitleTr = "Cennet Koylarında Yüzme Molaları",
                    TitleFa = "توقف برای شنا در خلیج‌های بهشتی",
                    TitleRu = "Купание в райских бухтах",
                    TitlePl = "Przystanki na kąpiel w rajskich zatokach",
                    TitleAr = "توقفات سباحة في خلجان الجنة",
                    DescriptionEn = "Dive into crystal bays like Phaselis Island and Cennet Cove with 45-minute breaks.",
                    DescriptionDe = "Schwimmen in kristallklaren Buchten wie Phaselis Island und Cennet Cove mit 45-Minuten-Pausen.",
                    DescriptionTr = "Phaselis Adası ve Cennet Koyu gibi turkuaz koylarda 45 dakikalık yüzme molaları.",
                    DescriptionFa = "با توقف‌های ۴۵ دقیقه‌ای در آب‌های شفاف جزیره فاسلیس و خلیج جنّت شنا کنید.",
                    DescriptionRu = "Купание в прозрачных водах бухт Фазелис и Дженнет с остановками по 45 минут.",
                    DescriptionPl = "45-minutowe postoje na kąpiel w krystalicznych wodach Phaselis i Cennet.",
                    DescriptionAr = "اسبح في مياه فيروزية عند جزيرة فاسيليس وخليج جنّت مع توقفات لمدة 45 دقيقة.",
                    CreationTime = timestamp
                },
                new RoyalFacility
                {
                    Id = 5,
                    IconClass = "lnr lnr-users",
                    DisplayOrder = 5,
                    TitleEn = "Live Multilingual Guide",
                    TitleDe = "Live-Guide in mehreren Sprachen",
                    TitleTr = "Canlı Çok Dilli Rehber",
                    TitleFa = "راهنمای زنده چندزبانه",
                    TitleRu = "Живой многоязычный гид",
                    TitlePl = "Żywy przewodnik wielojęzyczny",
                    TitleAr = "مرشد حي متعدد اللغات",
                    DescriptionEn = "Live English, French, German, Polish, Russian speaking guide shares stories and safety tips.",
                    DescriptionDe = "Moderation auf Englisch, Französisch, Deutsch, Polnisch und Russisch mit Geschichten und Sicherheitstipps.",
                    DescriptionTr = "İngilizce, Fransızca, Almanca, Lehçe ve Rusça konuşan rehber hikayeler ve güvenlik bilgileri paylaşır.",
                    DescriptionFa = "میزبان انگلیسی، فرانسوی، آلمانی، لهستانی و روسی داستان‌ها و نکات ایمنی را بیان می‌کند.",
                    DescriptionRu = "Гид на английском, французском, немецком, польском и русском делится историями и советами по безопасности.",
                    DescriptionPl = "Prowadzący mówi po angielsku, francusku, niemiecku, polsku i rosyjsku, dzieląc się historiami i zasadami bezpieczeństwa.",
                    DescriptionAr = "مرشد يتحدث الإنجليزية والفرنسية والألمانية والبولندية والروسية يشارك القصص وإرشادات السلامة.",
                    CreationTime = timestamp
                },
                new RoyalFacility
                {
                    Id = 6,
                    IconClass = "lnr lnr-music-note",
                    DisplayOrder = 6,
                    TitleEn = "Sun Deck Loungers & Music",
                    TitleDe = "Sonnendeck-Liegen & Musik",
                    TitleTr = "Güneşlenme Güvertesi ve Müzik",
                    TitleFa = "آفتاب‌گرفتن و موسیقی روی عرشه",
                    TitleRu = "Солярий и музыка на палубе",
                    TitlePl = "Leżaki słoneczne i muzyka",
                    TitleAr = "سطح شمسي وموسيقى",
                    DescriptionEn = "Relax on upper-deck sunbeds with chill-out music and an optional foam party.",
                    DescriptionDe = "Entspannen Sie auf dem Oberdeck mit Liegen, Chill-out-Musik und optionaler Schaumparty.",
                    DescriptionTr = "Üst güvertede şezlonglarda dinlenin, chill-out müzik ve isteğe bağlı köpük partisiyle eğlenin.",
                    DescriptionFa = "روی صندلی‌های آفتاب‌گیر عرشه بالا استراحت کنید و از موسیقی و فوم‌پارٹی اختیاری لذت ببرید.",
                    DescriptionRu = "Отдыхайте на лежаках верхней палубы под расслабляющую музыку и пенную вечеринку по желанию.",
                    DescriptionPl = "Relaks na górnym pokładzie z leżakami, chilloutową muzyką i opcjonalną imprezą pianową.",
                    DescriptionAr = "استرخِ على أسرة التشمس في السطح العلوي مع موسيقى هادئة وحفلة رغوة اختيارية.",
                    CreationTime = timestamp
                }
            };
        }

        private static RoyalFacility MergeWithDefaults(RoyalFacility facility, IDictionary<int, RoyalFacility> defaultsMap)
        {
            if (!defaultsMap.TryGetValue(facility.Id, out var defaults))
            {
                defaults = CreateDefaults().First();
            }

            return new RoyalFacility
            {
                Id = facility.Id,
                IconClass = string.IsNullOrWhiteSpace(facility.IconClass) ? defaults.IconClass : facility.IconClass,
                DisplayOrder = facility.DisplayOrder == 0 ? defaults.DisplayOrder : facility.DisplayOrder,
                TitleEn = string.IsNullOrWhiteSpace(facility.TitleEn) ? defaults.TitleEn : facility.TitleEn,
                TitleDe = string.IsNullOrWhiteSpace(facility.TitleDe) ? defaults.TitleDe : facility.TitleDe,
                TitleTr = string.IsNullOrWhiteSpace(facility.TitleTr) ? defaults.TitleTr : facility.TitleTr,
                TitleFa = string.IsNullOrWhiteSpace(facility.TitleFa) ? defaults.TitleFa : facility.TitleFa,
                TitleRu = string.IsNullOrWhiteSpace(facility.TitleRu) ? defaults.TitleRu : facility.TitleRu,
                TitlePl = string.IsNullOrWhiteSpace(facility.TitlePl) ? defaults.TitlePl : facility.TitlePl,
                TitleAr = string.IsNullOrWhiteSpace(facility.TitleAr) ? defaults.TitleAr : facility.TitleAr,
                DescriptionEn = string.IsNullOrWhiteSpace(facility.DescriptionEn) ? defaults.DescriptionEn : facility.DescriptionEn,
                DescriptionDe = string.IsNullOrWhiteSpace(facility.DescriptionDe) ? defaults.DescriptionDe : facility.DescriptionDe,
                DescriptionTr = string.IsNullOrWhiteSpace(facility.DescriptionTr) ? defaults.DescriptionTr : facility.DescriptionTr,
                DescriptionFa = string.IsNullOrWhiteSpace(facility.DescriptionFa) ? defaults.DescriptionFa : facility.DescriptionFa,
                DescriptionRu = string.IsNullOrWhiteSpace(facility.DescriptionRu) ? defaults.DescriptionRu : facility.DescriptionRu,
                DescriptionPl = string.IsNullOrWhiteSpace(facility.DescriptionPl) ? defaults.DescriptionPl : facility.DescriptionPl,
                DescriptionAr = string.IsNullOrWhiteSpace(facility.DescriptionAr) ? defaults.DescriptionAr : facility.DescriptionAr,
                CreationTime = facility.CreationTime == 0 ? defaults.CreationTime : facility.CreationTime
            };
        }

        private static RoyalFacilityDto ToDisplayDto(RoyalFacility facility, string language)
        {
            return new RoyalFacilityDto(
                facility.Id,
                facility.IconClass,
                facility.DisplayOrder,
                ResolveTitle(facility, language),
                ResolveDescription(facility, language));
        }

        private static RoyalFacilityEditDto ToEditDto(RoyalFacility facility)
        {
            return new RoyalFacilityEditDto(
                facility.Id,
                facility.IconClass,
                facility.DisplayOrder,
                facility.TitleEn,
                facility.TitleDe,
                facility.TitleTr,
                facility.TitleFa,
                facility.TitleRu,
                facility.TitlePl,
                facility.TitleAr,
                facility.DescriptionEn,
                facility.DescriptionDe,
                facility.DescriptionTr,
                facility.DescriptionFa,
                facility.DescriptionRu,
                facility.DescriptionPl,
                facility.DescriptionAr);
        }

        private static string ResolveTitle(RoyalFacility facility, string lang)
        {
            return lang switch
            {
                "de" => Fallback(facility.TitleDe, facility.TitleEn),
                "tr" => Fallback(facility.TitleTr, facility.TitleEn),
                "fa" => Fallback(facility.TitleFa, facility.TitleEn),
                "ru" => Fallback(facility.TitleRu, facility.TitleEn),
                "pl" => Fallback(facility.TitlePl, facility.TitleEn),
                "ar" => Fallback(facility.TitleAr, facility.TitleEn),
                _ => Fallback(facility.TitleEn)
            };
        }

        private static string ResolveDescription(RoyalFacility facility, string lang)
        {
            return lang switch
            {
                "de" => Fallback(facility.DescriptionDe, facility.DescriptionEn),
                "tr" => Fallback(facility.DescriptionTr, facility.DescriptionEn),
                "fa" => Fallback(facility.DescriptionFa, facility.DescriptionEn),
                "ru" => Fallback(facility.DescriptionRu, facility.DescriptionEn),
                "pl" => Fallback(facility.DescriptionPl, facility.DescriptionEn),
                "ar" => Fallback(facility.DescriptionAr, facility.DescriptionEn),
                _ => Fallback(facility.DescriptionEn)
            };
        }

        private static string Fallback(params string?[] values)
        {
            foreach (var value in values)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value.Trim();
                }
            }

            return string.Empty;
        }
    }
}
