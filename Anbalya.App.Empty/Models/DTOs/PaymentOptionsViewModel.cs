using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Models.Entities;

public class PaymentOptionsViewModel
{
    public string UserName { get; set; } = string.Empty;
    public List<PaymentOptionInputModel> Options { get; set; } = new();
    public string? FeedbackMessage { get; set; }
    public PayPalSettingsInputModel PayPal { get; set; } = new();

    public static PaymentOptionsViewModel FromOptions(string userName, IEnumerable<PaymentOption> options, PayPalSettings payPalSettings)
    {
        var optionList = options
            .OrderBy(o => o.Method)
            .Select(PaymentOptionInputModel.FromEntity)
            .ToList();

        if (optionList.Count == 0)
        {
            optionList.Add(new PaymentOptionInputModel
            {
                Method = PaymentMethod.PayPal,
                DisplayName = "PayPal",
                AccountIdentifier = string.Empty,
                Instructions = "Guests will receive PayPal payment details after booking.",
                IsEnabled = true
            });
        }

        return new PaymentOptionsViewModel
        {
            UserName = userName,
            Options = optionList,
            PayPal = PayPalSettingsInputModel.FromEntity(payPalSettings)
        };
    }
}

public class PaymentOptionInputModel
{
    [Required]
    public PaymentMethod Method { get; set; }

    [Required]
    [StringLength(60)]
    public string DisplayName { get; set; } = string.Empty;

    [Required]
    [StringLength(120)]
    public string AccountIdentifier { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Instructions { get; set; } = string.Empty;

    public bool IsEnabled { get; set; } = true;

    public static PaymentOptionInputModel FromEntity(PaymentOption option) =>
        new PaymentOptionInputModel
        {
            Method = option.Method,
            DisplayName = option.DisplayName,
            AccountIdentifier = option.AccountIdentifier,
            Instructions = option.Instructions,
            IsEnabled = option.IsEnabled
        };
}

public class PayPalSettingsInputModel
{
    [EmailAddress]
    [Display(Name = "PayPal business email")]
    public string BusinessEmail { get; set; } = string.Empty;

    [Required]
    [StringLength(10)]
    [Display(Name = "Currency code")]
    public string Currency { get; set; } = "EUR";

    [Required]
    [StringLength(200)]
    [Display(Name = "Success return URL")]
    public string ReturnUrl { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    [Display(Name = "Cancel return URL")]
    public string CancelUrl { get; set; } = string.Empty;

    [Display(Name = "Use PayPal sandbox (test mode)")]
    public bool UseSandbox { get; set; } = true;

    [Required]
    [StringLength(256)]
    [Display(Name = "PayPal API client ID")]
    public string ClientId { get; set; } = string.Empty;

    [Required]
    [StringLength(256)]
    [Display(Name = "PayPal API client secret")]
    public string ClientSecret { get; set; } = string.Empty;

    public static PayPalSettingsInputModel FromEntity(PayPalSettings settings) => new PayPalSettingsInputModel
    {
        BusinessEmail = settings.BusinessEmail,
        Currency = settings.Currency,
        ReturnUrl = settings.ReturnUrl,
        CancelUrl = settings.CancelUrl,
        UseSandbox = settings.UseSandbox,
        ClientId = settings.ClientId,
        ClientSecret = settings.ClientSecret
    };

    public PayPalSettings ToEntity(int id = 1) => new PayPalSettings
    {
        Id = id,
        BusinessEmail = BusinessEmail?.Trim() ?? string.Empty,
        Currency = Currency?.Trim().ToUpperInvariant() ?? "EUR",
        ReturnUrl = ReturnUrl?.Trim() ?? string.Empty,
        CancelUrl = CancelUrl?.Trim() ?? string.Empty,
        UseSandbox = UseSandbox,
        ClientId = ClientId?.Trim() ?? string.Empty,
        ClientSecret = ClientSecret?.Trim() ?? string.Empty
    };
}
