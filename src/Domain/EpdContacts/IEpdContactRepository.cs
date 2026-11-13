using Enfo.Domain.EpdContacts.Resources;

namespace Enfo.Domain.EpdContacts;

public interface IEpdContactRepository : IDisposable
{
    Task<EpdContactViewDto> GetAsync(int id, CancellationToken token);
    Task<IReadOnlyList<EpdContactViewDto>> ListAsync(bool includeInactive = false, CancellationToken token = default);
    Task<int> CreateAsync(EpdContactCommand resource, CancellationToken token);
    Task UpdateAsync(int id, EpdContactCommand resource, CancellationToken token);
    Task UpdateStatusAsync(int id, bool newActiveStatus, CancellationToken token);
    Task<bool> ExistsAsync(int id, CancellationToken token);
}
