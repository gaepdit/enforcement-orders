using Enfo.API.Controllers;
using Enfo.API.Tests.ServiceFakes;
using Enfo.Models.Resources;
using Enfo.Models.Services;
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
        private readonly List<CountyResource> countiesList = new List<CountyResource>
        {
            new CountyResource { Id = 1, CountyName = "Appling" },
            new CountyResource { Id = 2, CountyName = "Atkinson" },
            new CountyResource { Id = 3, CountyName = "Bacon" }
        };

        [Fact]
        public async Task GetReturnsOkAsync()
        {
            ICountyService countyService = new CountyServiceFake(countiesList);
            CountiesController controller = new CountiesController(countyService);

            var result = (await controller.GetAllAsync()
                .ConfigureAwait(false))
                .Result;

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetReturnsCorrectTypeAsync()
        {
            ICountyService countyService = new CountyServiceFake(countiesList);
            CountiesController controller = new CountiesController(countyService);

            var result = (await controller.GetAllAsync()
                .ConfigureAwait(false))
                .Result as OkObjectResult;

            Assert.IsAssignableFrom<IEnumerable<CountyResource>>(result.Value);
        }

        [Fact]
        public async Task GetReturnsAllItemsAsync()
        {
            ICountyService countyService = new CountyServiceFake(countiesList);
            CountiesController controller = new CountiesController(countyService);

            var result = (await controller.GetAllAsync()
                .ConfigureAwait(false))
                .Result as OkObjectResult;
            var items = result.Value as IEnumerable<CountyResource>;

            var expected = new CountyResource() { Id = 1, CountyName = "Appling" };

            items.Should().HaveCount(3);
            items.First().Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetByIdReturnsCorrectTypeAsync()
        {
            ICountyService countyService = new CountyServiceFake(countiesList);
            CountiesController controller = new CountiesController(countyService);

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
            ICountyService countyService = new CountyServiceFake(countiesList);
            CountiesController controller = new CountiesController(countyService);

            var value = (await controller.GetByIdAsync(id)
                .ConfigureAwait(false))
                .Value as CountyResource;

            value.Should().Be(countiesList.Find(e => e.Id == id));
        }

        [Fact]
        public async Task GetByNameReturnsCorrectTypeAsync()
        {
            ICountyService countyService = new CountyServiceFake(countiesList);
            CountiesController controller = new CountiesController(countyService);

            var value = (await controller.GetByNameAsync("Appling")
                .ConfigureAwait(false))
                .Value;

            value.Should().BeOfType<CountyResource>();
        }

        [Theory]
        [InlineData("Appling")]
        [InlineData("Atkinson")]
        [InlineData("Bacon")]
        public async Task GetByNameReturnsCorrectItemAsync(string name)
        {
            ICountyService countyService = new CountyServiceFake(countiesList);
            CountiesController controller = new CountiesController(countyService);

            var value = (await controller.GetByNameAsync(name)
                .ConfigureAwait(false))
                .Value as CountyResource;

            value.Should().Be(countiesList.Find(e => e.CountyName == name));
        }

        [Fact]
        public async Task GetByMissingIdReturnsNotFoundAsync()
        {
            ICountyService countyService = new CountyServiceFake(countiesList);
            CountiesController controller = new CountiesController(countyService);

            var result = (await controller.GetByIdAsync(0)
                .ConfigureAwait(false))
                .Value;

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByMissingNameReturnsNotFoundAsync()
        {
            ICountyService countyService = new CountyServiceFake(countiesList);
            CountiesController controller = new CountiesController(countyService);

            var result = (await controller.GetByNameAsync("None")
                .ConfigureAwait(false))
                .Value;

            result.Should().BeNull();
        }
    }
}
