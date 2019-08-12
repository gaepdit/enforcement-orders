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
    public class AddressControllerTests
    {
        [Fact]
        public async Task GetReturnsOkAsync()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var result = (await controller.Get().ConfigureAwait(false))
                .Result;

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetReturnsCorrectTypeAsync()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var result = (await controller.Get().ConfigureAwait(false))
                .Result as OkObjectResult;

            Assert.IsAssignableFrom<IEnumerable<AddressResource>>(result.Value);
        }

        [Fact]
        public async Task GetReturnsAllActiveItemsAsync()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var result = (await controller.Get().ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<AddressResource>;

            var expected = new AddressResource
            {
                Id = 2000,
                Active = true,
                City = "Atlanta",
                PostalCode = "30354",
                State = "GA",
                Street = "4244 International Parkway",
                Street2 = "Suite 120"
            };

            items.Should().HaveCount(2);
            items.ToList()[0].Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetWithInactiveReturnsAllItemsAsync()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var result = (await controller.Get(includeInactive: true).ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<AddressResource>;

            var expected = new AddressResource
            {
                Id = 2000,
                Active = true,
                City = "Atlanta",
                PostalCode = "30354",
                State = "GA",
                Street = "4244 International Parkway",
                Street2 = "Suite 120"
            };

            items.Should().HaveCount(3);
            items.ToList()[0].Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetByIdReturnsCorrectTypeAsync()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var value = (await controller.Get(2000).ConfigureAwait(false))
                .Value;

            value.Should().BeOfType<AddressResource>();
        }

        [Theory]
        [InlineData(2000)]
        [InlineData(2001)]
        [InlineData(2002)]
        public async Task GetByIdReturnsCorrectItemAsync(int id)
        {
            var repository = this.GetRepository<Address>(id);
            var controller = new AddressesController(repository);

            var value = (await controller.Get(id).ConfigureAwait(false))
                .Value;

            var expected = new AddressResource(await repository.GetByIdAsync(id).ConfigureAwait(false));

            value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetByMissingIdReturnsNotFoundAsync()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var result = (await controller.Get(0).ConfigureAwait(false))
                .Value;

            result.Should().BeNull();
        }

        [Fact]
        public async Task AddNewItemReturnsCorrectly()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var item = new AddressCreateResource() { City = "Atlanta", PostalCode = "33333", State = "GA", Street = "123 Fake St" };

            var result = await controller.Post(item).ConfigureAwait(false);

            result.Should().BeOfType<CreatedAtActionResult>();
            (result as CreatedAtActionResult).ActionName.Should().Be("Get");
        }

        [Fact]
        public async Task AddNewItemIsAddedCorrectly()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var item = new AddressCreateResource() { City = "Atlanta", PostalCode = "33333", State = "GA", Street = "123 Fake St" };

            var result = await controller.Post(item).ConfigureAwait(false);

            var id = (int)(result as CreatedAtActionResult).Value;
            var addedItem = new AddressResource(await repository.GetByIdAsync(id).ConfigureAwait(false));

            var expected = new AddressResource() { Id = 2003, Active = true, City = "Atlanta", PostalCode = "33333", State = "GA", Street = "123 Fake St", Street2 = null };

            addedItem.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task UpdateItemSucceeds()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var target = new AddressResource() { Id = 2000, City = "Atlanta", PostalCode = "33333", State = "GA", Street = "123 Fake St" };

            var original = await repository.GetByIdAsync(target.Id).ConfigureAwait(false);

            IActionResult result = await controller.Put(original.Id, target).ConfigureAwait(false);

            result.Should().BeOfType<OkObjectResult>();
            (result as OkObjectResult).StatusCode.Should().Be(200);

            var updated = await repository.GetByIdAsync(target.Id).ConfigureAwait(false);

            updated.Should().BeEquivalentTo(target);
        }
    }
}
