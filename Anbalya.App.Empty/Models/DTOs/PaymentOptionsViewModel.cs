using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Models.Entities;

public class PaymentOptionsViewModel
{
    public string UserName { get; set; } = string.Empty;
    public List<PaymentOptionInputModel> Options { get; set; } = new();
    public string? FeedbackMessage { get; set; }

    public static PaymentOptionsViewModel FromOptions(string userName, IEnumerable<PaymentOption> options)
    {
        var optionList = options
            .OrderBy(o => o.Method)
            .Select(PaymentOptionInputModel.FromEntity)
            .ToList();

        return new PaymentOptionsViewModel
        {
            UserName = userName,
            Options = optionList
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
