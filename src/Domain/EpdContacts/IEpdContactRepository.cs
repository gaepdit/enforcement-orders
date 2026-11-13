using Enfo.Domain.EpdContacts.Resources;

namespace Enfo.Domain.EpdContacts;

public interface IEpdContactRepository : IDisposable
{
    Task<EpdContactViewDto> GetAsync(int id, CancellationToken token = default);
    Task<IReadOnlyList<EpdContactViewDto>> ListAsync(bool includeInactive = false, CancellationToken token = default);
    Task<int> CreateAsync(EpdContactCommand resource, CancellationToken token = default);
    Task UpdateAsync(int id, EpdContactCommand resource, CancellationToken token = default);
    Task UpdateStatusAsync(int id, bool newActiveStatus, CancellationToken token = default);
    Task<bool> ExistsAsync(int id, CancellationToken token = default);
}
