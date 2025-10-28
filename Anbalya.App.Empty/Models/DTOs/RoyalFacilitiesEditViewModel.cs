using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class RoyalFacilitiesEditViewModel
{
    public string UserName { get; set; } = string.Empty;
    public List<RoyalFacilityEditItem> Facilities { get; set; } = new();
    public string? FeedbackMessage { get; set; }

    public static RoyalFacilitiesEditViewModel FromDtos(string userName, IReadOnlyList<RoyalFacilityEditDto> dtos)
    {
        var model = new RoyalFacilitiesEditViewModel
        {
            UserName = userName
        };

        foreach (var dto in dtos)
        {
            model.Facilities.Add(new RoyalFacilityEditItem
            {
                Id = dto.Id,
                IconClass = dto.IconClass,
                DisplayOrder = dto.DisplayOrder,
                TitleEn = dto.TitleEn,
                TitleDe = dto.TitleDe,
                TitleTr = dto.TitleTr,
                TitleFa = dto.TitleFa,
                TitleRu = dto.TitleRu,
                TitlePl = dto.TitlePl,
                TitleAr = dto.TitleAr,
                DescriptionEn = dto.DescriptionEn,
                DescriptionDe = dto.DescriptionDe,
                DescriptionTr = dto.DescriptionTr,
                DescriptionFa = dto.DescriptionFa,
                DescriptionRu = dto.DescriptionRu,
                DescriptionPl = dto.DescriptionPl,
                DescriptionAr = dto.DescriptionAr
            });
        }

        return model;
    }
}

public class RoyalFacilityEditItem
{
    public int Id { get; set; }

    [Required]
    public string IconClass { get; set; } = string.Empty;

    [Range(0, 999)]
    public int DisplayOrder { get; set; }

    [Required]
    public string TitleEn { get; set; } = string.Empty;
    public string TitleDe { get; set; } = string.Empty;
    public string TitleTr { get; set; } = string.Empty;
    public string TitleFa { get; set; } = string.Empty;
    public string TitleRu { get; set; } = string.Empty;
    public string TitlePl { get; set; } = string.Empty;
    public string TitleAr { get; set; } = string.Empty;

    [Required]
    public string DescriptionEn { get; set; } = string.Empty;
    public string DescriptionDe { get; set; } = string.Empty;
    public string DescriptionTr { get; set; } = string.Empty;
    public string DescriptionFa { get; set; } = string.Empty;
    public string DescriptionRu { get; set; } = string.Empty;
    public string DescriptionPl { get; set; } = string.Empty;
    public string DescriptionAr { get; set; } = string.Empty;
}
