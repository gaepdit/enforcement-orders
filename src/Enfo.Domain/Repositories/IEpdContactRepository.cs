using Enfo.Domain.Resources.EpdContact;

namespace Enfo.Domain.Repositories;

public interface IEpdContactRepository : IDisposable
{
    Task<EpdContactView> GetAsync(int id);
    Task<IReadOnlyList<EpdContactView>> ListAsync(bool includeInactive = false);
    Task<int> CreateAsync(EpdContactCommand resource);
    Task UpdateAsync(int id, EpdContactCommand resource);
    Task UpdateStatusAsync(int id, bool newActiveStatus);
    Task<bool> ExistsAsync(int id);
}
