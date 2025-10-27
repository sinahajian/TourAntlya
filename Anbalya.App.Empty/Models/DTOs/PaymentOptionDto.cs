using Models.Entities;

public record PaymentOptionDto(
    int Id,
    PaymentMethod Method,
    string DisplayName,
    string AccountIdentifier,
    string Instructions,
    bool IsEnabled)
{
    public static PaymentOptionDto FromEntity(PaymentOption option) =>
        new(option.Id, option.Method, option.DisplayName, option.AccountIdentifier, option.Instructions, option.IsEnabled);
}
