using System.Threading;
using System.Threading.Tasks;
namespace Models.Services
{
    public interface IInvoiceDocumentService
    {
        Task<EmailAttachment?> CreateInvoiceAsync(
            TourDto tour,
            ReservationDetailsDto reservation,
            decimal amount,
            string currency,
            CancellationToken ct = default);
    }
}
