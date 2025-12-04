public record PayPalResultViewModel
{
    public bool Success { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public string Invoice { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public string Currency { get; init; } = "EUR";
    public string? PayerId { get; init; }
    public string? Token { get; init; }
    public ReservationDetailsDto? Reservation { get; init; }
    public TourDto? Tour { get; init; }
}
