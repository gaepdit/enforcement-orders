using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Domain.Resources.EpdContact;

namespace Enfo.Domain.Repositories
{
    public interface IEpdContactRepository : IDisposable
    {
        Task<EpdContactView> GetAsync(int id);
        Task<IReadOnlyList<EpdContactView>> ListAsync(bool includeInactive = false);
        Task<int> CreateAsync(EpdContactCreate resource);
        Task UpdateAsync(int id, EpdContactUpdate resource);
        Task UpdateStatusAsync(int id, bool newActiveStatus);
        Task<bool> ExistsAsync(int id);
    }
}
