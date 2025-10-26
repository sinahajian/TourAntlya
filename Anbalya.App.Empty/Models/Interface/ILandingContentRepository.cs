using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public interface ILandingContentRepository
{
    Task<LandingContentDto> GetAsync(string language, CancellationToken ct = default);
    Task UpdateAsync(LandingContentDto dto, CancellationToken ct = default);
    Task UpdateAllAsync(IEnumerable<LandingContentDto> dtos, CancellationToken ct = default);
    Task<IReadOnlyList<LandingContentDto>> ListAllAsync(CancellationToken ct = default);
}
