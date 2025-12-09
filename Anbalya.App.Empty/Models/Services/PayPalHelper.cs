using Microsoft.Extensions.Logging;
using Models.Interface;
using PaypalServerSdk.Standard;
using PaypalServerSdk.Standard.Authentication;

namespace Models.Services;

public class PayPalHelper : IPayPalHelper
{
    private readonly PaypalServerSdkClient _client;

    public PayPalHelper(ILogger<PayPalHelper> logger)
    {
        if (string.IsNullOrWhiteSpace(PayPalStaticConfig.ClientId) || string.IsNullOrWhiteSpace(PayPalStaticConfig.ClientSecret))
        {
            logger.LogWarning("PayPal API credentials are missing. Update PayPalStaticConfig in PayPalHelper.cs.");
        }

        var environment = PayPalStaticConfig.UseSandbox
            ? PaypalServerSdk.Standard.Environment.Sandbox
            : PaypalServerSdk.Standard.Environment.Production;

        var authModel = new ClientCredentialsAuthModel.Builder(
            PayPalStaticConfig.ClientId ?? string.Empty,
            PayPalStaticConfig.ClientSecret ?? string.Empty);

        _client = new PaypalServerSdkClient.Builder()
            .ClientCredentialsAuth(authModel.Build())
            .Environment(environment)
            .LoggingConfig(config => config
                .LogLevel(PayPalStaticConfig.LogLevel)
                .RequestConfig(req => req.Body(PayPalStaticConfig.LogRequestBody))
                .ResponseConfig(resp => resp.Headers(PayPalStaticConfig.LogResponseHeaders)))
            .Build();
    }

    public PaypalServerSdkClient Client => _client;

    public static PayPalSettingsSnapshot GetSnapshot() => new()
    {
        BusinessEmail = PayPalStaticConfig.BusinessEmail,
        Currency = PayPalStaticConfig.Currency,
        ReturnUrl = PayPalStaticConfig.ReturnUrl,
        CancelUrl = PayPalStaticConfig.CancelUrl,
        UseSandbox = PayPalStaticConfig.UseSandbox,
        ClientId = PayPalStaticConfig.ClientId,
        ClientSecret = PayPalStaticConfig.ClientSecret,
        DisplayName = PayPalStaticConfig.DisplayName,
        PaymentInstructions = PayPalStaticConfig.PaymentInstructions
    };
}

public static class PayPalStaticConfig
{
    /* Test cards (sandbox)
    4032030313135015          Successful card – Expiry: any, CVC: any
    Trigger                   Processor response code
    Fraudulent card           CCREJECT-SF      9500
    Card is declined          CCREJECT-BANK_ERROR 5100
    CVC check fails           CCREJECT-CVV_F   00N7
    Card expired              CCREJECT-EC      5400

    3DS successful authentication
    Visa       4868 7194 6070 7704   Expiry 01/2025   CVC 123
    Mastercard 5329 8797 8623 4393   Expiry 01/2025   CVC 123

    Failed signature
    Visa       4868 7191 1551 4992   Expiry 01/2025   CVC 123
    Mastercard 5329 8797 8516 0250   Expiry 01/2025   CVC 123

    PayPal sandbox business login
    Email:    sb-jvulg46963544@business.example.com
    Password: Go)C)>F1
    */

    // Sandbox business credentials
    public static string BusinessEmail { get; set; } = "sb-jvulg46963544@business.example.com";
    public static string ClientId { get; set; } = "AdhqS8_mrp_kCRCodO7DkAbnTA1sWbBkF6P3VLnJ762-5Jbif0GMAG3kOawAdtlcpzV94p_29SRViOH7";
    public static string ClientSecret { get; set; } = "EBqPi7oq2y5j9OMqnt-X3WzPwLtUmZU8NIH56a5ucW1KRLVuS41WzXRdA63I3auESIoBgP8FxoPfIjIm";

    // Checkout settings
    public static string Currency { get; set; } = "EUR";
    public static bool UseSandbox { get; set; } = false;
    public static string ReturnUrl { get; set; } = "https://anbalya.com/paypal/success";
    public static string CancelUrl { get; set; } = "https://anbalya.com/paypal/cancel";
    public static string DisplayName { get; set; } = "PayPal";
    public static string PaymentInstructions { get; set; } =
        "Complete your reservation and follow the PayPal checkout link we send by email.";

    // Logging preferences
    public static Microsoft.Extensions.Logging.LogLevel LogLevel { get; set; } = Microsoft.Extensions.Logging.LogLevel.Information;
    public static bool LogRequestBody { get; set; }
    public static bool LogResponseHeaders { get; set; }
}

public record PayPalSettingsSnapshot
{
    public string BusinessEmail { get; init; } = string.Empty;
    public string Currency { get; init; } = "EUR";
    public string ReturnUrl { get; init; } = string.Empty;
    public string CancelUrl { get; init; } = string.Empty;
    public bool UseSandbox { get; init; }
    public string ClientId { get; init; } = string.Empty;
    public string ClientSecret { get; init; } = string.Empty;
    public string DisplayName { get; init; } = "PayPal";
    public string PaymentInstructions { get; init; } = string.Empty;
}
