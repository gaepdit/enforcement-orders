using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Domain.Entities;
using Enfo.Repository.Resources.County;
using Enfo.Repository.Resources.EpdContact;
using Enfo.Repository.Resources.LegalAuthority;

namespace Enfo.Repository.Repositories
{
    public interface ILegalAuthorityRepository : IDisposable
    {
        Task<LegalAuthorityView> GetAsync(int id);
        Task<IReadOnlyList<LegalAuthorityView>> ListAsync(bool includeInactive);
        Task<int> CreateAsync(LegalAuthorityCreate resource);
        Task UpdateAsync(int id, LegalAuthorityUpdate resource);
    }
}