using Enfo.API.Controllers;
using Enfo.API.Resources;
using Enfo.API.Tests.Helpers;
using Enfo.Domain.Entities;
using Enfo.Infrastructure.SeedData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Enfo.API.Tests.ControllerTests
{
    public class EnforcementOrdersControllerDetailsTests
    {
        private readonly EnforcementOrder[] _allOrders;

        public EnforcementOrdersControllerDetailsTests()
        {
            _allOrders = DevSeedData.GetEnforcementOrders();
        }

        [Fact]
        public async Task GetByIdReturnsCorrectly()
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = await controller.Details(140).ConfigureAwait(false);

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = result.Result as OkObjectResult;
            actionResult.Value.Should().BeOfType<EnforcementOrderDetailedResource>();
            actionResult.StatusCode.Should().Be(200);
        }

        [Theory]
        [InlineData(140)]
        [InlineData(27)]
        [InlineData(71715)]
        public async Task GetDetailedByIdReturnsCorrectItem(int id)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var value = ((await controller.Details(id).ConfigureAwait(false))
                .Result as OkObjectResult).Value;

            var expected = new EnforcementOrderDetailedResource(
                _allOrders.Single(e => e.Id == id));

            value.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetDetailedByMissingIdReturnsNotFound(int id)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = await controller.Get(id).ConfigureAwait(false);

            result.Result.Should().BeOfType<NotFoundResult>();
            result.Value.Should().BeNull();
            (result.Result as NotFoundResult).StatusCode.Should().Be(404);
        }
    }
}
