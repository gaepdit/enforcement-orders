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

            var expected = new CountyResource
            {
                Id = 1,
                CountyName = "Appling",
                Active = true
            };

            items.Should().HaveCount(159);
            items.ToList()[0].Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetPaginatedReturnsSomeItemsAsync()
        {
            var repository = this.GetRepository<County>();
            var controller = new CountiesController(repository);

            var result = (await controller.Get(10, 2).ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<CountyResource>;

            var expected = new CountyResource
            {
                Id = 11,
                CountyName = "Bibb",
                Active = true
            };

            items.Should().HaveCount(10);
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

            CountyResource result = (await controller.Get(0).ConfigureAwait(false))
                .Value;

            result.Should().BeNull();
        }
    }
}
