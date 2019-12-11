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
    public class LegalAuthoritiesControllerTests
    {
        private readonly LegalAuthority[] _legalAuthorities;

        public LegalAuthoritiesControllerTests()
        {
            _legalAuthorities = ProdSeedData.GetLegalAuthorities();
        }

        [Fact]
        public async Task GetReturnsCorrectly()
        {
            var repository = this.GetRepository<LegalAuthority>();
            var controller = new LegalAuthoritiesController(repository);

            var result = await controller.Get().ConfigureAwait(false);

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = (result.Result as OkObjectResult);
            Assert.IsAssignableFrom<IEnumerable<LegalAuthorityResource>>(actionResult.Value);
            actionResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetDefaultReturnsOnePageOfActiveItems()
        {
            var repository = this.GetRepository<LegalAuthority>();
            var controller = new LegalAuthoritiesController(repository);

            var items = ((await controller.Get()
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _legalAuthorities
                .OrderBy(e => e.Id)
                .Where(e => e.Active)
                .Take(PaginationFilter.DefaultPageSize)
                .Select(e => new LegalAuthorityResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetReturnsAllActiveItems()
        {
            var repository = this.GetRepository<LegalAuthority>();
            var controller = new LegalAuthoritiesController(repository);

            var items = ((await controller.Get(
                paging: new PaginationFilter() { PageSize = 0 })
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _legalAuthorities
                .Where(e => e.Active)
                .Select(e => new LegalAuthorityResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetWithInactiveReturnsAllItems()
        {
            var repository = this.GetRepository<LegalAuthority>();
            var controller = new LegalAuthoritiesController(repository);

            var items = ((await controller.Get(
                new ActiveItemFilter() { IncludeInactive = true },
                new PaginationFilter() { PageSize = 0 })
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _legalAuthorities
                .Select(e => new LegalAuthorityResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetPaginatedReturnsSomeActiveItems()
        {
            var repository = this.GetRepository<LegalAuthority>();
            var controller = new LegalAuthoritiesController(repository);

            int pageSize = 3;
            int pageNum = 1;

            var items = ((await controller.Get(
                paging: new PaginationFilter() { PageSize = pageSize, Page = pageNum })
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _legalAuthorities
                .OrderBy(e => e.Id)
                .Where(e => e.Active)
                .Skip((pageNum - 1) * pageSize).Take(pageSize)
                .Select(e => new LegalAuthorityResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task InvalidPageSizeReturnsDefaultPagination()
        {
            var repository = this.GetRepository<LegalAuthority>();
            var controller = new LegalAuthoritiesController(repository);

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
            var repository = this.GetRepository<LegalAuthority>();
            var controller = new LegalAuthoritiesController(repository);

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
            var repository = this.GetRepository<LegalAuthority>();
            var controller = new LegalAuthoritiesController(repository);

            var result = await controller.Get(1).ConfigureAwait(false);

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = result.Result as OkObjectResult;
            actionResult.Value.Should().BeOfType<LegalAuthorityResource>();
            actionResult.StatusCode.Should().Be(200);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(21)]
        public async Task GetByIdReturnsCorrectItem(int id)
        {
            var repository = this.GetRepository<LegalAuthority>(id);
            var controller = new LegalAuthoritiesController(repository);

            var value = ((await controller.Get(id).ConfigureAwait(false))
                .Result as OkObjectResult).Value;

            var expected = new LegalAuthorityResource(_legalAuthorities
                .Single(e => e.Id == id));

            value.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetByMissingIdReturnsNotFound(int id)
        {
            var repository = this.GetRepository<LegalAuthority>();
            var controller = new LegalAuthoritiesController(repository);

            var result = await controller.Get(id).ConfigureAwait(false);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
            result.Value.Should().BeNull();
            (result.Result as NotFoundObjectResult).StatusCode.Should().Be(404);
            (result.Result as NotFoundObjectResult).Value.Should().Be(id);
        }

        [Fact]
        public async Task AddNewItemReturnsCorrectly()
        {
            var repository = this.GetRepository<LegalAuthority>();
            var controller = new LegalAuthoritiesController(repository);

            var item = new LegalAuthorityCreateResource()
            {
                AuthorityName = "New"
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
            var repository = this.GetRepository<LegalAuthority>();
            var controller = new LegalAuthoritiesController(repository);

            var item = new LegalAuthorityCreateResource()
            {
                AuthorityName = "New"
            };

            var result = await controller.Post(item).ConfigureAwait(false);

            var id = (int)(result as CreatedAtActionResult).Value;
            var addedItem = await repository.GetByIdAsync(id).ConfigureAwait(false);

            // Item gets added with next value in DB
            var expected = item.NewLegalAuthority();
            expected.Id = _legalAuthorities.Max(e => e.Id) + 1;

            addedItem.Should().BeEquivalentTo(expected);

            // Verify repository has changed.
            var resultItems = await repository.ListAsync().ConfigureAwait(false);

            resultItems.Count.Should().Be(_legalAuthorities.Count() + 1);
        }

        [Fact]
        public async Task AddNullItemFails()
        {
            var repository = this.GetRepository<LegalAuthority>();
            var controller = new LegalAuthoritiesController(repository);

            var result = await controller.Post(null).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            (result as BadRequestObjectResult).StatusCode.Should().Be(400);

            // Verify repository not changed after attempting to Post null item.
            var resultItems = ((await controller.Get(
                new ActiveItemFilter() { IncludeInactive = true },
                paging: new PaginationFilter() { PageSize = 0 })
                .ConfigureAwait(false))
                .Result as OkObjectResult).Value;

            var expected = _legalAuthorities
                .Select(e => new LegalAuthorityResource(e));

            resultItems.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task UpdateItemSucceeds()
        {
            var repository = this.GetRepository<LegalAuthority>();
            var controller = new LegalAuthoritiesController(repository);

            var id = 1;
            var target = new LegalAuthorityUpdateResource
            {
                Active = false,
                AuthorityName = "XYZ"
            };

            var original = await repository.GetByIdAsync(id)
                .ConfigureAwait(false);

            var result = await controller.Put(original.Id, target)
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
            var repository = this.GetRepository<LegalAuthority>();
            var controller = new LegalAuthoritiesController(repository);

            var original = await repository.GetByIdAsync(1).ConfigureAwait(false);

            var result = await controller.Put(original.Id, null).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            (result as BadRequestObjectResult).StatusCode.Should().Be(400);

            var updated = await repository.GetByIdAsync(1).ConfigureAwait(false);

            updated.Should().BeEquivalentTo(original);
        }

        [Fact]
        public async Task UpdateWithMissingIdFails()
        {
            var repository = this.GetRepository<LegalAuthority>();
            var controller = new LegalAuthoritiesController(repository);

            var target = new LegalAuthorityUpdateResource
            {
                Active = false,
                AuthorityName = "XYZ"
            };

            IActionResult result = await controller.Put(9999, target).ConfigureAwait(false);

            result.Should().BeOfType<NotFoundObjectResult>();
            (result as NotFoundObjectResult).StatusCode.Should().Be(404);
            (result as NotFoundObjectResult).Value.Should().Be(9999);
        }
    }
}
