using System;

public class ReservationConfirmationViewModel
{
    public TourDto Tour { get; init; } = null!;
    public ReservationDetailsDto Reservation { get; init; } = null!;
    public PaymentOptionDto? PaymentOption { get; init; }
    public int TotalPrice => Reservation.TotalPrice;
    public string ReferenceCode => $"TA-{Reservation.Id:D6}";

    public string FormattedPreferredDate =>
        Reservation.PreferredDate?.ToString("yyyy-MM-dd") ?? "Flexible";

    public string FormattedCreatedAt =>
        Reservation.CreatedAt.ToLocalTime().ToString("yyyy-MM-dd HH:mm");

    public string GuestName => Reservation.FullName;
}
