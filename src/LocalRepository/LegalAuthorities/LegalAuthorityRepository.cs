using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;

namespace Enfo.LocalRepository.LegalAuthorities;

public sealed class LegalAuthorityRepository : ILegalAuthorityRepository
{
    public Task<LegalAuthorityView> GetAsync(int id) => throw new NotImplementedException();

    public Task<IReadOnlyList<LegalAuthorityView>> ListAsync(bool includeInactive = false) => throw new NotImplementedException();

    public Task<int> CreateAsync(LegalAuthorityCommand resource) => throw new NotImplementedException();

    public Task UpdateAsync(LegalAuthorityCommand resource) => throw new NotImplementedException();

    public Task UpdateStatusAsync(int id, bool newActiveStatus) => throw new NotImplementedException();

    public Task<bool> ExistsAsync(int id) => throw new NotImplementedException();

    public Task<bool> NameExistsAsync(string name, int? ignoreId = null) => throw new NotImplementedException();

    public void Dispose()
    {
        // Method intentionally left empty.
    }
}
