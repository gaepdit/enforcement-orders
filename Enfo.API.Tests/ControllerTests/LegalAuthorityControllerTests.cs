using Enfo.API.Controllers;
using Enfo.API.Resources;
using Enfo.API.Tests.Repositories;
using Enfo.Domain.Entities;
using Enfo.Infrastructure.Repositories;
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
        private readonly List<LegalAuthority> itemsList = new List<LegalAuthority>
        {
            new LegalAuthority { Id = 1, Active = true, AuthorityName = "ABC", OrderNumberTemplate = "abc" },
            new LegalAuthority { Id = 2, Active = false, AuthorityName = "DEF", OrderNumberTemplate = "def" },
            new LegalAuthority { Id = 3, Active = true, AuthorityName = "GHI", OrderNumberTemplate = "ghi" }
        };

        [Fact]
        public async Task GetReturnsOkAsync()
        {
            ILegalAuthorityRepository repository = new LegalAuthorityRepositoryFake(itemsList);
            LegalAuthoritiesController controller = new LegalAuthoritiesController(repository);

            var result = (await controller.GetAllAsync()
                .ConfigureAwait(false))
                .Result;

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetReturnsCorrectTypeAsync()
        {
            ILegalAuthorityRepository repository = new LegalAuthorityRepositoryFake(itemsList);
            LegalAuthoritiesController controller = new LegalAuthoritiesController(repository);

            var result = (await controller.GetAllAsync()
                .ConfigureAwait(false))
                .Result as OkObjectResult;

            Assert.IsAssignableFrom<IEnumerable<LegalAuthorityResource>>(result.Value);
        }

        [Fact]
        public async Task GetReturnsAllItemsAsync()
        {
            ILegalAuthorityRepository repository = new LegalAuthorityRepositoryFake(itemsList);
            LegalAuthoritiesController controller = new LegalAuthoritiesController(repository);

            var result = (await controller.GetAllAsync()
                .ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<LegalAuthorityResource>;

            var expected = new LegalAuthorityResource
            {
                Id = 1,
                Active = true,
                AuthorityName = "ABC",
                OrderNumberTemplate = "abc"
            };

            items.Should().HaveCount(3);
            items.ToList()[0].Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetByIdReturnsCorrectTypeAsync()
        {
            ILegalAuthorityRepository repository = new LegalAuthorityRepositoryFake(itemsList);
            LegalAuthoritiesController controller = new LegalAuthoritiesController(repository);

            var value = (await controller.GetByIdAsync(1)
                .ConfigureAwait(false))
                .Value;

            value.Should().BeOfType<LegalAuthorityResource>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetByIdReturnsCorrectItemAsync(int id)
        {
            ILegalAuthorityRepository repository = new LegalAuthorityRepositoryFake(itemsList);
            LegalAuthoritiesController controller = new LegalAuthoritiesController(repository);

            var value = (await controller.GetByIdAsync(id)
                .ConfigureAwait(false))
                .Value;

            value.Should().BeEquivalentTo(new LegalAuthorityResource(itemsList.Find(e => e.Id == id)));
        }

        [Fact]
        public async Task GetByMissingIdReturnsNotFoundAsync()
        {
            ILegalAuthorityRepository repository = new LegalAuthorityRepositoryFake(itemsList);
            LegalAuthoritiesController controller = new LegalAuthoritiesController(repository);

            LegalAuthorityResource result = (await controller.GetByIdAsync(0)
                .ConfigureAwait(false))
                .Value;

            result.Should().BeNull();
        }

        [Fact]
        public async Task AddNewItemReturnsCorrectly()
        {
            ILegalAuthorityRepository repository = new LegalAuthorityRepositoryFake(itemsList);
            LegalAuthoritiesController controller = new LegalAuthoritiesController(repository);

            LegalAuthorityResource item = new LegalAuthorityResource()
            {
                AuthorityName = "New",
                OrderNumberTemplate = "abc"
            };

            IActionResult result = await controller.PostLegalAuthority(item).ConfigureAwait(false);

            result.Should().BeOfType<CreatedAtActionResult>();
            (result as CreatedAtActionResult).ActionName.Should().Be("GetByIdAsync");
        }

        [Fact]
        public async Task UpdateItem()
        {
            ILegalAuthorityRepository repository = new LegalAuthorityRepositoryFake(itemsList);
            LegalAuthoritiesController controller = new LegalAuthoritiesController(repository);

            var target = new LegalAuthorityResource
            {
                Active = false,
                AuthorityName = "XYZ",
                Id = 1,
                OrderNumberTemplate = "none"
            };

            var original = await repository.GetByIdAsync(target.Id)
                .ConfigureAwait(false);

            IActionResult result = await controller.PutLegalAuthority(original.Id, target)
                .ConfigureAwait(false);

            result.Should().BeOfType<OkObjectResult>();
            (result as OkObjectResult).StatusCode.Should().Be(200);

            var updated = await repository.GetByIdAsync(target.Id)
                .ConfigureAwait(false);

            updated.Should().BeEquivalentTo(target);
        }
    }
}
