using Enfo.API.Controllers;
using Enfo.API.Resources;
using Enfo.API.Tests.Repositories;
using Enfo.Domain.Entities;
using Enfo.Infrastructure.Repositories;
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
        private readonly List<County> countiesList = new List<County>
        {
            new County { Id = 1, CountyName = "Appling" },
            new County { Id = 2, CountyName = "Atkinson" },
            new County { Id = 3, CountyName = "Bacon" }
        };

        [Fact]
        public async Task GetReturnsOkAsync()
        {
            ICountyRepository repository = new CountyRepositoryFake(countiesList);
            CountiesController controller = new CountiesController(repository);

            var result = (await controller.GetAllAsync()
                .ConfigureAwait(false))
                .Result;

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetReturnsCorrectTypeAsync()
        {
            ICountyRepository repository = new CountyRepositoryFake(countiesList);
            CountiesController controller = new CountiesController(repository);

            var result = (await controller.GetAllAsync()
                .ConfigureAwait(false))
                .Result as OkObjectResult;

            Assert.IsAssignableFrom<IEnumerable<CountyResource>>(result.Value);
        }

        [Fact]
        public async Task GetReturnsAllItemsAsync()
        {
            ICountyRepository repository = new CountyRepositoryFake(countiesList);
            CountiesController controller = new CountiesController(repository);

            var result = (await controller.GetAllAsync()
                .ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<CountyResource>;

            var expected = new CountyResource() { Id = 1, CountyName = "Appling", Active = true };

            items.Should().HaveCount(3);
            items.ToList()[0].Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetByIdReturnsCorrectTypeAsync()
        {
            ICountyRepository repository = new CountyRepositoryFake(countiesList);
            CountiesController controller = new CountiesController(repository);

            var value = (await controller.GetByIdAsync(1)
                .ConfigureAwait(false))
                .Value;

            value.Should().BeOfType<CountyResource>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetByIdReturnsCorrectItemAsync(int id)
        {
            ICountyRepository repository = new CountyRepositoryFake(countiesList);
            CountiesController controller = new CountiesController(repository);

            var value = (await controller.GetByIdAsync(id)
                .ConfigureAwait(false))
                .Value;

            value.Should().BeEquivalentTo(new CountyResource(countiesList.Find(e => e.Id == id)));
        }

        [Fact]
        public async Task GetByMissingIdReturnsNotFoundAsync()
        {
            ICountyRepository repository = new CountyRepositoryFake(countiesList);
            CountiesController controller = new CountiesController(repository);

            CountyResource result = (await controller.GetByIdAsync(0)
                .ConfigureAwait(false))
                .Value;

            result.Should().BeNull();
        }
    }
}
