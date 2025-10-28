namespace Models.Entities
{
    public class AboutContent : BaseEntity
    {
        public string ImagePath { get; set; } = "/image/about_bg.jpg";
        public string ButtonUrl { get; set; } = "#";

        public string TitleLine1En { get; set; } = string.Empty;
        public string TitleLine1De { get; set; } = string.Empty;
        public string TitleLine1Tr { get; set; } = string.Empty;
        public string TitleLine1Fa { get; set; } = string.Empty;
        public string TitleLine1Ru { get; set; } = string.Empty;
        public string TitleLine1Pl { get; set; } = string.Empty;
        public string TitleLine1Ar { get; set; } = string.Empty;

        public string TitleLine2En { get; set; } = string.Empty;
        public string TitleLine2De { get; set; } = string.Empty;
        public string TitleLine2Tr { get; set; } = string.Empty;
        public string TitleLine2Fa { get; set; } = string.Empty;
        public string TitleLine2Ru { get; set; } = string.Empty;
        public string TitleLine2Pl { get; set; } = string.Empty;
        public string TitleLine2Ar { get; set; } = string.Empty;

        public string BodyEn { get; set; } = string.Empty;
        public string BodyDe { get; set; } = string.Empty;
        public string BodyTr { get; set; } = string.Empty;
        public string BodyFa { get; set; } = string.Empty;
        public string BodyRu { get; set; } = string.Empty;
        public string BodyPl { get; set; } = string.Empty;
        public string BodyAr { get; set; } = string.Empty;

        public string ButtonTextEn { get; set; } = string.Empty;
        public string ButtonTextDe { get; set; } = string.Empty;
        public string ButtonTextTr { get; set; } = string.Empty;
        public string ButtonTextFa { get; set; } = string.Empty;
        public string ButtonTextRu { get; set; } = string.Empty;
        public string ButtonTextPl { get; set; } = string.Empty;
        public string ButtonTextAr { get; set; } = string.Empty;
    }
}
