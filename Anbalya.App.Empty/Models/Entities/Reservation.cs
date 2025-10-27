using System;

namespace Models.Entities
{
    public class Reservation : BaseEntity
    {
        public int TourId { get; set; }
        public Tour? Tour { get; set; }

        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string? CustomerPhone { get; set; }

        public DateTime? PreferredDate { get; set; }

        public int Adults { get; set; }
        public int Children { get; set; }
        public int Infants { get; set; }

        public string PickupLocation { get; set; } = string.Empty;
        public string? Notes { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
        public string? PaymentReference { get; set; }

        public int TotalPrice { get; set; }
        public string? Language { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedAt { get; set; }

        public int TotalGuests => Math.Max(0, Adults) + Math.Max(0, Children) + Math.Max(0, Infants);
    }
}
