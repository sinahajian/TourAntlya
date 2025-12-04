using System.Threading;
using System.Threading.Tasks;
using Models.Entities;

public interface IInvoiceSettingsRepository
{
    Task<InvoiceSettings> GetAsync(CancellationToken ct = default);
    Task UpdateAsync(InvoiceSettings settings, CancellationToken ct = default);
}
