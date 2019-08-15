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
    public class LegalAuthoritiesControllerTests
    {
        [Fact]
        public async Task GetReturnsOkAsync()
        {
            var repository = this.GetRepository<LegalAuthority>();
            var controller = new LegalAuthoritiesController(repository);

            var result = (await controller.Get().ConfigureAwait(false))
                .Result;

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetReturnsCorrectTypeAsync()
        {
            var repository = this.GetRepository<LegalAuthority>();
            var controller = new LegalAuthoritiesController(repository);

            var result = (await controller.Get().ConfigureAwait(false))
                .Result as OkObjectResult;

            Assert.IsAssignableFrom<IEnumerable<LegalAuthorityResource>>(result.Value);
        }

        [Fact]
        public async Task GetReturnsAllActiveItemsAsync()
        {
            var repository = this.GetRepository<LegalAuthority>();
            var controller = new LegalAuthoritiesController(repository);

            var result = (await controller.Get().ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<LegalAuthorityResource>;

            var originalList = DevSeedData.GetLegalAuthorities().Where(e => e.Active).ToArray();
            var expected = new LegalAuthorityResource(originalList[0]);

            items.Should().HaveCount(originalList.Length);
            items.ToList()[0].Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetWithInactiveReturnsAllItemsAsync()
        {
            var repository = this.GetRepository<LegalAuthority>();
            var controller = new LegalAuthoritiesController(repository);

            var result = (await controller.Get(includeInactive: true).ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<LegalAuthorityResource>;

            var originalList = DevSeedData.GetLegalAuthorities();
            var expected = new LegalAuthorityResource(originalList[0]);

            items.Should().HaveCount(originalList.Length);
            items.ToList()[0].Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetPaginatedReturnsSomeActiveItemsAsync()
        {
            var repository = this.GetRepository<LegalAuthority>();
            var controller = new LegalAuthoritiesController(repository);

            int pageSize = 2;
            int pageNum = 1;
            int itemNum = (pageNum - 1) * pageSize + 1;

            var result = (await controller.Get(pageSize, pageNum).ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<LegalAuthorityResource>;

            var originalList = DevSeedData.GetLegalAuthorities();
            var expected = new LegalAuthorityResource(originalList[itemNum - 1]);

            items.Should().HaveCount(pageSize);
            items.ToList()[0].Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetByIdReturnsCorrectTypeAsync()
        {
            var repository = this.GetRepository<LegalAuthority>();
            var controller = new LegalAuthoritiesController(repository);

            var value = (await controller.Get(1).ConfigureAwait(false))
                .Value;

            value.Should().BeOfType<LegalAuthorityResource>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(21)]
        public async Task GetByIdReturnsCorrectItemAsync(int id)
        {
            var repository = this.GetRepository<LegalAuthority>(id);
            var controller = new LegalAuthoritiesController(repository);

            var value = (await controller.Get(id).ConfigureAwait(false))
                .Value;

            var originalList = DevSeedData.GetLegalAuthorities();
            var expected = new LegalAuthorityResource(originalList.Single(e => e.Id == id));

            value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetByMissingIdReturnsNotFoundAsync()
        {
            var repository = this.GetRepository<LegalAuthority>();
            var controller = new LegalAuthoritiesController(repository);

            LegalAuthorityResource result = (await controller.Get(0).ConfigureAwait(false))
                .Value;

            result.Should().BeNull();
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
            (result as CreatedAtActionResult).ActionName.Should().Be("Get");
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
            var addedItem = new LegalAuthorityResource(await repository.GetByIdAsync(id).ConfigureAwait(false));

            var expected = new LegalAuthorityResource()
            {
                Id = 22,
                Active = true,
                AuthorityName = "New"
            };

            addedItem.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task UpdateItemSucceeds()
        {
            var repository = this.GetRepository<LegalAuthority>();
            var controller = new LegalAuthoritiesController(repository);

            var target = new LegalAuthorityResource
            {
                Active = false,
                AuthorityName = "XYZ",
                Id = 1
            };

            var original = await repository.GetByIdAsync(target.Id).ConfigureAwait(false);

            IActionResult result = await controller.Put(original.Id, target).ConfigureAwait(false);

            result.Should().BeOfType<OkObjectResult>();
            (result as OkObjectResult).StatusCode.Should().Be(200);

            var updated = await repository.GetByIdAsync(target.Id).ConfigureAwait(false);

            updated.Should().BeEquivalentTo(target);
        }
    }
}
