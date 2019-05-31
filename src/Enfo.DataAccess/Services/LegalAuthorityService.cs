using Enfo.DataAccess.Contexts;
using Enfo.Models.Resources;
using Enfo.Models.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enfo.DataAccess.Services
{
    public class LegalAuthorityService : BaseService, ILegalAuthorityService
    {
        public LegalAuthorityService(EnfoDbContext context) : base(context) { }

        public async Task<IEnumerable<LegalAuthorityResource>> GetAllAsync()
        {
            var items = await Context.LegalAuthorities.AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);

            return items.Select(e => new LegalAuthorityResource(e));
        }

        public async Task<LegalAuthorityResource> GetByIdAsync(int id)
        {
            var item = await Context.LegalAuthorities
                .FindAsync(id)
                .ConfigureAwait(false);

            if (item == null)
            {
                return null;
            }

            return new LegalAuthorityResource(item);
        }

        public Task<bool> CreateAsync(LegalAuthorityResource item)
        {

            throw new System.NotImplementedException();
        }

        public Task<LegalAuthorityResource> UpdateAsync(int id, LegalAuthorityResource item)
        {
            throw new System.NotImplementedException();
        }

        Task<LegalAuthorityResource> ILegalAuthorityService.CreateAsync(LegalAuthorityResource item)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await Context.LegalAuthorities.AsNoTracking()
                .AnyAsync(e => e.Id == id)
                .ConfigureAwait(false);
        }
    }
}
