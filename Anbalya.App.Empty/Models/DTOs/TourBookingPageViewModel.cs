using System;
using System.Collections.Generic;

public class TourBookingPageViewModel
{
    public TourDto Tour { get; init; } = null!;
    public TourReservationInputModel Form { get; set; } = new();
    public IReadOnlyList<PaymentOptionDto> PaymentOptions { get; init; } = new List<PaymentOptionDto>();
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }
    public IReadOnlyList<string> AccommodationOptions { get; init; } = new List<string>();
    public PayPalCheckoutInfo PayPal { get; init; } = PayPalCheckoutInfo.Disabled;
}

public record PayPalCheckoutInfo(bool Enabled, string BusinessEmail, string Currency, string ReturnUrl, string CancelUrl, bool UseSandbox)
{
    public static PayPalCheckoutInfo Disabled { get; } = new PayPalCheckoutInfo(false, string.Empty, "EUR", string.Empty, string.Empty, true);

    public string BaseUrl => UseSandbox ? "https://www.sandbox.paypal.com/cgi-bin/webscr" : "https://www.paypal.com/cgi-bin/webscr";

    public string BuildCheckoutUrl(decimal amount, string itemName, string invoiceId)
    {
        if (!Enabled || string.IsNullOrWhiteSpace(BusinessEmail)) return string.Empty;
        var encodedItem = Uri.EscapeDataString(itemName);
        var query = $"cmd=_xclick&business={Uri.EscapeDataString(BusinessEmail)}&currency_code={Currency}&amount={amount:0.00}&item_name={encodedItem}&invoice={Uri.EscapeDataString(invoiceId)}";
        if (!string.IsNullOrWhiteSpace(ReturnUrl))
        {
            query += $"&return={Uri.EscapeDataString(ReturnUrl)}";
        }
        if (!string.IsNullOrWhiteSpace(CancelUrl))
        {
            query += $"&cancel_return={Uri.EscapeDataString(CancelUrl)}";
        }
        return $"{BaseUrl}?{query}";
    }
}
