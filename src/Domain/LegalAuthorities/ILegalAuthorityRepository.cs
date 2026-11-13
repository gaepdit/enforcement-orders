using Enfo.Domain.LegalAuthorities.Resources;

namespace Enfo.Domain.LegalAuthorities;

public interface ILegalAuthorityRepository : IDisposable
{
    Task<LegalAuthorityViewDto> GetAsync(int id, CancellationToken token);

    Task<IReadOnlyList<LegalAuthorityViewDto>> ListAsync(bool includeInactive = false,
        CancellationToken token = default);

    Task<int> CreateAsync(LegalAuthorityCreateDto resource, CancellationToken token);
    Task UpdateAsync(int id, LegalAuthorityUpdateDto resource, CancellationToken token);
    Task<bool> ExistsAsync(int id, CancellationToken token);
    Task<bool> NameExistsAsync(string name, CancellationToken token);
    Task<int?> FindIdByNameAsync(string name, CancellationToken token);
}
