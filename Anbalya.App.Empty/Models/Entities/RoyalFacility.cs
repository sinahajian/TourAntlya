namespace Models.Entities
{
    public class RoyalFacility : BaseEntity
    {
        public string IconClass { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }

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
    }
}
