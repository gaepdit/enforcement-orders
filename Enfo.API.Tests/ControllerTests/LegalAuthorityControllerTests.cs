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
    public class LegalAuthorityControllerTests
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
        public async Task GetReturnsAllItemsAsync()
        {
            var repository = this.GetRepository<LegalAuthority>();
            var controller = new LegalAuthoritiesController(repository);

            var result = (await controller.Get().ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<LegalAuthorityResource>;

            var expected = new LegalAuthorityResource
            {
                Id = 1,
                Active = true,
                AuthorityName = "Air Quality Act",
                OrderNumberTemplate = "EPD-AQC-"
            };

            items.Should().HaveCount(3);
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

            var expected = new LegalAuthorityResource(await repository.GetByIdAsync(id).ConfigureAwait(false));

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

            LegalAuthorityResource item = new LegalAuthorityResource()
            {
                AuthorityName = "New",
                OrderNumberTemplate = "abc"
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

            var item = new LegalAuthorityResource()
            {
                AuthorityName = "New",
                OrderNumberTemplate = "abc"
            };

            var result = await controller.Post(item).ConfigureAwait(false);

            var id = (int)(result as CreatedAtActionResult).Value;
            var addedItem = new LegalAuthorityResource(await repository.GetByIdAsync(id).ConfigureAwait(false));

            var expected = new LegalAuthorityResource()
            {
                Id = 22,
                Active = true,
                AuthorityName = "New",
                OrderNumberTemplate = "abc"
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
                Id = 1,
                OrderNumberTemplate = "none"
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
