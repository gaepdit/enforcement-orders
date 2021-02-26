using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Repository.Resources.LegalAuthority;

namespace Enfo.Repository.Repositories
{
    public interface ILegalAuthorityRepository : IDisposable
    {
        Task<LegalAuthorityView> GetAsync(int id);
        Task<IReadOnlyList<LegalAuthorityView>> ListAsync(bool includeInactive = false);
        Task<int> CreateAsync(LegalAuthorityCreate resource);
        Task UpdateAsync(int id, LegalAuthorityUpdate resource);
        Task<bool> ExistsAsync(int id);
    }
}