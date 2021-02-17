using Enfo.API.Classes;
using Enfo.API.Controllers;
using Enfo.API.QueryStrings;
using Enfo.API.Tests.Helpers;
using Enfo.Domain.Entities;
using Enfo.Repository.Querying;
using Enfo.Infrastructure.SeedData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Enfo.Repository.Resources;
using Xunit;

namespace Enfo.API.Tests.IntegrationTests
{
    public class EpdContactIntegrationTests
    {
        private readonly EpdContact[] _epdContacts;
        private readonly Address[] _addresses;

        public EpdContactIntegrationTests()
        {
            _addresses = ProdSeedData.GetAddresses();
            _epdContacts = ProdSeedData.GetEpdContacts();

            foreach (var contact in _epdContacts)
            {
                contact.Address = _addresses
                    .SingleOrDefault(e => e.Id == contact.AddressId);
            }
        }

        [Fact]
        public async Task GetReturnsCorrectly()
        {
            var repository = this.GetRepository<EpdContact>();
            var controller = new EpdContactsController(repository);

            var result = await controller.Get().ConfigureAwait(false);

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = (result.Result as OkObjectResult);
            Assert.IsAssignableFrom<PaginatedList<EpdContactResource>>(actionResult.Value);
            actionResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetDefaultReturnsOnePageOfActiveItems()
        {
            var repository = this.GetRepository<EpdContact>();
            var controller = new EpdContactsController(repository);

            var items = (PaginatedList<EpdContactResource>)((await controller.Get()
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _epdContacts
                .OrderBy(e => e.Id)
                .Where(e => e.Active)
                .Take(PaginationFilter.DefaultPageSize)
                .Select(e => new EpdContactResource(e));

            items.Items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetReturnsAllActiveItems()
        {
            var repository = this.GetRepository<EpdContact>();
            var controller = new EpdContactsController(repository);

            var items = (PaginatedList<EpdContactResource>)((await controller.Get(
                paging: new PaginationFilter() { PageSize = 0 })
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _epdContacts
                .Where(e => e.Active)
                .Select(e => new EpdContactResource(e));

            items.Items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetWithIncludeInactiveReturnsAllItems()
        {
            var repository = this.GetRepository<EpdContact>();
            var controller = new EpdContactsController(repository);

            var items = (PaginatedList<EpdContactResource>)((await controller.Get(
                new ActiveItemFilter() { IncludeInactive = true },
                new PaginationFilter() { PageSize = 0 })
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _epdContacts
                .Select(e => new EpdContactResource(e));

            items.Items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetPaginatedReturnsCorrectItems()
        {
            var repository = this.GetRepository<EpdContact>();
            var controller = new EpdContactsController(repository);

            int pageSize = 3;
            int pageNumber = 2;

            var items = (PaginatedList<EpdContactResource>)((await controller.Get(
                paging: new PaginationFilter() { PageSize = pageSize, Page = pageNumber })
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _epdContacts
                .OrderBy(e => e.Id)
                .Where(e => e.Active)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .Select(e => new EpdContactResource(e));

            items.Items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task InvalidPageSizeReturnsDefaultPagination()
        {
            var repository = this.GetRepository<EpdContact>();
            var controller = new EpdContactsController(repository);

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
            var repository = this.GetRepository<EpdContact>();
            var controller = new EpdContactsController(repository);

            var result = (await controller.Get(
                paging: new PaginationFilter() { Page = 0 })
                .ConfigureAwait(false)).Result as OkObjectResult;

            var expected = (await controller.Get()
                .ConfigureAwait(false)).Result as OkObjectResult;

            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetByIdReturnsCorrectly()
        {
            var repository = this.GetRepository<EpdContact>();
            var controller = new EpdContactsController(repository);

            var result = await controller.Get(2000).ConfigureAwait(false);

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = result.Result as OkObjectResult;
            actionResult.Value.Should().BeOfType<EpdContactResource>();
            actionResult.StatusCode.Should().Be(200);
        }

        [Theory]
        [InlineData(2000)]
        [InlineData(2001)]
        [InlineData(2002)]
        public async Task GetByIdReturnsCorrectItem(int id)
        {
            var repository = this.GetRepository<EpdContact>();
            var controller = new EpdContactsController(repository);

            var value = ((await controller.Get(id).ConfigureAwait(false))
                .Result as OkObjectResult).Value as EpdContactResource;

            var expected = new EpdContactResource(_epdContacts
                .Single(e => e.Id == id));

            value.Should().BeEquivalentTo(expected);
            value.Address.Should().NotBeNull().And.BeEquivalentTo(expected.Address);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetByMissingIdReturnsNotFound(int id)
        {
            var repository = this.GetRepository<EpdContact>();
            var controller = new EpdContactsController(repository);

            var result = await controller.Get(id).ConfigureAwait(false);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
            result.Value.Should().BeNull();
            (result.Result as NotFoundObjectResult).StatusCode.Should().Be(404);
            (result.Result as NotFoundObjectResult).Value.Should().Be(id);
        }

        [Fact]
        public async Task AddNewItemReturnsCorrectly()
        {
            var repository = this.GetRepository<EpdContact>();
            var controller = new EpdContactsController(repository);

            var item = new EpdContactCreateResource
            {
                AddressId = 2000,
                ContactName = "Mr. Fake Name",
                Email = "fake.name@example.com",
                Organization = "Environmental Protection Division",
                Title = "Ombudsman"
            };

            var result = await controller.Post(item).ConfigureAwait(false);

            result.Should().BeOfType<CreatedAtActionResult>();
            var actionResult = result as CreatedAtActionResult;
            actionResult.ActionName.Should().Be("Get");
            actionResult.StatusCode.Should().Be(201);
            actionResult.Value.Should().BeOfType<int>();
        }

        [Fact]
        public async Task AddNewItemCorrectlyAdds()
        {
            var repository = this.GetRepository<EpdContact>();
            var controller = new EpdContactsController(repository);

            var item = new EpdContactCreateResource
            {
                AddressId = 2000,
                ContactName = "Mr. Fake Name",
                Email = "fake.name@example.com",
                Organization = "Environmental Protection Division",
                Title = "Ombudsman"
            };

            var result = await controller.Post(item).ConfigureAwait(false);

            var id = (int)(result as CreatedAtActionResult).Value;
            var addedItem = await repository
                .GetByIdAsync(id, inclusion: new EpdContactIncludingAddress())
                .ConfigureAwait(false);

            // Item gets added with next value in DB
            var expected = item.NewEpdContact();
            expected.Id = _epdContacts.Max(e => e.Id) + 1;
            expected.Address = _addresses.Single(e => e.Id == item.AddressId);

            addedItem.Should().BeEquivalentTo(expected);

            // Verify repository has changed.
            var resultItems = await repository.ListAsync().ConfigureAwait(false);

            resultItems.Count.Should().Be(_epdContacts.Count() + 1);
        }

        [Fact]
        public async Task AddNullItemFails()
        {
            var repository = this.GetRepository<EpdContact>();
            var controller = new EpdContactsController(repository);

            var result = await controller.Post(null).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            (result as BadRequestObjectResult).StatusCode.Should().Be(400);

            // Verify repository not changed after attempting to Post null item.
            var resultItems = (PaginatedList<EpdContactResource>)((await controller.Get(
                new ActiveItemFilter() { IncludeInactive = true },
                new PaginationFilter() { PageSize = 0 })
                .ConfigureAwait(false))
                .Result as OkObjectResult).Value;

            var expected = _epdContacts
                .Select(e => new EpdContactResource(e));

            resultItems.Items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task UpdateItemSucceeds()
        {
            var repository = this.GetRepository<EpdContact>();
            var controller = new EpdContactsController(repository);

            var id = 2000;
            var target = new EpdContactUpdateResource
            {
                AddressId = 2002,
                ContactName = "Name Update",
                Email = "email@example.com",
                Organization = "Com",
                Telephone = "555-1212",
                Title = "Title"
            };

            var result = await controller.Put(id, target)
                .ConfigureAwait(false);

            result.Should().BeOfType<NoContentResult>();
            (result as NoContentResult).StatusCode.Should().Be(204);

            var updated = await repository.GetByIdAsync(id)
                .ConfigureAwait(false);

            updated.Should().BeEquivalentTo(target);
        }

        [Fact]
        public async Task UpdateWithNullFails()
        {
            var repository = this.GetRepository<EpdContact>();
            var controller = new EpdContactsController(repository);

            var original = await repository.GetByIdAsync(2000).ConfigureAwait(false);

            var result = await controller.Put(original.Id, null).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            (result as BadRequestObjectResult).StatusCode.Should().Be(400);

            var updated = await repository.GetByIdAsync(2000).ConfigureAwait(false);

            updated.Should().BeEquivalentTo(original);
        }

        [Fact]
        public async Task UpdateWithMissingIdFails()
        {
            var repository = this.GetRepository<EpdContact>();
            var controller = new EpdContactsController(repository);

            var target = new EpdContactUpdateResource
            {
                AddressId = 2002,
                ContactName = "Name Update",
                Email = "email@example.com",
                Organization = "Com",
                Telephone = "555-1212",
                Title = "Title"
            };

            var result = await controller.Put(9999, target).ConfigureAwait(false);

            result.Should().BeOfType<NotFoundObjectResult>();
            (result as NotFoundObjectResult).StatusCode.Should().Be(404);
            (result as NotFoundObjectResult).Value.Should().Be(9999);
        }
    }
}
