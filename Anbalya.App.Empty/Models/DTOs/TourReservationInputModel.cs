using System;
using System.ComponentModel.DataAnnotations;
using Models.Entities;

public class TourReservationInputModel
{
    [Required]
    public int TourId { get; set; }

    public string? Language { get; set; }

    [Required]
    [Display(Name = "First name")]
    [StringLength(80)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Last name")]
    [StringLength(80)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string CustomerEmail { get; set; } = string.Empty;

    [Phone]
    [Display(Name = "Phone")]
    public string? CustomerPhone { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Preferred date")]
    public DateTime? PreferredDate { get; set; }

    [Range(1, 50)]
    [Display(Name = "Adults")]
    public int Adults { get; set; } = 1;

    [Range(0, 50)]
    [Display(Name = "Children")]
    public int Children { get; set; }

    [Range(0, 20)]
    [Display(Name = "Infants")]
    public int Infants { get; set; }

    [StringLength(160)]
    [Display(Name = "Pickup location")]
    public string PickupLocation { get; set; } = string.Empty;

    [Display(Name = "Hotel or accommodation")]
    [StringLength(160)]
    public string? HotelName { get; set; }

    [Display(Name = "Room number")]
    [StringLength(40)]
    public string? RoomNumber { get; set; }

    [StringLength(400)]
    [Display(Name = "Notes")]
    public string? Notes { get; set; }

    [Required]
    [Display(Name = "Payment method")]
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.PayPal;

    [StringLength(160)]
    [Display(Name = "Payment reference")]
    public string? PaymentReference { get; set; }

    public bool PayWithPayPal { get; set; }
}
