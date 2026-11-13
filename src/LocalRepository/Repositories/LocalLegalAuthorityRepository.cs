using Enfo.Domain.LegalAuthorities;
using Enfo.Domain.LegalAuthorities.Resources;
using EnfoTests.TestData;

namespace Enfo.LocalRepository.Repositories;

public sealed class LocalLegalAuthorityRepository : ILegalAuthorityRepository
{
    public Task<LegalAuthorityViewDto> GetAsync(int id, CancellationToken token) =>
        LegalAuthorityData.LegalAuthorities.Exists(e => e.Id == id)
            ? Task.FromResult(new LegalAuthorityViewDto(
                LegalAuthorityData.LegalAuthorities.SingleOrDefault(e => e.Id == id) ??
                throw new ArgumentException($"ID ({id}) not found.", nameof(id))))
            : Task.FromResult<LegalAuthorityViewDto>(null);

    public Task<IReadOnlyList<LegalAuthorityViewDto>> ListAsync(bool includeInactive = false,
        CancellationToken token = default) =>
        Task.FromResult<IReadOnlyList<LegalAuthorityViewDto>>(LegalAuthorityData.LegalAuthorities
            .Where(e => e.Active || includeInactive)
            .Select(e => new LegalAuthorityViewDto(e)).ToList());

    public Task<int> CreateAsync(LegalAuthorityCreateDto resource, CancellationToken token)
    {
        var id = LegalAuthorityData.LegalAuthorities.Max(e => e.Id) + 1;
        var item = new LegalAuthority { Id = id, AuthorityName = resource.AuthorityName };
        LegalAuthorityData.LegalAuthorities.Add(item);

        return Task.FromResult(id);
    }

    public Task UpdateAsync(int id, LegalAuthorityUpdateDto resource, CancellationToken token)
    {
        var item = LegalAuthorityData.LegalAuthorities.SingleOrDefault(e => e.Id == id) ??
                   throw new ArgumentException($"ID ({id}) not found.", nameof(resource));

        item.AuthorityName = resource.AuthorityName.Trim();
        item.Active = resource.Active;

        return Task.CompletedTask;
    }


    public Task<bool> ExistsAsync(int id, CancellationToken token) =>
        Task.FromResult(LegalAuthorityData.LegalAuthorities.Exists(e => e.Id == id));

    public Task<bool> NameExistsAsync(string name, CancellationToken token) =>
        Task.FromResult(LegalAuthorityData.LegalAuthorities.Exists(e => e.AuthorityName == name));

    public Task<int?> FindIdByNameAsync(string name, CancellationToken token) =>
        Task.FromResult(LegalAuthorityData.LegalAuthorities.SingleOrDefault(e => e.AuthorityName == name)?.Id);

    public void Dispose()
    {
        // Method intentionally left empty.
    }
}
