using Enfo.Infrastructure.Contexts;
using Enfo.Infrastructure.Services;
using Enfo.Domain.Resources;
using Enfo.Domain.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;

namespace Enfo.Infrastructure.Tests.ServiceTests
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

            var expected = new CountyResource() { Id = 1, CountyName = "Appling" };

            items.Should().HaveCount(159);            
            items.First().Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetCountyByIdReturnsItemAsync()
        {
            ICountyService service = GetService();

            CountyResource item = await service.GetByIdAsync(1)
                .ConfigureAwait(false);

            var expected = new CountyResource() { Id = 1, CountyName = "Appling" };

            item.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetCountyByNameReturnsItemAsync()
        {
            ICountyService service = GetService();

            CountyResource item = await service.GetByNameAsync("Appling")
                .ConfigureAwait(false);

            var expected = new CountyResource() { Id = 1, CountyName = "Appling" };

            item.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetCountyByMissingIdReturnsNullAsync()
        {
            ICountyService service = GetService();

            CountyResource item = await service.GetByIdAsync(-1)
                .ConfigureAwait(false);

            item.Should().BeNull();
        }

        [Fact]
        public async Task GetCountyByMissingNameReturnsNullAsync()
        {
            ICountyService service = GetService();

            CountyResource item = await service.GetByNameAsync("None")
                .ConfigureAwait(false);

            item.Should().BeNull();
        }
    }
}
