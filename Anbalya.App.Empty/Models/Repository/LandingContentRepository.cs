using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.DbContexts;
using Models.Entities;
using Models.Helper;

namespace Models.Repository
{
    public class LandingContentRepository : ILandingContentRepository
    {
        private readonly TourDbContext _context;

        public LandingContentRepository(TourDbContext context)
        {
            _context = context;
        }

        public async Task<LandingContentDto> GetAsync(string language, CancellationToken ct = default)
        {
            var normalized = LanguageCatalog.Normalize(language);

            var records = await _context.LandingContents
                .AsNoTracking()
                .ToListAsync(ct);

            var composite = records.Count == 0
                ? CreateDefault()
                : Compose(records);

            return Map(composite, normalized);
        }

        public Task UpdateAsync(LandingContentDto dto, CancellationToken ct = default)
        {
            return UpdateAllAsync(new[] { dto }, ct);
        }

        public async Task UpdateAllAsync(IEnumerable<LandingContentDto> dtos, CancellationToken ct = default)
        {
            if (dtos is null) return;

            var prepared = dtos
                .Select(d => new LandingContentDto(
                    LanguageCatalog.Normalize(d.Language),
                    (d.Tagline ?? string.Empty).Trim(),
                    (d.Title ?? string.Empty).Trim(),
                    (d.Description ?? string.Empty).Trim(),
                    string.IsNullOrWhiteSpace(d.BackgroundImage) ? null : d.BackgroundImage.Trim()))
                .GroupBy(d => d.Language, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.Last())
                .ToList();

            if (prepared.Count == 0) return;

            var backgroundImage = prepared
                .Select(d => d.BackgroundImage)
                .FirstOrDefault(); // same for all entries coming from the form

            var all = await _context.LandingContents.ToListAsync(ct);
            var primary = all.FirstOrDefault(c => LanguageCatalog.Normalize(c.Language) == "en")
                ?? all.FirstOrDefault();

            if (primary is null)
            {
                primary = CreateDefault();
                primary.Language = "en";
                primary.CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                await _context.LandingContents.AddAsync(primary, ct);
                all.Add(primary);
            }

            foreach (var dto in prepared)
            {
                ApplyTranslation(primary, dto.Language, dto.Tagline, dto.Title, dto.Description);

                var legacy = all.FirstOrDefault(c =>
                    c.Id != primary.Id && LanguageCatalog.Normalize(c.Language) == dto.Language);

                if (legacy is not null)
                {
                    legacy.Tagline = dto.Tagline;
                    legacy.Title = dto.Title;
                    legacy.Description = dto.Description;
                    legacy.BackgroundImage = backgroundImage;

                    if (legacy.CreationTime == 0)
                    {
                        legacy.CreationTime = primary.CreationTime;
                    }
                }
            }

            if (primary.CreationTime == 0)
            {
                primary.CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            }

            primary.BackgroundImage = backgroundImage;

            await _context.SaveChangesAsync(ct);
        }

        public async Task<IReadOnlyList<LandingContentDto>> ListAllAsync(CancellationToken ct = default)
        {
            var records = await _context.LandingContents
                .AsNoTracking()
                .ToListAsync(ct);

            var composite = records.Count == 0
                ? CreateDefault()
                : Compose(records);

            return LanguageCatalog.Options
                .Select(option => Map(composite, option.Code))
                .ToList();
        }

        private static LandingContentDto Map(LandingContent entity, string language)
        {
            var normalized = LanguageCatalog.Normalize(language);

            string tagline = normalized switch
            {
                "de" => entity.TaglineDe,
                "tr" => entity.TaglineTr,
                "fa" => entity.TaglineFa,
                "ru" => entity.TaglineRu,
                "pl" => entity.TaglinePl,
                "ar" => entity.TaglineAr,
                _ => entity.TaglineEn
            };

            string title = normalized switch
            {
                "de" => entity.TitleDe,
                "tr" => entity.TitleTr,
                "fa" => entity.TitleFa,
                "ru" => entity.TitleRu,
                "pl" => entity.TitlePl,
                "ar" => entity.TitleAr,
                _ => entity.TitleEn
            };

            string description = normalized switch
            {
                "de" => entity.DescriptionDe,
                "tr" => entity.DescriptionTr,
                "fa" => entity.DescriptionFa,
                "ru" => entity.DescriptionRu,
                "pl" => entity.DescriptionPl,
                "ar" => entity.DescriptionAr,
                _ => entity.DescriptionEn
            };

            return new LandingContentDto(
                normalized,
                Resolve(tagline,
                    entity.TaglineEn,
                    entity.Tagline,
                    entity.TaglineDe,
                    entity.TaglineTr,
                    entity.TaglineFa,
                    entity.TaglineRu,
                    entity.TaglinePl,
                    entity.TaglineAr),
                Resolve(title,
                    entity.TitleEn,
                    entity.Title,
                    entity.TitleDe,
                    entity.TitleTr,
                    entity.TitleFa,
                    entity.TitleRu,
                    entity.TitlePl,
                    entity.TitleAr),
                Resolve(description,
                    entity.DescriptionEn,
                    entity.Description,
                    entity.DescriptionDe,
                    entity.DescriptionTr,
                    entity.DescriptionFa,
                    entity.DescriptionRu,
                    entity.DescriptionPl,
                    entity.DescriptionAr),
                entity.BackgroundImage
            );
        }

        private static LandingContent Compose(IReadOnlyList<LandingContent> records)
        {
            var composite = new LandingContent
            {
                BackgroundImage = records
                    .Select(r => r.BackgroundImage)
                    .FirstOrDefault(b => !string.IsNullOrWhiteSpace(b)),
                CreationTime = records
                    .Select(r => r.CreationTime)
                    .Where(t => t > 0)
                    .DefaultIfEmpty(0)
                    .Min()
            };

            foreach (var record in records)
            {
                composite.TaglineEn = Assign(composite.TaglineEn, record.TaglineEn);
                composite.TaglineDe = Assign(composite.TaglineDe, record.TaglineDe);
                composite.TaglineTr = Assign(composite.TaglineTr, record.TaglineTr);
                composite.TaglineFa = Assign(composite.TaglineFa, record.TaglineFa);
                composite.TaglineRu = Assign(composite.TaglineRu, record.TaglineRu);
                composite.TaglinePl = Assign(composite.TaglinePl, record.TaglinePl);
                composite.TaglineAr = Assign(composite.TaglineAr, record.TaglineAr);

                composite.TitleEn = Assign(composite.TitleEn, record.TitleEn);
                composite.TitleDe = Assign(composite.TitleDe, record.TitleDe);
                composite.TitleTr = Assign(composite.TitleTr, record.TitleTr);
                composite.TitleFa = Assign(composite.TitleFa, record.TitleFa);
                composite.TitleRu = Assign(composite.TitleRu, record.TitleRu);
                composite.TitlePl = Assign(composite.TitlePl, record.TitlePl);
                composite.TitleAr = Assign(composite.TitleAr, record.TitleAr);

                composite.DescriptionEn = Assign(composite.DescriptionEn, record.DescriptionEn);
                composite.DescriptionDe = Assign(composite.DescriptionDe, record.DescriptionDe);
                composite.DescriptionTr = Assign(composite.DescriptionTr, record.DescriptionTr);
                composite.DescriptionFa = Assign(composite.DescriptionFa, record.DescriptionFa);
                composite.DescriptionRu = Assign(composite.DescriptionRu, record.DescriptionRu);
                composite.DescriptionPl = Assign(composite.DescriptionPl, record.DescriptionPl);
                composite.DescriptionAr = Assign(composite.DescriptionAr, record.DescriptionAr);

                var normalized = LanguageCatalog.Normalize(record.Language);
                switch (normalized)
                {
                    case "de":
                        composite.TaglineDe = Assign(composite.TaglineDe, record.Tagline);
                        composite.TitleDe = Assign(composite.TitleDe, record.Title);
                        composite.DescriptionDe = Assign(composite.DescriptionDe, record.Description);
                        break;
                    case "tr":
                        composite.TaglineTr = Assign(composite.TaglineTr, record.Tagline);
                        composite.TitleTr = Assign(composite.TitleTr, record.Title);
                        composite.DescriptionTr = Assign(composite.DescriptionTr, record.Description);
                        break;
                    case "fa":
                        composite.TaglineFa = Assign(composite.TaglineFa, record.Tagline);
                        composite.TitleFa = Assign(composite.TitleFa, record.Title);
                        composite.DescriptionFa = Assign(composite.DescriptionFa, record.Description);
                        break;
                    case "ru":
                        composite.TaglineRu = Assign(composite.TaglineRu, record.Tagline);
                        composite.TitleRu = Assign(composite.TitleRu, record.Title);
                        composite.DescriptionRu = Assign(composite.DescriptionRu, record.Description);
                        break;
                    case "pl":
                        composite.TaglinePl = Assign(composite.TaglinePl, record.Tagline);
                        composite.TitlePl = Assign(composite.TitlePl, record.Title);
                        composite.DescriptionPl = Assign(composite.DescriptionPl, record.Description);
                        break;
                    case "ar":
                        composite.TaglineAr = Assign(composite.TaglineAr, record.Tagline);
                        composite.TitleAr = Assign(composite.TitleAr, record.Title);
                        composite.DescriptionAr = Assign(composite.DescriptionAr, record.Description);
                        break;
                    default:
                        composite.TaglineEn = Assign(composite.TaglineEn, record.Tagline);
                        composite.TitleEn = Assign(composite.TitleEn, record.Title);
                        composite.DescriptionEn = Assign(composite.DescriptionEn, record.Description);
                        break;
                }
            }

            var defaults = CreateDefault();

            composite.TaglineEn = Resolve(composite.TaglineEn, defaults.TaglineEn);
            composite.TitleEn = Resolve(composite.TitleEn, defaults.TitleEn);
            composite.DescriptionEn = Resolve(composite.DescriptionEn, defaults.DescriptionEn);

            composite.TaglineDe = Resolve(composite.TaglineDe, defaults.TaglineDe, composite.TaglineEn);
            composite.TitleDe = Resolve(composite.TitleDe, defaults.TitleDe, composite.TitleEn);
            composite.DescriptionDe = Resolve(composite.DescriptionDe, defaults.DescriptionDe, composite.DescriptionEn);

            composite.TaglineTr = Resolve(composite.TaglineTr, defaults.TaglineTr, composite.TaglineEn);
            composite.TitleTr = Resolve(composite.TitleTr, defaults.TitleTr, composite.TitleEn);
            composite.DescriptionTr = Resolve(composite.DescriptionTr, defaults.DescriptionTr, composite.DescriptionEn);

            composite.TaglineFa = Resolve(composite.TaglineFa, defaults.TaglineFa, composite.TaglineEn);
            composite.TitleFa = Resolve(composite.TitleFa, defaults.TitleFa, composite.TitleEn);
            composite.DescriptionFa = Resolve(composite.DescriptionFa, defaults.DescriptionFa, composite.DescriptionEn);

            composite.TaglineRu = Resolve(composite.TaglineRu, defaults.TaglineRu, composite.TaglineEn);
            composite.TitleRu = Resolve(composite.TitleRu, defaults.TitleRu, composite.TitleEn);
            composite.DescriptionRu = Resolve(composite.DescriptionRu, defaults.DescriptionRu, composite.DescriptionEn);

            composite.TaglinePl = Resolve(composite.TaglinePl, defaults.TaglinePl, composite.TaglineEn);
            composite.TitlePl = Resolve(composite.TitlePl, defaults.TitlePl, composite.TitleEn);
            composite.DescriptionPl = Resolve(composite.DescriptionPl, defaults.DescriptionPl, composite.DescriptionEn);

            composite.TaglineAr = Resolve(composite.TaglineAr, defaults.TaglineAr, composite.TaglineEn);
            composite.TitleAr = Resolve(composite.TitleAr, defaults.TitleAr, composite.TitleEn);
            composite.DescriptionAr = Resolve(composite.DescriptionAr, defaults.DescriptionAr, composite.DescriptionEn);

            composite.Tagline = Resolve(composite.TaglineEn, defaults.Tagline);
            composite.Title = Resolve(composite.TitleEn, defaults.Title);
            composite.Description = Resolve(composite.DescriptionEn, defaults.Description);
            composite.Language = "en";
            composite.BackgroundImage ??= defaults.BackgroundImage;

            return composite;
        }

        private static void ApplyTranslation(LandingContent entity, string language, string tagline, string title, string description)
        {
            switch (language)
            {
                case "de":
                    entity.TaglineDe = tagline;
                    entity.TitleDe = title;
                    entity.DescriptionDe = description;
                    break;
                case "tr":
                    entity.TaglineTr = tagline;
                    entity.TitleTr = title;
                    entity.DescriptionTr = description;
                    break;
                case "fa":
                    entity.TaglineFa = tagline;
                    entity.TitleFa = title;
                    entity.DescriptionFa = description;
                    break;
                case "ru":
                    entity.TaglineRu = tagline;
                    entity.TitleRu = title;
                    entity.DescriptionRu = description;
                    break;
                case "pl":
                    entity.TaglinePl = tagline;
                    entity.TitlePl = title;
                    entity.DescriptionPl = description;
                    break;
                case "ar":
                    entity.TaglineAr = tagline;
                    entity.TitleAr = title;
                    entity.DescriptionAr = description;
                    break;
                default:
                    entity.TaglineEn = tagline;
                    entity.TitleEn = title;
                    entity.DescriptionEn = description;
                    break;
            }

            entity.Tagline = Resolve(entity.TaglineEn, entity.Tagline);
            entity.Title = Resolve(entity.TitleEn, entity.Title);
            entity.Description = Resolve(entity.DescriptionEn, entity.Description);
            entity.Language = "en";
        }

        private static string Assign(string current, params string?[] candidates)
        {
            if (!string.IsNullOrWhiteSpace(current)) return current;

            foreach (var candidate in candidates)
            {
                if (!string.IsNullOrWhiteSpace(candidate))
                {
                    return candidate.Trim();
                }
            }

            return current;
        }

        private static string Resolve(string? primary, params string?[] fallbacks)
        {
            if (!string.IsNullOrWhiteSpace(primary))
            {
                return primary.Trim();
            }

            foreach (var fallback in fallbacks)
            {
                if (!string.IsNullOrWhiteSpace(fallback))
                {
                    return fallback.Trim();
                }
            }

            return string.Empty;
        }

        private static LandingContent CreateDefault()
        {
            return new LandingContent
            {
                Language = "en",
                Tagline = "Away from monotonous life",
                Title = "Relax Your Mind",
                Description = "Step away from routine: gentle cruises, adventures, and sunny escapes are ready for you.",
                TaglineEn = "Away from monotonous life",
                TitleEn = "Relax Your Mind",
                DescriptionEn = "Step away from routine: gentle cruises, adventures, and sunny escapes are ready for you.",
                TaglineDe = "Raus aus dem Alltag",
                TitleDe = "Entspann deinen Geist",
                DescriptionDe = "Lass den Alltag hinter dir: sanfte Bootstouren, Naturerlebnisse und Ausflüge voller Sonne warten bereits auf dich.",
                TaglineTr = "Monoton hayattan uzaklaşın",
                TitleTr = "Zihninizi rahatlatın",
                DescriptionTr = "Rutinden uzaklaşın: sakin tekne gezileri, macera dolu aktiviteler ve güneşli kaçamaklar sizi bekliyor.",
                TaglineFa = "دور از زندگی یکنواخت",
                TitleFa = "ذهن خود را آرام کنید",
                DescriptionFa = "با تورهای ما از شلوغی دور شوید؛ سفرهایی آرام، ماجراجویانه و سرشار از تجربه‌های تازه.",
                TaglineRu = "Подальше от монотонной жизни",
                TitleRu = "Расслабьте свой разум",
                DescriptionRu = "Устройте себе отдых: море, приключения и новые впечатления ждут вас каждый день.",
                TaglinePl = "Z dala od monotonii",
                TitlePl = "Zrelaksuj swój umysł",
                DescriptionPl = "Odpocznij od codzienności: czekają na Ciebie rejsy, przygody i chwile czystego relaksu.",
                TaglineAr = "ابتعد عن الحياة الرتيبة",
                TitleAr = "أرخِ ذهنك",
                DescriptionAr = "امنح نفسك استراحة حقيقية: رحلات بحرية، مغامرات وتجارب مميزة تلائم كل الأذواق."
            };
        }
    }
}
