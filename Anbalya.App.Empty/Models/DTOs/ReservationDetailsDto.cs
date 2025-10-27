using System;
using System.Linq;
using Models.Entities;

public record ReservationDetailsDto(
    int Id,
    int TourId,
    string TourName,
    string CustomerFirstName,
    string CustomerLastName,
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
    string? HotelName,
    string? RoomNumber,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public int TotalGuests => Math.Max(0, Adults) + Math.Max(0, Children) + Math.Max(0, Infants);
    public string FullName => string.IsNullOrWhiteSpace(CustomerName)
        ? string.Join(" ", new[] { CustomerFirstName, CustomerLastName }.Where(s => !string.IsNullOrWhiteSpace(s)))
        : CustomerName;

    public static ReservationDetailsDto FromEntity(Reservation reservation) =>
        new(
            reservation.Id,
            reservation.TourId,
            reservation.Tour?.TourName ?? string.Empty,
            reservation.CustomerFirstName,
            reservation.CustomerLastName,
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
            reservation.HotelName,
            reservation.RoomNumber,
            reservation.CreatedAt,
            reservation.UpdatedAt);
}
