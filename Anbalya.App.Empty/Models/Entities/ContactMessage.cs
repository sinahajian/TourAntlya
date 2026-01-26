namespace Models.Entities
{
    public class ContactMessage : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Language { get; set; }
        public ContactMessageStatus Status { get; set; } = ContactMessageStatus.New;
        public long UpdatedTime { get; set; }
    }

    public enum ContactMessageStatus
    {
        New = 0,
        Read = 1,
        Archived = 2,
        Spam = 3
    }
}
