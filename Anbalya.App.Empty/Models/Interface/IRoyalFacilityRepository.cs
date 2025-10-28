using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public interface IRoyalFacilityRepository
{
    Task<IReadOnlyList<RoyalFacilityDto>> ListAsync(string language, CancellationToken ct = default);
    Task<IReadOnlyList<RoyalFacilityEditDto>> ListForEditAsync(CancellationToken ct = default);
    Task UpdateAsync(IEnumerable<RoyalFacilityEditDto> dtos, CancellationToken ct = default);
}
