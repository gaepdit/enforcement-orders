using Enfo.API.Controllers;
using Enfo.API.Tests.ServiceFakes;
using Enfo.Models.Resources;
using Enfo.Models.Services;
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

            Assert.IsType<OkObjectResult>(result);
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

            Assert.Equal(3, items.Count());
            Assert.Equal("Appling", items.First().CountyName);
        }

        [Fact]
        public async Task GetByIdReturnsCorrectTypeAsync()
        {
            ICountyService countyService = new CountyServiceFake(countiesList);
            CountiesController controller = new CountiesController(countyService);

            var value = (await controller.GetByIdAsync(1)
                .ConfigureAwait(false))
                .Value;

            Assert.IsType<CountyResource>(value);
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

            Assert.Equal(countiesList.Find(item => item.Id == id), value);
        }

        [Fact]
        public async Task GetByNameReturnsCorrectTypeAsync()
        {
            ICountyService countyService = new CountyServiceFake(countiesList);
            CountiesController controller = new CountiesController(countyService);

            var value = (await controller.GetByNameAsync("Appling")
                .ConfigureAwait(false))
                .Value;

            Assert.IsType<CountyResource>(value);
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

            Assert.Equal(countiesList.Find(e => e.CountyName == name), value);
        }

        [Fact]
        public async Task GetByMissingIdReturnsNotFoundAsync()
        {
            ICountyService countyService = new CountyServiceFake(countiesList);
            CountiesController controller = new CountiesController(countyService);

            var result = (await controller.GetByIdAsync(0)
                .ConfigureAwait(false))
                .Value;

            Assert.Null(result);
        }

        [Fact]
        public async Task GetByMissingNameReturnsNotFoundAsync()
        {
            ICountyService countyService = new CountyServiceFake(countiesList);
            CountiesController controller = new CountiesController(countyService);

            var result = (await controller.GetByNameAsync("None")
                .ConfigureAwait(false))
                .Value;

            Assert.Null(result);
        }
    }
}
