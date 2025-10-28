public record RoyalFacilityDto(
    int Id,
    string IconClass,
    int DisplayOrder,
    string Title,
    string Description
);

public record RoyalFacilityEditDto(
    int Id,
    string IconClass,
    int DisplayOrder,
    string TitleEn,
    string TitleDe,
    string TitleTr,
    string TitleFa,
    string TitleRu,
    string TitlePl,
    string TitleAr,
    string DescriptionEn,
    string DescriptionDe,
    string DescriptionTr,
    string DescriptionFa,
    string DescriptionRu,
    string DescriptionPl,
    string DescriptionAr
);
