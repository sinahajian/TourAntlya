using Models.Entities;

namespace Models.Entities
{
    public class PayPalSettings : BaseEntity
    {
        public string BusinessEmail { get; set; } = string.Empty;
        public string Currency { get; set; } = "EUR";
        public string ReturnUrl { get; set; } = string.Empty;
        public string CancelUrl { get; set; } = string.Empty;
        public bool UseSandbox { get; set; } = true;

        public string SandboxBaseUrl => "https://www.sandbox.paypal.com/cgi-bin/webscr";
        public string LiveBaseUrl => "https://www.paypal.com/cgi-bin/webscr";
        public string BaseUrl => UseSandbox ? SandboxBaseUrl : LiveBaseUrl;
    }
}
