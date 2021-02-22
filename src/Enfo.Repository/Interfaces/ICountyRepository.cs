using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Repository.Resources.County;

namespace Enfo.Repository.Interfaces
{
    public interface ICountyRepository : IDisposable
    {
        Task<CountyView> GetAsync(int id);
        Task<IReadOnlyList<CountyView>> ListAsync(bool includeInactive);
    }
}