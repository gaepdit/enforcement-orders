using Enfo.Domain.LegalAuthorities.Entities;
using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using EnfoTests.TestData;

namespace Enfo.LocalRepository;

public sealed class LocalLegalAuthorityRepository : ILegalAuthorityRepository
{
    public Task<LegalAuthorityView> GetAsync(int id) =>
        LegalAuthorityData.LegalAuthorities.Any(e => e.Id == id)
            ? Task.FromResult(new LegalAuthorityView(
                LegalAuthorityData.LegalAuthorities.SingleOrDefault(e => e.Id == id)!))
            : Task.FromResult(null as LegalAuthorityView);

    public Task<IReadOnlyList<LegalAuthorityView>> ListAsync(bool includeInactive = false) =>
        Task.FromResult(
            (IReadOnlyList<LegalAuthorityView>)
            LegalAuthorityData.LegalAuthorities
                .Where(e => e.Active || includeInactive)
                .Select(e => new LegalAuthorityView(e)).ToList());

    public Task<int> CreateAsync(LegalAuthorityCommand resource)
    {
        resource.TrimAll();
        var id = LegalAuthorityData.LegalAuthorities.Max(e => e.Id) + 1;
        var item = new LegalAuthority(resource) { Id = id };
        LegalAuthorityData.LegalAuthorities.Add(item);

        return Task.FromResult(id);
    }

    public Task UpdateAsync(LegalAuthorityCommand resource)
    {
        if (LegalAuthorityData.LegalAuthorities.All(e => e.Id != resource.Id))
            throw new ArgumentException($"ID ({resource.Id}) not found.", nameof(resource));

        var item = LegalAuthorityData.LegalAuthorities.Single(e => e.Id == resource.Id);

        if (!item.Active) throw new ArgumentException("Only active items can be edited.", nameof(resource));

        resource.TrimAll();
        item.ApplyUpdate(resource);

        return Task.CompletedTask;
    }

    public Task UpdateStatusAsync(int id, bool newActiveStatus)
    {
        if (LegalAuthorityData.LegalAuthorities.All(e => e.Id != id))
            throw new ArgumentException($"ID ({id}) not found.", nameof(id));

        var item = LegalAuthorityData.LegalAuthorities.Single(e => e.Id == id);
        item.Active = newActiveStatus;

        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(int id) =>
        Task.FromResult(
            LegalAuthorityData.LegalAuthorities.Any(e => e.Id == id));

    public Task<bool> NameExistsAsync(string name) =>
        Task.FromResult(
            LegalAuthorityData.LegalAuthorities.Any(e => e.AuthorityName == name));

    public void Dispose()
    {
        // Method intentionally left empty.
    }
}
