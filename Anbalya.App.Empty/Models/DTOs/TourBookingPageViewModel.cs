using System.Collections.Generic;

public class TourBookingPageViewModel
{
    public TourDto Tour { get; init; } = null!;
    public TourReservationInputModel Form { get; set; } = new();
    public IReadOnlyList<PaymentOptionDto> PaymentOptions { get; init; } = new List<PaymentOptionDto>();
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }
}
