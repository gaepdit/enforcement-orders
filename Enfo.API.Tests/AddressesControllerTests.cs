using Enfo.API.Controllers;
using Enfo.API.QueryStrings;
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
        public async Task GetDefaultReturnsOnePageOfActiveItems()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var items = ((await controller.Get()
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _allAddresses
                .OrderBy(e => e.Id)
                .Where(e => e.Active)
                .Take(PaginationFilter.DefaultPageSize)
                .Select(e => new AddressResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetReturnsAllActiveItems()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var items = ((await controller.Get(
                paging: new PaginationFilter() { PageSize = 0 })
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _allAddresses
                .Where(e => e.Active)
                .Select(e => new AddressResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetWithIncludeInactiveReturnsAllItems()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var items = ((await controller.Get(
                new ActiveItemFilter() { IncludeInactive = true },
                new PaginationFilter() { PageSize = 0 })
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _allAddresses
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

            var items = ((await controller.Get(
                paging: new PaginationFilter() { PageSize = pageSize, Page = pageNum })
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
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var result = (await controller.Get(
                paging: new PaginationFilter() { PageSize = 0 })
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

            result.Result.Should().BeOfType<NotFoundObjectResult>();
            result.Value.Should().BeNull();
            (result.Result as NotFoundObjectResult).StatusCode.Should().Be(404);
            (result.Result as NotFoundObjectResult).Value.Should().Be(id);
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

            result.Should().BeOfType<BadRequestObjectResult>();
            (result as BadRequestObjectResult).StatusCode.Should().Be(400);

            // Verify repository not changed after attempting to Post null item.
            var resultItems = ((await controller.Get(
                new ActiveItemFilter() { IncludeInactive = true },
                paging: new PaginationFilter() { PageSize = 0 })
                .ConfigureAwait(false))
                .Result as OkObjectResult).Value;

            var expected = _allAddresses
                .Select(e => new AddressResource(e));

            resultItems.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task AddInvalidItemFails()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var item = new AddressCreateResource()
            {
                City = null,
                PostalCode = "abc",
                State = "GA",
                Street = "123 Fake St"
            };

            controller.ModelState.AddModelError(nameof(item.City), "City is required");
            controller.ModelState.AddModelError(nameof(item.PostalCode), "Valid US Postal Code is required");

            var result = await controller.Post(item).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            var objectResult = (result as BadRequestObjectResult);
            objectResult.StatusCode.Should().Be(400);
            var objectResultValue = (objectResult.Value as Microsoft.AspNetCore.Mvc.SerializableError);
            objectResultValue.Count.Should().Be(2);
            objectResultValue.Keys.Should().BeEquivalentTo(new List<string>() { "City", "PostalCode" });

            // Verify repository not changed after attempting to Post invalid item.
            var resultItems = ((await controller.Get(
                new ActiveItemFilter() { IncludeInactive = true },
                paging: new PaginationFilter() { PageSize = 0 })
                .ConfigureAwait(false))
                .Result as OkObjectResult).Value;

            var expected = _allAddresses
                .Select(e => new AddressResource(e));

            resultItems.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task UpdateItemSucceeds()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var target = new AddressResource(
                new Address()
                {
                    Id = 2000,
                    City = "Atlanta",
                    PostalCode = "33333",
                    State = "GA",
                    Street = "123 Fake St"
                });

            var original = await repository.GetByIdAsync(target.Id)
                .ConfigureAwait(false);

            var result = await controller.Put(original.Id, target)
                .ConfigureAwait(false);

            result.Should().BeOfType<NoContentResult>();
            (result as NoContentResult).StatusCode.Should().Be(204);

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

            result.Should().BeOfType<BadRequestObjectResult>();
            (result as BadRequestObjectResult).StatusCode.Should().Be(400);

            var updated = await repository.GetByIdAsync(2000).ConfigureAwait(false);

            updated.Should().BeEquivalentTo(original);
        }

        [Fact]
        public async Task UpdateWithWrongIdFails()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var target = new AddressResource(
                new Address()
                {
                    Id = 9999,
                    City = "Atlanta",
                    PostalCode = "33333",
                    State = "GA",
                    Street = "123 Fake St"
                });

            var original = await repository.GetByIdAsync(2000).ConfigureAwait(false);

            var result = await controller.Put(original.Id, target).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            (result as BadRequestObjectResult).StatusCode.Should().Be(400);

            var updated = await repository.GetByIdAsync(original.Id).ConfigureAwait(false);

            updated.Should().BeEquivalentTo(original);
        }

        [Fact]
        public async Task UpdateWithMissingIdFails()
        {
            var repository = this.GetRepository<Address>();
            var controller = new AddressesController(repository);

            var target = new AddressResource(
                new Address()
                {
                    Id = 9999,
                    City = "Atlanta",
                    PostalCode = "33333",
                    State = "GA",
                    Street = "123 Fake St"
                });

            IActionResult result = await controller.Put(9999, target).ConfigureAwait(false);

            result.Should().BeOfType<NotFoundObjectResult>();
            (result as NotFoundObjectResult).StatusCode.Should().Be(404);
            (result as NotFoundObjectResult).Value.Should().Be(9999);
        }
    }
}
