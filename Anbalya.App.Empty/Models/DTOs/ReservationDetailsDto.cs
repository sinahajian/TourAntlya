using System;
using Models.Entities;

public record ReservationDetailsDto(
    int Id,
    int TourId,
    string TourName,
    string CustomerName,
    string CustomerEmail,
    string? CustomerPhone,
    DateTime? PreferredDate,
    int Adults,
    int Children,
    int Infants,
    string PickupLocation,
    string? Notes,
    PaymentMethod PaymentMethod,
    PaymentStatus PaymentStatus,
    ReservationStatus Status,
    string? PaymentReference,
    int TotalPrice,
    string? Language,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public int TotalGuests => Math.Max(0, Adults) + Math.Max(0, Children) + Math.Max(0, Infants);

    public static ReservationDetailsDto FromEntity(Reservation reservation) =>
        new(
            reservation.Id,
            reservation.TourId,
            reservation.Tour?.TourName ?? string.Empty,
            reservation.CustomerName,
            reservation.CustomerEmail,
            reservation.CustomerPhone,
            reservation.PreferredDate,
            reservation.Adults,
            reservation.Children,
            reservation.Infants,
            reservation.PickupLocation,
            reservation.Notes,
            reservation.PaymentMethod,
            reservation.PaymentStatus,
            reservation.Status,
            reservation.PaymentReference,
            reservation.TotalPrice,
            reservation.Language,
            reservation.CreatedAt,
            reservation.UpdatedAt);
}
