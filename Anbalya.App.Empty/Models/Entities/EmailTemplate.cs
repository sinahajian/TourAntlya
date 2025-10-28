namespace Models.Entities
{
    public class EmailTemplate : BaseEntity
    {
        public string TemplateKey { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string? Language { get; set; }
    }
}
