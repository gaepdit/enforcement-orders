using Enfo.Domain.LegalAuthorities.Resources;

namespace Enfo.Domain.LegalAuthorities;

public interface ILegalAuthorityRepository : IDisposable
{
    Task<LegalAuthorityViewDto> GetAsync(int id, CancellationToken token = default);

    Task<IReadOnlyList<LegalAuthorityViewDto>> ListAsync(bool includeInactive = false,
        CancellationToken token = default);

    Task<int> CreateAsync(LegalAuthorityCreateDto resource, CancellationToken token = default);
    Task UpdateAsync(int id, LegalAuthorityUpdateDto resource, CancellationToken token = default);
    Task<bool> ExistsAsync(int id, CancellationToken token = default);
    Task<bool> NameExistsAsync(string name, CancellationToken token = default);
    Task<int?> FindIdByNameAsync(string name, CancellationToken token = default);
}
