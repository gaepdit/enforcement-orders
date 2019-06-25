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
    public class EpdContactControllerTests
    {
        [Fact]
        public async Task GetReturnsOkAsync()
        {
            var repository = this.GetRepository<EpdContact>();
            var controller = new EpdContactsController(repository);

            var result = (await controller.Get().ConfigureAwait(false))
                .Result;

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetReturnsCorrectTypeAsync()
        {
            var repository = this.GetRepository<EpdContact>();
            var controller = new EpdContactsController(repository);

            var result = (await controller.Get().ConfigureAwait(false))
                .Result as OkObjectResult;

            Assert.IsAssignableFrom<IEnumerable<EpdContactResource>>(result.Value);
        }

        [Fact]
        public async Task GetReturnsAllItemsAsync()
        {
            var repository = this.GetRepository<EpdContact>();
            var controller = new EpdContactsController(repository);

            var result = (await controller.Get().ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<EpdContactResource>;

            var expectedAddress = new AddressResource { Id = 2000, Active = true, City = "Atlanta", PostalCode = "30354", State = "GA", Street = "4244 International Parkway", Street2 = "Suite 120" };
            var expectedContact = new EpdContactResource { Id = 2000, Active = false, Address = expectedAddress, AddressId = 2000, ContactName = "Mr. Keith M. Bentley", Email = "null", Organization = "Environmental Protection Division", Title = "Chief, Air Protection Branch" };

            items.Should().HaveCount(3);
            items.ToList()[0].Should().BeEquivalentTo(expectedContact);
        }

        [Fact]
        public async Task GetByIdReturnsCorrectTypeAsync()
        {
            var repository = this.GetRepository<EpdContact>();
            var controller = new EpdContactsController(repository);

            var value = (await controller.Get(2000).ConfigureAwait(false))
                .Value;

            value.Should().BeOfType<EpdContactResource>();
        }

        [Theory]
        [InlineData(2000)]
        [InlineData(2001)]
        [InlineData(2002)]
        public async Task GetByIdReturnsCorrectItemAsync(int id)
        {
            var repository = this.GetRepository<EpdContact>();
            var controller = new EpdContactsController(repository);

            var value = (await controller.Get(id).ConfigureAwait(false))
                .Value;

            var expected = new EpdContactResource(await repository.GetByIdAsync(id).ConfigureAwait(false));

            value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetByMissingIdReturnsNotFoundAsync()
        {
            var repository = this.GetRepository<EpdContact>();
            var controller = new EpdContactsController(repository);

            EpdContactResource result = (await controller.Get(0).ConfigureAwait(false))
                .Value;

            result.Should().BeNull();
        }

        [Fact]
        public async Task AddNewItemReturnsCorrectly()
        {
            var repository = this.GetRepository<EpdContact>();
            var controller = new EpdContactsController(repository);

            var newAddress = new AddressResource { City = "Atlanta", PostalCode = "33333", State = "GA", Street = "123 Fake St" };
            var newContact = new EpdContactResource { Address = newAddress, ContactName = "Mr. Fake Name", Email = "fake.name@example.com", Organization = "Environmental Protection Division", Title = "" };

            var postResult = controller.Post(newContact).ConfigureAwait(false);
            var result = await postResult;

            result.Should().BeOfType<CreatedAtActionResult>();
            (result as CreatedAtActionResult).ActionName.Should().Be("Get");
        }

        [Fact]
        public async Task AddNewItemCorrectlyAdds()
        {
            var repository = this.GetRepository<EpdContact>();
            var controller = new EpdContactsController(repository);

            var newAddress = new AddressResource { City = "Atlanta", PostalCode = "33333", State = "GA", Street = "123 Fake St" };
            var newContact = new EpdContactResource { Address = newAddress, ContactName = "Mr. Fake Name", Email = "fake.name@example.com", Organization = "Environmental Protection Division", Title = "" };

            await controller.Post(newContact).ConfigureAwait(false);

            EpdContactResource addedItem = (await controller.Get(2003).ConfigureAwait(false))
                .Value;

            // Contact and address ID get added with next value in DB
            newContact.Id = 2003;
            newContact.AddressId = 2003;
            newContact.Address.Id = 2003;

            addedItem.Should().BeEquivalentTo(newContact);
        }

        [Fact]
        public async Task UpdateItem()
        {
            var repository = this.GetRepository<EpdContact>();
            var controller = new EpdContactsController(repository);

            var target = new EpdContactResource { Id = 2000, AddressId = 2002, ContactName = "Name One Update", Email = "email@example.com", Organization = "Com", Telephone = "555-1212", Title = "Title" };

            var original = await repository.GetByIdAsync(target.Id).ConfigureAwait(false);

            IActionResult result = await controller.Put(original.Id, target).ConfigureAwait(false);

            result.Should().BeOfType<OkObjectResult>();
            (result as OkObjectResult).StatusCode.Should().Be(200);

            // Contact can be saved with null address if address ID is present,
            // but will be returned from database with address entity included
            var addressRepository = this.GetRepository<Address>();
            target.Address = new AddressResource(await addressRepository.GetByIdAsync(2002).ConfigureAwait(false));

            var updated = await repository.GetByIdAsync(target.Id).ConfigureAwait(false);

            updated.Should().BeEquivalentTo(target);
        }
    }
}
