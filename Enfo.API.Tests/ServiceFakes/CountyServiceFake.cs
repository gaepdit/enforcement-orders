using Enfo.Domain.Resources;
using Enfo.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Enfo.API.Tests.ServiceFakes
{
    class CountyServiceFake : ICountyService
    {
        private readonly List<CountyResource> counties;

        public CountyServiceFake(List<CountyResource> counties)
        {
            this.counties = counties;
        }

        public Task<IEnumerable<CountyResource>> GetAllAsync()
        {
            IEnumerable<CountyResource> counties = this.counties;
            return Task.FromResult(counties);
        }

        public Task<CountyResource> GetByIdAsync(int id)
        {
            CountyResource county = counties.Find(e => e.Id == id);
            return Task.FromResult(county);
        }

        public Task<CountyResource> GetByNameAsync(string name)
        {
            CountyResource county = counties.Find(e => e.CountyName == name);
            return Task.FromResult(county);
        }
    }
}
