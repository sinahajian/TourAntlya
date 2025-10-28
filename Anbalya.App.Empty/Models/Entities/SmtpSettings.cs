namespace Models.Entities
{
    public class SmtpSettings : BaseEntity
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 587;
        public bool EnableSsl { get; set; } = true;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FromEmail { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
        public string NotificationEmail { get; set; } = string.Empty;
        public string? ReplyToEmail { get; set; }
    }
}
