using Enfo.Models.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Enfo.Models.Services
{
    public interface ILegalAuthorityService
    {
        Task<IEnumerable<LegalAuthorityResource>> GetAllAsync();
        Task<LegalAuthorityResource> GetByIdAsync(int id);
    }
}
