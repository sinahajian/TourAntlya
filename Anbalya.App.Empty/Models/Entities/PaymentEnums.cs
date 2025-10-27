using System.ComponentModel.DataAnnotations;

namespace Models.Entities
{
    public enum PaymentMethod
    {
        [Display(Name = "PayPal")]
        PayPal = 0,

        [Display(Name = "Visa Card")]
        Visa = 1,

        [Display(Name = "Revolut")]
        Revolut = 2
    }

    public enum ReservationStatus
    {
        Pending = 0,
        Confirmed = 1,
        Cancelled = 2
    }

    public enum PaymentStatus
    {
        Pending = 0,
        Paid = 1,
        Refunded = 2,
        Failed = 3
    }
}
