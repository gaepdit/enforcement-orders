using Enfo.Domain.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Enfo.Domain.Services
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
