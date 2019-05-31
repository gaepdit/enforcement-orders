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
        private ICountyService GetService([CallerMemberName] string dbName = null)
        {
            var options = new DbContextOptionsBuilder<EnfoDbContext>()
                .UseSqlite($"Data Source={dbName}.db")
                .Options;

            var context = new EnfoDbContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return new CountyService(context);
        }

        [Fact]
        public async Task GetAllCountiesReturnsListAsync()
        {
            ICountyService service = GetService();

            IEnumerable<CountyResource> items = await service.GetAllAsync()
                .ConfigureAwait(false);

            Assert.Equal(159, items.Count());
            Assert.Equal("Appling", items.First().CountyName);
        }

        [Fact]
        public async Task GetCountyByIdReturnsItemAsync()
        {
            ICountyService service = GetService();

            CountyResource item = await service.GetByIdAsync(1)
                .ConfigureAwait(false);

            Assert.Equal("Appling", item.CountyName);
        }

        [Fact]
        public async Task GetCountyByNameReturnsItemAsync()
        {
            ICountyService service = GetService();

            CountyResource item = await service.GetByNameAsync("Appling")
                .ConfigureAwait(false);

            Assert.Equal(1, item.Id);
        }

        [Fact]
        public async Task GetCountyByMissingIdReturnsNullAsync()
        {
            ICountyService service = GetService();

            CountyResource item = await service.GetByIdAsync(-1)
                .ConfigureAwait(false);

            Assert.Null(item);
        }

        [Fact]
        public async Task GetCountyByMissingNameReturnsNullAsync()
        {
            ICountyService service = GetService();

            CountyResource item = await service.GetByNameAsync("None")
                .ConfigureAwait(false);

            Assert.Null(item);
        }
    }
}
