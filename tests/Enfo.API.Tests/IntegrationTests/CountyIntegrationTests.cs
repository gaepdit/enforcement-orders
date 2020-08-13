using Enfo.API.Classes;
using Enfo.API.Controllers;
using Enfo.API.QueryStrings;
using Enfo.API.Resources;
using Enfo.API.Tests.Helpers;
using Enfo.Domain.Entities;
using Enfo.Infrastructure.SeedData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Enfo.API.Tests.IntegrationTests
{
    public class CountyIntegrationTests
    {
        private readonly County[] _counties;

        public CountyIntegrationTests()
        {
            _counties = ProdSeedData.GetCounties();
        }

        [Fact]
        public async Task GetReturnsCorrectly()
        {
            var repository = this.GetRepository<County>();
            var controller = new CountiesController(repository);

            var result = await controller.Get().ConfigureAwait(false);

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = result.Result as OkObjectResult;
            Assert.IsAssignableFrom<PaginatedList<CountyResource>>(actionResult.Value);
            actionResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetDefaultReturnsOnePageOfActiveItems()
        {
            var repository = this.GetRepository<County>();
            var controller = new CountiesController(repository);

            var items = (PaginatedList<CountyResource>)((await controller.Get()
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _counties
                .OrderBy(e => e.CountyName)
                .Take(PaginationFilter.DefaultPageSize)
                .Select(e => new CountyResource(e));

            items.Items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetReturnsAllItems()
        {
            var repository = this.GetRepository<County>();
            var controller = new CountiesController(repository);

            var items = (PaginatedList<CountyResource>)((await controller.Get(
                new PaginationFilter() { PageSize = 0 })
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _counties
                .Select(e => new CountyResource(e));

            items.Items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetPaginatedReturnsCorrectItems()
        {
            var repository = this.GetRepository<County>();
            var controller = new CountiesController(repository);

            int pageSize = 3;
            int pageNumber = 2;
            int firstItemIndex = (pageNumber - 1) * pageSize;

            var items = (PaginatedList<CountyResource>)((await controller.Get(
                new PaginationFilter() { PageSize = pageSize, Page = pageNumber })
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _counties
                .OrderBy(e => e.CountyName)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .Select(e => new CountyResource(e));

            items.Items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task InvalidPageSizeReturnsDefaultPagination()
        {
            var repository = this.GetRepository<County>();
            var controller = new CountiesController(repository);

            var result = (await controller.Get(
                paging: new PaginationFilter() { PageSize = -1 })
                .ConfigureAwait(false)).Result as OkObjectResult;

            var expected = (await controller.Get()
                .ConfigureAwait(false)).Result as OkObjectResult;

            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task InvalidPageNumberReturnsDefaultPagination()
        {
            var repository = this.GetRepository<County>();
            var controller = new CountiesController(repository);

            var result = (await controller.Get(
                paging: new PaginationFilter() { Page = 0 })
                .ConfigureAwait(false)).Result as OkObjectResult;

            var expected = (await controller.Get()
                .ConfigureAwait(false)).Result as OkObjectResult;

            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetByIdReturnsCorrectly()
        {
            var repository = this.GetRepository<County>();
            var controller = new CountiesController(repository);

            var result = await controller.Get(1).ConfigureAwait(false);

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = result.Result as OkObjectResult;
            actionResult.Value.Should().BeOfType<CountyResource>();
            actionResult.StatusCode.Should().Be(200);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetByIdReturnsCorrectItem(int id)
        {
            var repository = this.GetRepository<County>(id);
            var controller = new CountiesController(repository);

            var value = ((await controller.Get(id).ConfigureAwait(false))
                .Result as OkObjectResult).Value;

            var expected = new CountyResource(_counties
                .Single(e => e.Id == id));

            value.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetByMissingIdReturnsNotFound(int id)
        {
            var repository = this.GetRepository<County>();
            var controller = new CountiesController(repository);

            var result = await controller.Get(id).ConfigureAwait(false);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
            result.Value.Should().BeNull();
            (result.Result as NotFoundObjectResult).StatusCode.Should().Be(404);
            (result.Result as NotFoundObjectResult).Value.Should().Be(id);
        }
    }
}
