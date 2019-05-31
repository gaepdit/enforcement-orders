using Enfo.Models.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Enfo.Models.Services
{
    public interface ICountyService
    {
        Task<IEnumerable<CountyResource>> GetAllAsync();
        Task<CountyResource> GetByIdAsync(int id);
        Task<CountyResource> GetByNameAsync(string name);
    }
}
