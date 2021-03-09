using System.Collections.Generic;
using System.Linq;
using Enfo.Domain.Data;
using Enfo.Repository.Resources.County;

namespace Enfo.Infrastructure.Repositories
{
    public static class CountyRepository
    {
        public static CountyView Get(int id)
        {
            var county = DomainData.Counties.SingleOrDefault(e => e.Id == id);
            return county == null ? null : new CountyView(county);
        }

        public static IReadOnlyList<CountyView> List() =>
            DomainData.Counties.Select(e => new CountyView(e)).ToList();
    }
}
