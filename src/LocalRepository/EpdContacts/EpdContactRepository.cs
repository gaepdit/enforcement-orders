using Enfo.Domain.EpdContacts.Repositories;
using Enfo.Domain.EpdContacts.Resources;

namespace Enfo.LocalRepository.EpdContacts;

public sealed class EpdContactRepository : IEpdContactRepository
{
    public Task<EpdContactView> GetAsync(int id) => throw new NotImplementedException();

    public Task<IReadOnlyList<EpdContactView>> ListAsync(bool includeInactive = false) =>
        throw new NotImplementedException();

    public Task<int> CreateAsync(EpdContactCommand resource) => throw new NotImplementedException();

    public Task UpdateAsync(EpdContactCommand resource) => throw new NotImplementedException();

    public Task UpdateStatusAsync(int id, bool newActiveStatus) => throw new NotImplementedException();

    public Task<bool> ExistsAsync(int id) => throw new NotImplementedException();

    public void Dispose()
    {
        // Method intentionally left empty.
    }
}
