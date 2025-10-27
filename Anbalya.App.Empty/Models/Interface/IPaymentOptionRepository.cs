using Models.Entities;

namespace Models.Interface
{
    public interface IPaymentOptionRepository
    {
        Task<IReadOnlyList<PaymentOption>> ListAsync(CancellationToken ct = default);
        Task<PaymentOption?> GetByMethodAsync(PaymentMethod method, CancellationToken ct = default);
        Task UpdateAsync(PaymentOption option, CancellationToken ct = default);
    }
}
