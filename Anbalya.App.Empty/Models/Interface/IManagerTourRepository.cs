using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Models.Interface
{
    public interface IManagerTourRepository
    {
        Task<IReadOnlyList<ManagerTourListItemDto>> ListAsync(CancellationToken ct = default);
        Task DeleteAsync(int tourId, CancellationToken ct = default);
        Task SetPrimaryPhotoAsync(int tourId, string photoAddress, CancellationToken ct = default);
        Task<ManagerTourEditDto?> GetForEditAsync(int tourId, CancellationToken ct = default);
        Task UpdateAsync(ManagerTourEditDto dto, CancellationToken ct = default);
        Task<int> AddTourAsync(ManagerTourCreateDto dto, IEnumerable<string> photoAddresses, CancellationToken ct = default);
        Task AddPhotosAsync(int tourId, IEnumerable<string> photoAddresses, CancellationToken ct = default);
    }
}
