using Enfo.DataAccess.Contexts;
using Enfo.Models.Resources;
using Enfo.Models.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enfo.DataAccess.Services
{
    public class CountyService : BaseService, ICountyService
    {
        public CountyService(EnfoDbContext context) : base(context) { }

        public async Task<IEnumerable<CountyResource>> GetAllAsync()
        {
            var items = await Context.Counties.AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);

            return items.Select(e => new CountyResource(e));
        }

        public async Task<CountyResource> GetByIdAsync(int id)
        {
            var item = await Context.Counties
                .FindAsync(id)
                .ConfigureAwait(false);

            if (item == null)
            {
                return null;
            }

            return new CountyResource(item);
        }

        public async Task<CountyResource> GetByNameAsync(string name)
        {
            var item = await Context.Counties.AsNoTracking()
                .SingleOrDefaultAsync(e => e.CountyName == name)
                .ConfigureAwait(false);

            if (item == null)
            {
                return null;
            }

            return new CountyResource(item);
        }
    }
}
