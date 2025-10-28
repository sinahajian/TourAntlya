namespace Models.Entities
{
    public class LandingContent : BaseEntity
    {
        public string Language { get; set; } = "en";
        public string Tagline { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TaglineEn { get; set; } = string.Empty;
        public string TaglineDe { get; set; } = string.Empty;
        public string TaglineTr { get; set; } = string.Empty;
        public string TaglineFa { get; set; } = string.Empty;
        public string TaglineRu { get; set; } = string.Empty;
        public string TaglinePl { get; set; } = string.Empty;
        public string TaglineAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;
        public string TitleDe { get; set; } = string.Empty;
        public string TitleTr { get; set; } = string.Empty;
        public string TitleFa { get; set; } = string.Empty;
        public string TitleRu { get; set; } = string.Empty;
        public string TitlePl { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public string DescriptionDe { get; set; } = string.Empty;
        public string DescriptionTr { get; set; } = string.Empty;
        public string DescriptionFa { get; set; } = string.Empty;
        public string DescriptionRu { get; set; } = string.Empty;
        public string DescriptionPl { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string? BackgroundImage { get; set; }
    }
}
