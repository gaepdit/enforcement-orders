using Enfo.API.Controllers;
using Enfo.API.Resources;
using Enfo.API.Tests.Helpers;
using Enfo.Domain.Entities;
using Enfo.Infrastructure.SeedData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Enfo.API.Tests.ControllerTests
{
    public class CountiesControllerTests
    {
        private readonly County[] _allCounties;

        public CountiesControllerTests()
        {
            _allCounties = ProdSeedData.GetCounties();
        }

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

            var result = (await controller.Get(pageSize: 0).ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<CountyResource>;

            var expected = new CountyResource(_allCounties[0]);

            items.Should().HaveCount(_allCounties.Length);
            items.ToList()[0].Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetPaginatedReturnsSomeItemsAsync()
        {
            var repository = this.GetRepository<County>();
            var controller = new CountiesController(repository);

            int pageSize = 10;
            int pageNum = 2;
            int firstItemIndex = (pageNum - 1) * pageSize;

            var result = (await controller.Get(pageSize, pageNum).ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<CountyResource>;

            var expected = new CountyResource(_allCounties[firstItemIndex]);

            items.Should().HaveCount(pageSize);
            items.ToList()[0].Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task InvalidPageSizeReturnsDefaultPaginationAsync()
        {
            var repository = this.GetRepository<County>();
            var controller = new CountiesController(repository);

            var result = (await controller.Get(pageSize: -1)
                .ConfigureAwait(false))
                .Result as OkObjectResult;

            var expected = (await controller.Get()
                .ConfigureAwait(false))
                .Result as OkObjectResult;

            result.Should().BeEquivalentTo(expected);
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

            var expected = new CountyResource(Array.Find(_allCounties,
                e => e.Id == id));

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
