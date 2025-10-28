using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public interface IAboutContentRepository
{
    Task<AboutContentDto> GetAsync(string language, CancellationToken ct = default);
    Task<AboutContentEditDto> GetForEditAsync(CancellationToken ct = default);
    Task UpdateAsync(AboutContentEditDto dto, CancellationToken ct = default);
}
