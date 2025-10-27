using System;

namespace Models.Entities
{
    public class PaymentOption : BaseEntity
    {
        public PaymentMethod Method { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string AccountIdentifier { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
