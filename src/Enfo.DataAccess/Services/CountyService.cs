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
            var counties = await Context.Counties
                .ToListAsync()
                .ConfigureAwait(false);

            return counties.Select(item => new CountyResource(item));
        }

        public async Task<CountyResource> GetByIdAsync(int id)
        {
            var county = await Context.Counties
                .FindAsync(id)
                .ConfigureAwait(false);

            if (county == null)
            {
                return null;
            }

            return new CountyResource(county);
        }

        public async Task<CountyResource> GetByNameAsync(string name)
        {
            var county = await Context.Counties
                .SingleOrDefaultAsync(item => item.CountyName == name)
                .ConfigureAwait(false);

            if (county == null)
            {
                return null;
            }

            return new CountyResource(county);
        }
    }
}
