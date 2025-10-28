using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models.Entities;

public interface IContactMessageRepository
{
    Task<int> CreateAsync(ContactMessage message, CancellationToken ct = default);
    Task<IReadOnlyList<ContactMessage>> ListAsync(ContactMessageStatus? status = null, CancellationToken ct = default);
    Task<ContactMessage?> GetByIdAsync(int id, CancellationToken ct = default);
    Task UpdateStatusAsync(int id, ContactMessageStatus status, CancellationToken ct = default);
}
