using Models.Entities;

namespace Models.Interface
{
    public interface IPayPalSettingsRepository
    {
        Task<PayPalSettings> GetAsync(CancellationToken ct = default);
        Task UpdateAsync(PayPalSettings settings, CancellationToken ct = default);
    }
}
