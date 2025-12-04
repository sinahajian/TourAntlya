using Models.Entities;

namespace Models.Interface
{
    public interface IReservationRepository
    {
        Task<int> CreateAsync(Reservation reservation, CancellationToken ct = default);
        Task<IReadOnlyList<Reservation>> ListAsync(int? tourId = null, CancellationToken ct = default);
        Task<Reservation?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Reservation?> GetByPaymentReferenceAsync(string paymentReference, CancellationToken ct = default);
        Task UpdateStatusAsync(int id, ReservationStatus status, PaymentStatus paymentStatus, string? paymentReference, CancellationToken ct = default);
    }
}
