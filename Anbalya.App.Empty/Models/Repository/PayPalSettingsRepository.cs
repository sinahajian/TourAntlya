using System.Threading;
using System.Threading.Tasks;
using Models.Entities;
using Models.Interface;
using Models.Services;

namespace Models.Repository
{
    public class PayPalSettingsRepository : IPayPalSettingsRepository
    {
        public async Task<PayPalSettings> GetAsync(CancellationToken ct = default)
        {
            var snapshot = PayPalHelper.GetSnapshot();
            var settings = new PayPalSettings
            {
                BusinessEmail = snapshot.BusinessEmail,
                Currency = snapshot.Currency,
                ReturnUrl = snapshot.ReturnUrl,
                CancelUrl = snapshot.CancelUrl,
                UseSandbox = snapshot.UseSandbox,
                ClientId = snapshot.ClientId,
                ClientSecret = snapshot.ClientSecret
            };
            return await Task.FromResult(settings);
        }

        public async Task UpdateAsync(PayPalSettings settings, CancellationToken ct = default)
        {
            // Static configuration is defined in PayPalHelper; updates via repository are ignored.
            await Task.CompletedTask;
        }
    }
}



