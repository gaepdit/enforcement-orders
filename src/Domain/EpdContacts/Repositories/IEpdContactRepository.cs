using Enfo.Domain.EpdContacts.Resources;

namespace Enfo.Domain.EpdContacts.Repositories;

public interface IEpdContactRepository : IDisposable
{
    Task<EpdContactView> GetAsync(int id);
    Task<IReadOnlyList<EpdContactView>> ListAsync(bool includeInactive = false);
    Task<int> CreateAsync(EpdContactCommand resource);
    Task UpdateAsync(EpdContactCommand resource);
    Task UpdateStatusAsync(int id, bool newActiveStatus);
    Task<bool> ExistsAsync(int id);
}
