using Enfo.DataAccess.Contexts;
using Enfo.DataAccess.Services;
using Enfo.Models.Resources;
using Enfo.Models.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Enfo.DataAccess.Tests.ServiceTests
{
    public class CountyServiceTests
    {
        private ICountyService GetCountyService()
        {
            var options = new DbContextOptionsBuilder<EnfoDbContext>()
                .UseSqlite("Data Source=EnfoSqliteTestDatabase.db")
                .Options;

            var context = new EnfoDbContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return new CountyService(context);
        }

        [Fact]
        public async Task GetAllReturnsCountyList()
        {
            ICountyService countyService = GetCountyService();

            IEnumerable<CountyResource> counties = await countyService.GetAllAsync()
                .ConfigureAwait(false);

            Assert.Equal(159, counties.Count());
            Assert.Equal("Appling", counties.First().CountyName);
        }

        [Fact]
        public async Task GetByIdReturnsCountyAsync()
        {
            ICountyService countyService = GetCountyService();

            CountyResource county = await countyService.GetByIdAsync(1)
                .ConfigureAwait(false);

            Assert.Equal("Appling", county.CountyName);
        }

        [Fact]
        public async Task GetByNameReturnsCountyAsync()
        {
            ICountyService countyService = GetCountyService();

            CountyResource county = await countyService.GetByNameAsync("Appling")
                .ConfigureAwait(false);

            Assert.Equal(1, county.Id);
        }

        [Fact]
        public async Task GetByMissingIdReturnsNullAsync()
        {
            ICountyService countyService = GetCountyService();

            CountyResource county = await countyService.GetByIdAsync(0)
                .ConfigureAwait(false);

            Assert.Null(county);
        }

        [Fact]
        public async Task GetByMissingNameReturnsNullAsync()
        {
            ICountyService countyService = GetCountyService();

            CountyResource county = await countyService.GetByNameAsync("None")
                .ConfigureAwait(false);

            Assert.Null(county);
        }
    }
}
