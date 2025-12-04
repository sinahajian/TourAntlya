using System.ComponentModel.DataAnnotations;

namespace Models.Entities
{
    public class InvoiceSettings : BaseEntity
    {
        [MaxLength(200)]
        public string CompanyName { get; set; } = "Tour Antalya";

        [MaxLength(200)]
        public string CompanyEmail { get; set; } = "support@tourantalya.com";

        [MaxLength(100)]
        public string CompanyPhone { get; set; } = "+90 242 000 00 00";

        [MaxLength(400)]
        public string CompanyAddress { get; set; } = "Antalya, Turkey";

        [MaxLength(200)]
        public string LogoPath { get; set; } = "/image/logo.png";

        [MaxLength(40)]
        public string AccentColor { get; set; } = "#FF6B2C";

        public string HeaderHtml { get; set; } = string.Empty;
        public string IntroHtml { get; set; } = string.Empty;
        public string FooterHtml { get; set; } = string.Empty;
        public string TermsHtml { get; set; } = string.Empty;
    }
}
