using Enfo.API.Controllers;
using Enfo.API.Resources;
using Enfo.API.Tests.Helpers;
using Enfo.Domain.Entities;
using Enfo.Infrastructure.SeedData;
using FluentAssertions;
using System;
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

        [Theory]
        [InlineData(140)]
        [InlineData(27)]
        [InlineData(71715)]
        public async Task GetDetailedByIdReturnsCorrectItemAsync(int id)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var value = (await controller.Details(id).ConfigureAwait(false))
                .Value;

            var expected = new EnforcementOrderDetailedResource(Array.Find(_allOrders,
                e => e.Id == id));

            value.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetDetailedByMissingIdReturnsNotFoundAsync(int id)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = (await controller.Details(id).ConfigureAwait(false))
                .Value;

            result.Should().BeNull();
        }
    }
}
