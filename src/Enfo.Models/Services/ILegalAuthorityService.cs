using Enfo.Models.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Enfo.Models.Services
{
    public interface ILegalAuthorityService
    {
        Task<IEnumerable<LegalAuthorityResource>> GetAllAsync();
        Task<LegalAuthorityResource> GetByIdAsync(int id);
        Task<LegalAuthorityResource> CreateAsync(LegalAuthorityResource item);
        Task<LegalAuthorityResource> UpdateAsync(int id, LegalAuthorityResource item);
        Task<bool> ExistsAsync(int id);
    }
}
