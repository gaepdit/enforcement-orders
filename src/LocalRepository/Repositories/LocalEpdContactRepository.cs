using Enfo.Domain.EpdContacts;
using Enfo.Domain.EpdContacts.Resources;
using EnfoTests.TestData;

namespace Enfo.LocalRepository.Repositories;

public sealed class LocalEpdContactRepository : IEpdContactRepository
{
    public Task<EpdContactViewDto> GetAsync(int id, CancellationToken token) =>
        EpdContactData.EpdContacts.Exists(e => e.Id == id)
            ? Task.FromResult(new EpdContactViewDto(
                EpdContactData.EpdContacts.SingleOrDefault(e => e.Id == id)!))
            : Task.FromResult<EpdContactViewDto>(null);

    public Task<IReadOnlyList<EpdContactViewDto>> ListAsync(bool includeInactive = false,
        CancellationToken token = default) =>
        Task.FromResult<IReadOnlyList<EpdContactViewDto>>(EpdContactData.EpdContacts
            .Where(e => e.Active || includeInactive)
            .Select(e => new EpdContactViewDto(e)).ToList());

    public Task<int> CreateAsync(EpdContactCommand resource, CancellationToken token)
    {
        resource.TrimAll();
        var id = EpdContactData.EpdContacts.Max(e => e.Id) + 1;
        var item = new EpdContact(resource) { Id = id };
        EpdContactData.EpdContacts.Add(item);

        return Task.FromResult(id);
    }

    public Task UpdateAsync(int id, EpdContactCommand resource, CancellationToken token)
    {
        var item = EpdContactData.EpdContacts.SingleOrDefault(e => e.Id == id);
        if (item is null) throw new ArgumentException($"ID ({id}) not found.", nameof(resource));

        resource.TrimAll();
        item.ApplyUpdate(resource);

        return Task.CompletedTask;
    }

    public Task UpdateStatusAsync(int id, bool newActiveStatus, CancellationToken token)
    {
        if (!EpdContactData.EpdContacts.Exists(e => e.Id == id))
            throw new ArgumentException($"ID ({id}) not found.", nameof(id));

        var item = EpdContactData.EpdContacts.Single(e => e.Id == id);
        item.Active = newActiveStatus;

        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(int id, CancellationToken token) =>
        Task.FromResult(EpdContactData.EpdContacts.Exists(e => e.Id == id));

    public void Dispose()
    {
        // Method intentionally left empty.
    }
}
