using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.DbContexts;
using Models.Entities;
using Models.Helper;

namespace Models.Repository
{
    public class AboutContentRepository : IAboutContentRepository
    {
        private readonly TourDbContext _context;
        private readonly ILogger<AboutContentRepository> _logger;

        public AboutContentRepository(TourDbContext context, ILogger<AboutContentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<AboutContentDto> GetAsync(string language, CancellationToken ct = default)
        {
            var lang = LanguageCatalog.Normalize(language);
            await EnsureSeedAsync(ct);
            var about = await _context.AboutContents.AsNoTracking().FirstOrDefaultAsync(ct);
            if (about is null)
            {
                _logger.LogWarning("About repository: no persisted row found, using defaults.");
            }
            else
            {
                _logger.LogDebug("About repository: retrieved existing row TitleLine1En='{Title}'", about.TitleLine1En);
            }
            var resolved = MergeWithDefaults(about);

            _logger.LogDebug("About repository: returning DTO TitleLine1En='{Title}'", resolved.TitleLine1En);
            return new AboutContentDto(
                ResolveTitleLine1(resolved, lang),
                ResolveTitleLine2(resolved, lang),
                ResolveBody(resolved, lang),
                ResolveButtonText(resolved, lang),
                resolved.ButtonUrl,
                string.IsNullOrWhiteSpace(resolved.ImagePath) ? "/image/about_bg.jpg" : resolved.ImagePath
            );
        }

        public async Task<AboutContentEditDto> GetForEditAsync(CancellationToken ct = default)
        {
            await EnsureSeedAsync(ct);
            var about = await _context.AboutContents.AsNoTracking().FirstOrDefaultAsync(ct);
            if (about is null)
            {
                _logger.LogWarning("About repository: no persisted row found for edit, using defaults.");
            }
            else
            {
                _logger.LogDebug("About repository: retrieved existing row for edit TitleLine1En='{Title}'", about.TitleLine1En);
            }
            var resolved = MergeWithDefaults(about);

            _logger.LogDebug("About repository: returning edit DTO TitleLine1En='{Title}'", resolved.TitleLine1En);
            return new AboutContentEditDto(
                resolved.Id,
                resolved.ImagePath,
                resolved.ButtonUrl,
                resolved.TitleLine1En,
                resolved.TitleLine1De,
                resolved.TitleLine1Tr,
                resolved.TitleLine1Fa,
                resolved.TitleLine1Ru,
                resolved.TitleLine1Pl,
                resolved.TitleLine1Ar,
                resolved.TitleLine2En,
                resolved.TitleLine2De,
                resolved.TitleLine2Tr,
                resolved.TitleLine2Fa,
                resolved.TitleLine2Ru,
                resolved.TitleLine2Pl,
                resolved.TitleLine2Ar,
                resolved.BodyEn,
                resolved.BodyDe,
                resolved.BodyTr,
                resolved.BodyFa,
                resolved.BodyRu,
                resolved.BodyPl,
                resolved.BodyAr,
                resolved.ButtonTextEn,
                resolved.ButtonTextDe,
                resolved.ButtonTextTr,
                resolved.ButtonTextFa,
                resolved.ButtonTextRu,
                resolved.ButtonTextPl,
                resolved.ButtonTextAr
            );
        }

        public async Task UpdateAsync(AboutContentEditDto dto, CancellationToken ct = default)
        {
            var about = await _context.AboutContents.FirstOrDefaultAsync(ct);
            if (about is null)
            {
                about = CreateDefaults();
                if (dto.Id != 0) about.Id = dto.Id;
                _context.AboutContents.Add(about);
            }

            about.ImagePath = dto.ImagePath?.Trim() ?? about.ImagePath;
            about.ButtonUrl = dto.ButtonUrl?.Trim() ?? about.ButtonUrl;

            about.TitleLine1En = dto.TitleLine1En?.Trim() ?? string.Empty;
            about.TitleLine1De = dto.TitleLine1De?.Trim() ?? string.Empty;
            about.TitleLine1Tr = dto.TitleLine1Tr?.Trim() ?? string.Empty;
            about.TitleLine1Fa = dto.TitleLine1Fa?.Trim() ?? string.Empty;
            about.TitleLine1Ru = dto.TitleLine1Ru?.Trim() ?? string.Empty;
            about.TitleLine1Pl = dto.TitleLine1Pl?.Trim() ?? string.Empty;
            about.TitleLine1Ar = dto.TitleLine1Ar?.Trim() ?? string.Empty;

            about.TitleLine2En = dto.TitleLine2En?.Trim() ?? string.Empty;
            about.TitleLine2De = dto.TitleLine2De?.Trim() ?? string.Empty;
            about.TitleLine2Tr = dto.TitleLine2Tr?.Trim() ?? string.Empty;
            about.TitleLine2Fa = dto.TitleLine2Fa?.Trim() ?? string.Empty;
            about.TitleLine2Ru = dto.TitleLine2Ru?.Trim() ?? string.Empty;
            about.TitleLine2Pl = dto.TitleLine2Pl?.Trim() ?? string.Empty;
            about.TitleLine2Ar = dto.TitleLine2Ar?.Trim() ?? string.Empty;

            about.BodyEn = dto.BodyEn?.Trim() ?? string.Empty;
            about.BodyDe = dto.BodyDe?.Trim() ?? string.Empty;
            about.BodyTr = dto.BodyTr?.Trim() ?? string.Empty;
            about.BodyFa = dto.BodyFa?.Trim() ?? string.Empty;
            about.BodyRu = dto.BodyRu?.Trim() ?? string.Empty;
            about.BodyPl = dto.BodyPl?.Trim() ?? string.Empty;
            about.BodyAr = dto.BodyAr?.Trim() ?? string.Empty;

            about.ButtonTextEn = dto.ButtonTextEn?.Trim() ?? string.Empty;
            about.ButtonTextDe = dto.ButtonTextDe?.Trim() ?? string.Empty;
            about.ButtonTextTr = dto.ButtonTextTr?.Trim() ?? string.Empty;
            about.ButtonTextFa = dto.ButtonTextFa?.Trim() ?? string.Empty;
            about.ButtonTextRu = dto.ButtonTextRu?.Trim() ?? string.Empty;
            about.ButtonTextPl = dto.ButtonTextPl?.Trim() ?? string.Empty;
            about.ButtonTextAr = dto.ButtonTextAr?.Trim() ?? string.Empty;

            await _context.SaveChangesAsync(ct);
        }

        private async Task EnsureSeedAsync(CancellationToken ct)
        {
            var about = await _context.AboutContents.FirstOrDefaultAsync(ct);
            if (about is null)
            {
                _logger.LogWarning("About repository: table empty, inserting default about content.");
                _context.AboutContents.Add(CreateDefaults());
                await _context.SaveChangesAsync(ct);
                return;
            }

            if (!string.IsNullOrWhiteSpace(about.TitleLine1En))
            {
                _logger.LogDebug("About repository: content already populated, seeding skipped.");
                return;
            }

            ApplyDefaults(about);
            await _context.SaveChangesAsync(ct);
            _logger.LogInformation("About repository: defaults applied to existing about row.");
        }

        private static string ResolveTitleLine1(AboutContent about, string lang)
        {
            return lang switch
            {
                "de" => Fallback(about.TitleLine1De, about.TitleLine1En),
                "tr" => Fallback(about.TitleLine1Tr, about.TitleLine1En),
                "fa" => Fallback(about.TitleLine1Fa, about.TitleLine1En),
                "ru" => Fallback(about.TitleLine1Ru, about.TitleLine1En),
                "pl" => Fallback(about.TitleLine1Pl, about.TitleLine1En),
                "ar" => Fallback(about.TitleLine1Ar, about.TitleLine1En),
                _ => Fallback(about.TitleLine1En)
            };
        }

        private static string ResolveTitleLine2(AboutContent about, string lang)
        {
            return lang switch
            {
                "de" => Fallback(about.TitleLine2De, about.TitleLine2En),
                "tr" => Fallback(about.TitleLine2Tr, about.TitleLine2En),
                "fa" => Fallback(about.TitleLine2Fa, about.TitleLine2En),
                "ru" => Fallback(about.TitleLine2Ru, about.TitleLine2En),
                "pl" => Fallback(about.TitleLine2Pl, about.TitleLine2En),
                "ar" => Fallback(about.TitleLine2Ar, about.TitleLine2En),
                _ => Fallback(about.TitleLine2En)
            };
        }

        private static string ResolveBody(AboutContent about, string lang)
        {
            return lang switch
            {
                "de" => Fallback(about.BodyDe, about.BodyEn),
                "tr" => Fallback(about.BodyTr, about.BodyEn),
                "fa" => Fallback(about.BodyFa, about.BodyEn),
                "ru" => Fallback(about.BodyRu, about.BodyEn),
                "pl" => Fallback(about.BodyPl, about.BodyEn),
                "ar" => Fallback(about.BodyAr, about.BodyEn),
                _ => Fallback(about.BodyEn)
            };
        }

        private static string ResolveButtonText(AboutContent about, string lang)
        {
            return lang switch
            {
                "de" => Fallback(about.ButtonTextDe, about.ButtonTextEn),
                "tr" => Fallback(about.ButtonTextTr, about.ButtonTextEn),
                "fa" => Fallback(about.ButtonTextFa, about.ButtonTextEn),
                "ru" => Fallback(about.ButtonTextRu, about.ButtonTextEn),
                "pl" => Fallback(about.ButtonTextPl, about.ButtonTextEn),
                "ar" => Fallback(about.ButtonTextAr, about.ButtonTextEn),
                _ => Fallback(about.ButtonTextEn)
            };
        }

        private static AboutContent CreateDefaults()
        {
            return new AboutContent
            {
                Id = 1,
                ImagePath = "/image/about_bg.jpg",
                ButtonUrl = "#contact",
                TitleLine1En = "About Us",
                TitleLine1De = "Über uns",
                TitleLine1Tr = "Hakkımızda",
                TitleLine1Fa = "درباره ما",
                TitleLine1Ru = "О нас",
                TitleLine1Pl = "O nas",
                TitleLine1Ar = "من نحن",
                TitleLine2En = "Our History · Mission & Vision",
                TitleLine2De = "Unsere Geschichte · Mission & Vision",
                TitleLine2Tr = "Hikayemiz · Misyon & Vizyon",
                TitleLine2Fa = "تاریخچه · ماموریت و چشم‌انداز",
                TitleLine2Ru = "Наша история · Миссия и видение",
                TitleLine2Pl = "Nasza historia · Misja i wizja",
                TitleLine2Ar = "تاريخنا · رسالتنا ورؤيتنا",
                BodyEn = "From bespoke Antalya experiences to curated tours across Turkey, we craft journeys that blend comfort with discovery.",
                BodyDe = "Von maßgeschneiderten Antalya-Erlebnissen bis zu kuratierten Türkei-Rundreisen – wir verbinden Komfort mit Entdeckung.",
                BodyTr = "Antalya'daki özel deneyimlerden Türkiye genelindeki seçkin turlara kadar, konforu keşif duygusuyla harmanlayan yolculuklar tasarlıyoruz.",
                BodyFa = "از تجربه‌های اختصاصی آنتالیا تا تورهای خاص سراسر ترکیه، سفری می‌سازیم که راحتی و کشف را در کنار هم قرار می‌دهد.",
                BodyRu = "От индивидуальных впечатлений в Анталии до авторских туров по всей Турции — мы соединяем комфорт и открытие нового.",
                BodyPl = "Od szytych na miarę przeżyć w Antalyi po starannie dobrane wycieczki po Turcji – łączymy komfort z odkrywaniem.",
                BodyAr = "من تجارب أنطاليا المصممة حسب الطلب إلى جولات منسقة في عموم تركيا، نصنع رحلات تجمع الراحة بالاكتشاف.",
                ButtonTextEn = "Request Custom Price",
                ButtonTextDe = "Individuelles Angebot",
                ButtonTextTr = "Özel Fiyat Talebi",
                ButtonTextFa = "درخواست قیمت اختصاصی",
                ButtonTextRu = "Запросить индивидуальную цену",
                ButtonTextPl = "Poproś o wycenę",
                ButtonTextAr = "اطلب عرض سعر خاص",
                CreationTime = 0
            };
        }

        private static void ApplyDefaults(AboutContent target)
        {
            var defaults = CreateDefaults();

            target.ImagePath = defaults.ImagePath;
            target.ButtonUrl = defaults.ButtonUrl;

            target.TitleLine1En = defaults.TitleLine1En;
            target.TitleLine1De = defaults.TitleLine1De;
            target.TitleLine1Tr = defaults.TitleLine1Tr;
            target.TitleLine1Fa = defaults.TitleLine1Fa;
            target.TitleLine1Ru = defaults.TitleLine1Ru;
            target.TitleLine1Pl = defaults.TitleLine1Pl;
            target.TitleLine1Ar = defaults.TitleLine1Ar;

            target.TitleLine2En = defaults.TitleLine2En;
            target.TitleLine2De = defaults.TitleLine2De;
            target.TitleLine2Tr = defaults.TitleLine2Tr;
            target.TitleLine2Fa = defaults.TitleLine2Fa;
            target.TitleLine2Ru = defaults.TitleLine2Ru;
            target.TitleLine2Pl = defaults.TitleLine2Pl;
            target.TitleLine2Ar = defaults.TitleLine2Ar;

            target.BodyEn = defaults.BodyEn;
            target.BodyDe = defaults.BodyDe;
            target.BodyTr = defaults.BodyTr;
            target.BodyFa = defaults.BodyFa;
            target.BodyRu = defaults.BodyRu;
            target.BodyPl = defaults.BodyPl;
            target.BodyAr = defaults.BodyAr;

            target.ButtonTextEn = defaults.ButtonTextEn;
            target.ButtonTextDe = defaults.ButtonTextDe;
            target.ButtonTextTr = defaults.ButtonTextTr;
            target.ButtonTextFa = defaults.ButtonTextFa;
            target.ButtonTextRu = defaults.ButtonTextRu;
            target.ButtonTextPl = defaults.ButtonTextPl;
            target.ButtonTextAr = defaults.ButtonTextAr;
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

        private static AboutContent MergeWithDefaults(AboutContent? about)
        {
            var defaults = CreateDefaults();
            if (about is null)
            {
                return defaults;
            }

            return new AboutContent
            {
                Id = about.Id == 0 ? defaults.Id : about.Id,
                ImagePath = string.IsNullOrWhiteSpace(about.ImagePath) ? defaults.ImagePath : about.ImagePath,
                ButtonUrl = string.IsNullOrWhiteSpace(about.ButtonUrl) ? defaults.ButtonUrl : about.ButtonUrl,
                TitleLine1En = WithFallback(about.TitleLine1En, defaults.TitleLine1En),
                TitleLine1De = WithFallback(about.TitleLine1De, defaults.TitleLine1De),
                TitleLine1Tr = WithFallback(about.TitleLine1Tr, defaults.TitleLine1Tr),
                TitleLine1Fa = WithFallback(about.TitleLine1Fa, defaults.TitleLine1Fa),
                TitleLine1Ru = WithFallback(about.TitleLine1Ru, defaults.TitleLine1Ru),
                TitleLine1Pl = WithFallback(about.TitleLine1Pl, defaults.TitleLine1Pl),
                TitleLine1Ar = WithFallback(about.TitleLine1Ar, defaults.TitleLine1Ar),
                TitleLine2En = WithFallback(about.TitleLine2En, defaults.TitleLine2En),
                TitleLine2De = WithFallback(about.TitleLine2De, defaults.TitleLine2De),
                TitleLine2Tr = WithFallback(about.TitleLine2Tr, defaults.TitleLine2Tr),
                TitleLine2Fa = WithFallback(about.TitleLine2Fa, defaults.TitleLine2Fa),
                TitleLine2Ru = WithFallback(about.TitleLine2Ru, defaults.TitleLine2Ru),
                TitleLine2Pl = WithFallback(about.TitleLine2Pl, defaults.TitleLine2Pl),
                TitleLine2Ar = WithFallback(about.TitleLine2Ar, defaults.TitleLine2Ar),
                BodyEn = WithFallback(about.BodyEn, defaults.BodyEn),
                BodyDe = WithFallback(about.BodyDe, defaults.BodyDe),
                BodyTr = WithFallback(about.BodyTr, defaults.BodyTr),
                BodyFa = WithFallback(about.BodyFa, defaults.BodyFa),
                BodyRu = WithFallback(about.BodyRu, defaults.BodyRu),
                BodyPl = WithFallback(about.BodyPl, defaults.BodyPl),
                BodyAr = WithFallback(about.BodyAr, defaults.BodyAr),
                ButtonTextEn = WithFallback(about.ButtonTextEn, defaults.ButtonTextEn),
                ButtonTextDe = WithFallback(about.ButtonTextDe, defaults.ButtonTextDe),
                ButtonTextTr = WithFallback(about.ButtonTextTr, defaults.ButtonTextTr),
                ButtonTextFa = WithFallback(about.ButtonTextFa, defaults.ButtonTextFa),
                ButtonTextRu = WithFallback(about.ButtonTextRu, defaults.ButtonTextRu),
                ButtonTextPl = WithFallback(about.ButtonTextPl, defaults.ButtonTextPl),
                ButtonTextAr = WithFallback(about.ButtonTextAr, defaults.ButtonTextAr),
                CreationTime = about.CreationTime == 0 ? defaults.CreationTime : about.CreationTime
            };
        }

        private static string WithFallback(string? value, string defaultValue) =>
            string.IsNullOrWhiteSpace(value) ? defaultValue : value;
    }
}
