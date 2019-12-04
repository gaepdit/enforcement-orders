using Enfo.API.Controllers;
using Enfo.API.Resources;
using Enfo.API.Tests.Helpers;
using Enfo.Domain.Entities;
using Enfo.Infrastructure.SeedData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Enfo.API.Tests.ControllerTests
{
    public class AddressesControllerTests
    {
        private readonly Address[] _allAddresses;

        public AddressesControllerTests()
        {
            _allAddresses = ProdSeedData.GetAddresses();
        }

        [Fact]
        public async Task GetReturnsCorrectly()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var result = await controller.Get().ConfigureAwait(false);

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = result.Result as OkObjectResult;
            Assert.IsAssignableFrom<IEnumerable<AddressResource>>(actionResult.Value);
            actionResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetReturnsAllActiveItems()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var items = ((await controller.Get(pageSize: 0)
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _allAddresses
                .OrderBy(e => e.Id)
                .Where(e => e.Active)
                .Select(e => new AddressResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetWithIncludeInactiveReturnsAllItems()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var items = ((await controller.Get(includeInactive: true, pageSize: 0)
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _allAddresses
                .OrderBy(e => e.Id)
                .Select(e => new AddressResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetPaginatedReturnsCorrectItems()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            int pageSize = 3;
            int pageNum = 2;

            var items = ((await controller.Get(pageSize, pageNum)
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _allAddresses
                .OrderBy(e => e.Id)
                .Where(e => e.Active)
                .Skip((pageNum - 1) * pageSize).Take(pageSize)
                .Select(e => new AddressResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task InvalidPageSizeReturnsDefaultPagination()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var result = (await controller.Get(pageSize: -1)
                .ConfigureAwait(false)).Result as OkObjectResult;

            var expected = (await controller.Get()
                .ConfigureAwait(false)).Result as OkObjectResult;

            result.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public async Task InvalidPageNumberReturnsDefaultPagination()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var result = (await controller.Get(page: 0)
                .ConfigureAwait(false)).Result as OkObjectResult;

            var expected = (await controller.Get()
                .ConfigureAwait(false)).Result as OkObjectResult;

            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetByIdReturnsCorrectly()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var result = await controller.Get(2000).ConfigureAwait(false);

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = result.Result as OkObjectResult;
            actionResult.Value.Should().BeOfType<AddressResource>();
            actionResult.StatusCode.Should().Be(200);
        }

        [Theory]
        [InlineData(2000)]
        [InlineData(2001)]
        [InlineData(2002)]
        public async Task GetByIdReturnsCorrectItem(int id)
        {
            var repository = this.GetRepository<Address>(id);
            var controller = new AddressesController(repository);

            var value = ((await controller.Get(id).ConfigureAwait(false))
                .Result as OkObjectResult).Value;

            var expected = new AddressResource(_allAddresses
                .Single(e => e.Id == id));

            value.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetByMissingIdReturnsNotFound(int id)
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var result = await controller.Get(id).ConfigureAwait(false);

            result.Result.Should().BeOfType<NotFoundResult>();
            result.Value.Should().BeNull();
            (result.Result as NotFoundResult).StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task AddNewItemReturnsCorrectly()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var item = new AddressCreateResource()
            {
                City = "Atlanta",
                PostalCode = "33333",
                State = "GA",
                Street = "123 Fake St"
            };

            var result = await controller.Post(item).ConfigureAwait(false);

            result.Should().BeOfType<CreatedAtActionResult>();
            var actionResult = result as CreatedAtActionResult;
            actionResult.ActionName.Should().Be("Get");
            actionResult.StatusCode.Should().Be(201);
            actionResult.Value.Should().BeOfType<int>();
        }

        [Fact]
        public async Task AddNewItemIsAddedCorrectly()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var item = new AddressCreateResource()
            {
                City = "Atlanta",
                PostalCode = "33333",
                State = "GA",
                Street = "123 Fake St"
            };

            var result = await controller.Post(item).ConfigureAwait(false);

            var id = (int)(result as CreatedAtActionResult).Value;
            var addedItem = new AddressResource(await repository.GetByIdAsync(id)
                .ConfigureAwait(false));

            // Item gets added with next value in DB
            var expected = new AddressResource(item.NewAddress())
            {
                Id = _allAddresses.Max(e => e.Id) + 1
            };

            addedItem.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task AddNullItemFails()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var result = await controller.Post(null).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestResult>();
            (result as BadRequestResult).StatusCode.Should().Be(400);

            // Verify repository not changed after attempting to Post null item.
            var resultItems = ((await controller.Get(includeInactive: true,
                pageSize: 0).ConfigureAwait(false))
                .Result as OkObjectResult).Value;

            var expected = _allAddresses
                .OrderBy(e => e.Id)
                .Select(e => new AddressResource(e));

            resultItems.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task UpdateItemSucceeds()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var target = new AddressResource()
            {
                Id = 2000,
                City = "Atlanta",
                PostalCode = "33333",
                State = "GA",
                Street = "123 Fake St"
            };

            var original = await repository.GetByIdAsync(target.Id)
                .ConfigureAwait(false);

            var result = await controller.Put(original.Id, target)
                .ConfigureAwait(false);

            result.Should().BeOfType<OkObjectResult>();
            (result as OkObjectResult).StatusCode.Should().Be(200);

            var updated = await repository.GetByIdAsync(target.Id)
                .ConfigureAwait(false);

            updated.Should().BeEquivalentTo(target);
        }

        [Fact]
        public async Task UpdateWithNullFails()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var original = await repository.GetByIdAsync(2000).ConfigureAwait(false);

            var result = await controller.Put(original.Id, null).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestResult>();
            (result as BadRequestResult).StatusCode.Should().Be(400);

            var updated = await repository.GetByIdAsync(2000).ConfigureAwait(false);

            updated.Should().BeEquivalentTo(original);
        }

        [Fact]
        public async Task UpdateWithWrongIdFails()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var target = new AddressResource()
            {
                Id = 9999,
                City = "Atlanta",
                PostalCode = "33333",
                State = "GA",
                Street = "123 Fake St"
            };

            var original = await repository.GetByIdAsync(2000).ConfigureAwait(false);

            var result = await controller.Put(original.Id, target).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestResult>();
            (result as BadRequestResult).StatusCode.Should().Be(400);

            var updated = await repository.GetByIdAsync(original.Id).ConfigureAwait(false);

            updated.Should().BeEquivalentTo(original);
        }
    }
}
