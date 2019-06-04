using Enfo.Domain.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Enfo.Domain.Services
{
    public interface ICountyService
    {
        Task<IEnumerable<CountyResource>> GetAllAsync();
        Task<CountyResource> GetByIdAsync(int id);
        Task<CountyResource> GetByNameAsync(string name);
    }
}
