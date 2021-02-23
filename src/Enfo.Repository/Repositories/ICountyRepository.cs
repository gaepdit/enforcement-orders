using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Repository.Resources.County;

namespace Enfo.Repository.Repositories
{
    public interface ICountyRepository : IDisposable
    {
        Task<CountyView> GetAsync(int id);
        Task<IReadOnlyList<CountyView>> ListAsync(bool includeInactive);
    }
}