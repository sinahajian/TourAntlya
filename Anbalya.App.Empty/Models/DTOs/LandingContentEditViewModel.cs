using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Models.Helper;

public class LandingContentEditViewModel
{
    public string UserName { get; set; } = string.Empty;

    [Display(Name = "Background Image URL")]
    public string? BackgroundImage { get; set; }

    [Display(Name = "Replace Background Image")]
    public IFormFile? BackgroundImageFile { get; set; }

    public string ActiveLanguage { get; set; } = "en";

    public List<LandingContentLanguageInput> Languages { get; set; } = new();

    public static LandingContentEditViewModel FromDtos(IEnumerable<LandingContentDto> dtos, string userName, string? activeLanguage = null)
    {
        var normalizedActive = LanguageCatalog.Normalize(activeLanguage ?? "en");
        var dtoList = dtos.ToList();
        var languageInputs = LanguageCatalog.Options
            .Select(option =>
            {
                var match = dtoList.FirstOrDefault(d =>
                    LanguageCatalog.Normalize(d.Language) == option.Code);

                return new LandingContentLanguageInput
                {
                    Code = option.Code,
                    DisplayName = option.DisplayName,
                    Tagline = match?.Tagline ?? string.Empty,
                    Title = match?.Title ?? string.Empty,
                    Description = match?.Description ?? string.Empty,
                    IsRtl = LanguageCatalog.IsRtl(option.Code)
                };
            })
            .ToList();

        var background = dtoList.FirstOrDefault(d =>
            LanguageCatalog.Normalize(d.Language) == normalizedActive)?.BackgroundImage
            ?? dtoList.FirstOrDefault()?.BackgroundImage;

        return new LandingContentEditViewModel
        {
            UserName = userName,
            BackgroundImage = background,
            ActiveLanguage = normalizedActive,
            Languages = languageInputs
        };
    }

    public IReadOnlyList<LandingContentDto> ToDtos()
    {
        EnsureLanguageMetadata();

        var background = string.IsNullOrWhiteSpace(BackgroundImage)
            ? null
            : BackgroundImage.Trim();

        return Languages
            .Select(lang => new LandingContentDto(
                LanguageCatalog.Normalize(lang.Code),
                lang.Tagline?.Trim() ?? string.Empty,
                lang.Title?.Trim() ?? string.Empty,
                lang.Description?.Trim() ?? string.Empty,
                background))
            .ToList();
    }

    public void EnsureLanguageMetadata()
    {
        ActiveLanguage = LanguageCatalog.Normalize(ActiveLanguage);

        var byLanguage = Languages
            .GroupBy(l => LanguageCatalog.Normalize(l.Code))
            .ToDictionary(g => g.Key, g => g.Last());

        var ordered = new List<LandingContentLanguageInput>();
        foreach (var option in LanguageCatalog.Options)
        {
            if (!byLanguage.TryGetValue(option.Code, out var entry))
            {
                entry = new LandingContentLanguageInput
                {
                    Code = option.Code,
                    DisplayName = option.DisplayName,
                    IsRtl = LanguageCatalog.IsRtl(option.Code)
                };
            }
            else
            {
                entry.Code = option.Code;
                entry.DisplayName = option.DisplayName;
                entry.IsRtl = LanguageCatalog.IsRtl(option.Code);
                entry.Tagline ??= string.Empty;
                entry.Title ??= string.Empty;
                entry.Description ??= string.Empty;
            }

            ordered.Add(entry);
        }

        Languages = ordered;
    }
}

public class LandingContentLanguageInput
{
    [Required]
    public string Code { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public bool IsRtl { get; set; }

    [Required]
    [MaxLength(160)]
    [Display(Name = "Tagline")]
    public string Tagline { get; set; } = string.Empty;

    [Required]
    [MaxLength(160)]
    [Display(Name = "Headline")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;
}
