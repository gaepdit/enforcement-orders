using Enfo.Infrastructure.SeedData;
using Enfo.API.Controllers;
using Enfo.API.Resources;
using Enfo.API.Tests.Helpers;
using Enfo.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Enfo.API.Tests.ControllerTests
{
    public class CountiesControllerTests
    {
        [Fact]
        public async Task GetReturnsOkAsync()
        {
            var repository = this.GetRepository<County>();
            var controller = new CountiesController(repository);

            var result = (await controller.Get().ConfigureAwait(false))
                .Result;

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetReturnsCorrectTypeAsync()
        {
            var repository = this.GetRepository<County>();
            var controller = new CountiesController(repository);

            var result = (await controller.Get().ConfigureAwait(false))
                .Result as OkObjectResult;

            Assert.IsAssignableFrom<IEnumerable<CountyResource>>(result.Value);
        }

        [Fact]
        public async Task GetReturnsAllItemsAsync()
        {
            var repository = this.GetRepository<County>();
            var controller = new CountiesController(repository);

            var result = (await controller.Get().ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<CountyResource>;

            var originalList = SeedData.GetCounties();
            var expected = new CountyResource(originalList[0]);

            items.Should().HaveCount(originalList.Length);
            items.ToList()[0].Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetPaginatedReturnsSomeItemsAsync()
        {
            var repository = this.GetRepository<County>();
            var controller = new CountiesController(repository);

            int pageSize = 10;
            int pageNum = 2;
            int itemNum = (pageNum - 1) * pageSize + 1;

            var result = (await controller.Get(pageSize, pageNum).ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<CountyResource>;

            var originalList = SeedData.GetCounties();
            var expected = new CountyResource(originalList[itemNum - 1]);

            items.Should().HaveCount(pageSize);
            items.ToList()[0].Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetByIdReturnsCorrectTypeAsync()
        {
            var repository = this.GetRepository<County>();
            var controller = new CountiesController(repository);

            var value = (await controller.Get(1).ConfigureAwait(false))
                .Value;

            value.Should().BeOfType<CountyResource>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetByIdReturnsCorrectItemAsync(int id)
        {
            var repository = this.GetRepository<County>(id);
            var controller = new CountiesController(repository);

            var value = (await controller.Get(id).ConfigureAwait(false))
                .Value;

            var expected = new CountyResource(await repository.GetByIdAsync(id).ConfigureAwait(false));

            value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetByMissingIdReturnsNotFoundAsync()
        {
            var repository = this.GetRepository<County>();
            var controller = new CountiesController(repository);

            var result = (await controller.Get(0).ConfigureAwait(false))
                .Value;

            result.Should().BeNull();
        }
    }
}
