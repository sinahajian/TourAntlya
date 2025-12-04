using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models.Entities;
using Models.Interface;
using Models.Services;

namespace Models.Repository
{
    public class PaymentOptionRepository : IPaymentOptionRepository
    {
        public async Task<IReadOnlyList<PaymentOption>> ListAsync(CancellationToken ct = default)
        {
            var option = CreateStaticOption();
            return await Task.FromResult<IReadOnlyList<PaymentOption>>(new[] { option });
        }

        public async Task<PaymentOption?> GetByMethodAsync(PaymentMethod method, CancellationToken ct = default)
        {
            var option = CreateStaticOption();
            return await Task.FromResult(method == PaymentMethod.PayPal ? option : null);
        }

        public async Task UpdateAsync(PaymentOption option, CancellationToken ct = default)
        {
            // Static configuration cannot be updated at runtime.
            await Task.CompletedTask;
        }

        private static PaymentOption CreateStaticOption()
        {
            var snapshot = PayPalHelper.GetSnapshot();
            return new PaymentOption
            {
                Method = PaymentMethod.PayPal,
                DisplayName = string.IsNullOrWhiteSpace(snapshot.DisplayName) ? "PayPal" : snapshot.DisplayName,
                AccountIdentifier = snapshot.BusinessEmail,
                Instructions = string.IsNullOrWhiteSpace(snapshot.PaymentInstructions)
                    ? "Complete the reservation and follow the PayPal instructions sent via email."
                    : snapshot.PaymentInstructions,
                IsEnabled = true,
                UpdatedAt = DateTimeOffset.UtcNow
            };
        }
    }
}
