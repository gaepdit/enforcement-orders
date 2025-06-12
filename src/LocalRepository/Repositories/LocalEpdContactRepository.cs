using Enfo.Domain.EpdContacts.Entities;
using Enfo.Domain.EpdContacts.Repositories;
using Enfo.Domain.EpdContacts.Resources;
using EnfoTests.TestData;

namespace Enfo.LocalRepository.Repositories;

public sealed class LocalEpdContactRepository : IEpdContactRepository
{
    public Task<EpdContactView> GetAsync(int id) =>
        EpdContactData.EpdContacts.Exists(e => e.Id == id)
            ? Task.FromResult(new EpdContactView(
                EpdContactData.EpdContacts.SingleOrDefault(e => e.Id == id)!))
            : Task.FromResult<EpdContactView>(null);

    public Task<IReadOnlyList<EpdContactView>> ListAsync(bool includeInactive = false) =>
        Task.FromResult<IReadOnlyList<EpdContactView>>(EpdContactData.EpdContacts
            .Where(e => e.Active || includeInactive)
            .Select(e => new EpdContactView(e)).ToList());

    public Task<int> CreateAsync(EpdContactCommand resource)
    {
        resource.TrimAll();
        var id = EpdContactData.EpdContacts.Max(e => e.Id) + 1;
        var item = new EpdContact(resource) { Id = id };
        EpdContactData.EpdContacts.Add(item);

        return Task.FromResult(id);
    }

    public Task UpdateAsync(EpdContactCommand resource)
    {
        if (!EpdContactData.EpdContacts.Exists(e => e.Id == resource.Id))
            throw new ArgumentException($"ID ({resource.Id}) not found.", nameof(resource));

        var item = EpdContactData.EpdContacts.Single(e => e.Id == resource.Id);

        if (!item.Active) throw new ArgumentException("Only active items can be edited.", nameof(resource));

        resource.TrimAll();
        item.ApplyUpdate(resource);

        return Task.CompletedTask;
    }

    public Task UpdateStatusAsync(int id, bool newActiveStatus)
    {
        if (!EpdContactData.EpdContacts.Exists(e => e.Id == id))
            throw new ArgumentException($"ID ({id}) not found.", nameof(id));

        var item = EpdContactData.EpdContacts.Single(e => e.Id == id);
        item.Active = newActiveStatus;

        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(int id) =>
        Task.FromResult(EpdContactData.EpdContacts.Exists(e => e.Id == id));

    public void Dispose()
    {
        // Method intentionally left empty.
    }
}
