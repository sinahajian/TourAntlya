using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

public class AboutContentEditViewModel
{
    public string UserName { get; set; } = string.Empty;
    public string? FeedbackMessage { get; set; }

    public int Id { get; set; }

    [Display(Name = "Image path")]
    [Required]
    public string ImagePath { get; set; } = "/image/about_bg.jpg";

    [Display(Name = "Button URL")]
    public string ButtonUrl { get; set; } = "#";

    [Required]
    public string TitleLine1En { get; set; } = string.Empty;
    public string TitleLine1De { get; set; } = string.Empty;
    public string TitleLine1Tr { get; set; } = string.Empty;
    public string TitleLine1Fa { get; set; } = string.Empty;
    public string TitleLine1Ru { get; set; } = string.Empty;
    public string TitleLine1Pl { get; set; } = string.Empty;
    public string TitleLine1Ar { get; set; } = string.Empty;

    [Required]
    public string TitleLine2En { get; set; } = string.Empty;
    public string TitleLine2De { get; set; } = string.Empty;
    public string TitleLine2Tr { get; set; } = string.Empty;
    public string TitleLine2Fa { get; set; } = string.Empty;
    public string TitleLine2Ru { get; set; } = string.Empty;
    public string TitleLine2Pl { get; set; } = string.Empty;
    public string TitleLine2Ar { get; set; } = string.Empty;

    [Required]
    public string BodyEn { get; set; } = string.Empty;
    public string BodyDe { get; set; } = string.Empty;
    public string BodyTr { get; set; } = string.Empty;
    public string BodyFa { get; set; } = string.Empty;
    public string BodyRu { get; set; } = string.Empty;
    public string BodyPl { get; set; } = string.Empty;
    public string BodyAr { get; set; } = string.Empty;

    [Required]
    public string ButtonTextEn { get; set; } = string.Empty;
    public string ButtonTextDe { get; set; } = string.Empty;
    public string ButtonTextTr { get; set; } = string.Empty;
    public string ButtonTextFa { get; set; } = string.Empty;
    public string ButtonTextRu { get; set; } = string.Empty;
    public string ButtonTextPl { get; set; } = string.Empty;
    public string ButtonTextAr { get; set; } = string.Empty;

    [Display(Name = "Upload new image")]
    public IFormFile? ImageFile { get; set; }

    public static AboutContentEditViewModel FromDto(string userName, AboutContentEditDto dto)
    {
        return new AboutContentEditViewModel
        {
            UserName = userName,
            Id = dto.Id,
            ImagePath = dto.ImagePath,
            ButtonUrl = dto.ButtonUrl,
            TitleLine1En = dto.TitleLine1En,
            TitleLine1De = dto.TitleLine1De,
            TitleLine1Tr = dto.TitleLine1Tr,
            TitleLine1Fa = dto.TitleLine1Fa,
            TitleLine1Ru = dto.TitleLine1Ru,
            TitleLine1Pl = dto.TitleLine1Pl,
            TitleLine1Ar = dto.TitleLine1Ar,
            TitleLine2En = dto.TitleLine2En,
            TitleLine2De = dto.TitleLine2De,
            TitleLine2Tr = dto.TitleLine2Tr,
            TitleLine2Fa = dto.TitleLine2Fa,
            TitleLine2Ru = dto.TitleLine2Ru,
            TitleLine2Pl = dto.TitleLine2Pl,
            TitleLine2Ar = dto.TitleLine2Ar,
            BodyEn = dto.BodyEn,
            BodyDe = dto.BodyDe,
            BodyTr = dto.BodyTr,
            BodyFa = dto.BodyFa,
            BodyRu = dto.BodyRu,
            BodyPl = dto.BodyPl,
            BodyAr = dto.BodyAr,
            ButtonTextEn = dto.ButtonTextEn,
            ButtonTextDe = dto.ButtonTextDe,
            ButtonTextTr = dto.ButtonTextTr,
            ButtonTextFa = dto.ButtonTextFa,
            ButtonTextRu = dto.ButtonTextRu,
            ButtonTextPl = dto.ButtonTextPl,
            ButtonTextAr = dto.ButtonTextAr
        };
    }

    public AboutContentEditDto ToDto()
    {
        return new AboutContentEditDto(
            Id,
            ImagePath,
            ButtonUrl,
            TitleLine1En,
            TitleLine1De,
            TitleLine1Tr,
            TitleLine1Fa,
            TitleLine1Ru,
            TitleLine1Pl,
            TitleLine1Ar,
            TitleLine2En,
            TitleLine2De,
            TitleLine2Tr,
            TitleLine2Fa,
            TitleLine2Ru,
            TitleLine2Pl,
            TitleLine2Ar,
            BodyEn,
            BodyDe,
            BodyTr,
            BodyFa,
            BodyRu,
            BodyPl,
            BodyAr,
            ButtonTextEn,
            ButtonTextDe,
            ButtonTextTr,
            ButtonTextFa,
            ButtonTextRu,
            ButtonTextPl,
            ButtonTextAr
        );
    }
}
