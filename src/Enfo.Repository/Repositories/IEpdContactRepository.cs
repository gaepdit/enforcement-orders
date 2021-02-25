using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Repository.Resources.EpdContact;

namespace Enfo.Repository.Repositories
{
    public interface IEpdContactRepository : IDisposable
    {
        Task<EpdContactView> GetAsync(int id);
        Task<IReadOnlyList<EpdContactView>> ListAsync(bool includeInactive = false);
        Task<int> CreateAsync(EpdContactCreate resource);
        Task UpdateAsync(int id, EpdContactUpdate resource);
    }
}