using Enfo.API.Classes;
using Enfo.API.Controllers;
using Enfo.Domain.Entities;
using Enfo.Repository.Querying;
using Enfo.Repository.Repositories;
using Enfo.Infrastructure.SeedData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enfo.Repository.Resources;
using Enfo.Repository.Resources.LegalAuthority;
using Xunit;

namespace Enfo.API.Tests.UnitTests
{
    public class LegalAuthorityUnitTests
    {
        private readonly LegalAuthority[] _legalAuthorities;

        public LegalAuthorityUnitTests()
        {
            _legalAuthorities = ProdSeedData.GetLegalAuthorities();
        }

        [Fact]
        public async Task GetReturnsCorrectly()
        {
            var mock = new Mock<IWritableRepository<LegalAuthority>>();
            mock.Setup(l => l.ListAsync(
                    It.IsAny<ISpecification<LegalAuthority>>(),
                    It.IsAny<IPagination>(),
                    null,
                    null))
                .ReturnsAsync(_legalAuthorities.ToList())
                .Verifiable();
            mock.Setup(l => l.CountAsync(It.IsAny<ISpecification<LegalAuthority>>()))
                .Verifiable();

            var controller = new LegalAuthoritiesController(mock.Object);

            var result = await controller.Get().ConfigureAwait(false);

            mock.Verify();
            mock.VerifyNoOtherCalls();

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = result.Result as OkObjectResult;
            Assert.IsAssignableFrom<PaginatedList<LegalAuthorityView>>(actionResult.Value);
            actionResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetEmptySetReturnsCorrectly()
        {
            var emptyList = new List<LegalAuthority>();

            var mock = new Mock<IWritableRepository<LegalAuthority>>();
            mock.Setup(l => l.ListAsync(
                    It.IsAny<ISpecification<LegalAuthority>>(),
                    It.IsAny<IPagination>(),
                    null,
                    null))
                .ReturnsAsync(emptyList)
                .Verifiable();
            mock.Setup(l => l.CountAsync(It.IsAny<ISpecification<LegalAuthority>>()))
                .Verifiable();

            var controller = new LegalAuthoritiesController(mock.Object);

            var result = await controller.Get().ConfigureAwait(false);

            mock.Verify();
            mock.VerifyNoOtherCalls();

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = result.Result as OkObjectResult;
            Assert.IsAssignableFrom<PaginatedList<LegalAuthorityView>>(actionResult.Value);
            actionResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetByIdReturnsCorrectly()
        {
            var id = 1;
            var item = _legalAuthorities.Single(e => e.Id == id);

            var mock = new Mock<IWritableRepository<LegalAuthority>>();
            mock.Setup(l => l.GetByIdAsync(id, null, null))
                .ReturnsAsync(item)
                .Verifiable();

            var controller = new LegalAuthoritiesController(mock.Object);

            var result = await controller.Get(id).ConfigureAwait(false);

            mock.Verify();
            mock.VerifyNoOtherCalls();

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = result.Result as OkObjectResult;
            actionResult.Value.Should().BeOfType<LegalAuthorityView>();
            actionResult.StatusCode.Should().Be(200);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetByIdReturnsCorrectItem(int id)
        {
            var item = _legalAuthorities.Single(e => e.Id == id);

            var mock = new Mock<IWritableRepository<LegalAuthority>>();
            mock.Setup(l => l.GetByIdAsync(id, null, null))
                .ReturnsAsync(item);

            var controller = new LegalAuthoritiesController(mock.Object);

            var value = ((await controller.Get(id).ConfigureAwait(false))
                .Result as OkObjectResult).Value;

            var expected = new LegalAuthorityView(item);

            value.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetByMissingIdReturnsNotFound(int id)
        {
            var mock = new Mock<IWritableRepository<LegalAuthority>>();
            LegalAuthority nullAddress = null;
            mock.Setup(l => l.GetByIdAsync(id, null, null))
                .ReturnsAsync(nullAddress);

            var controller = new LegalAuthoritiesController(mock.Object);

            var result = await controller.Get(id).ConfigureAwait(false);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
            result.Value.Should().BeNull();
            (result.Result as NotFoundObjectResult).StatusCode.Should().Be(404);
            (result.Result as NotFoundObjectResult).Value.Should().Be(id);
        }

        [Fact]
        public async Task AddNewItemReturnsCorrectly()
        {
            var item = new LegalAuthorityCreate()
            {
                AuthorityName = "New"
            };

            var mock = new Mock<IWritableRepository<LegalAuthority>>();
            mock.Setup(l => l.CompleteAsync()).ReturnsAsync(1);
            mock.Setup(l => l.Add(item.NewLegalAuthority()));

            var controller = new LegalAuthoritiesController(mock.Object);

            var result = await controller.Post(item).ConfigureAwait(false);

            result.Should().BeOfType<CreatedAtActionResult>();
            var actionResult = result as CreatedAtActionResult;
            actionResult.ActionName.Should().Be("Get");
            actionResult.StatusCode.Should().Be(201);
            actionResult.Value.Should().BeOfType<int>();
        }

        [Fact]
        public async Task AddNullItemFails()
        {
            var mock = new Mock<IWritableRepository<LegalAuthority>>();
            var controller = new LegalAuthoritiesController(mock.Object);

            var result = await controller.Post(null).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            (result as BadRequestObjectResult).StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task AddInvalidItemFails()
        {
            var mock = new Mock<IWritableRepository<LegalAuthority>>();
            var controller = new LegalAuthoritiesController(mock.Object);

            var item = new LegalAuthorityCreate()
            {
                AuthorityName = null
            };

            controller.ModelState.AddModelError(nameof(item.AuthorityName), "Legal Authority name is required");

            var result = await controller.Post(item).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            var objectResult = (result as BadRequestObjectResult);
            objectResult.StatusCode.Should().Be(400);
            var objectResultValue = (objectResult.Value as Microsoft.AspNetCore.Mvc.SerializableError);
            objectResultValue.Count.Should().Be(1);
            objectResultValue.Keys.Should()
                .BeEquivalentTo(new List<string>()
                    {
                        nameof(LegalAuthorityCreate.AuthorityName)
                    });
        }

        [Fact]
        public async Task UpdateItemReturnCorrectly()
        {
            var id = 1;
            var item = _legalAuthorities.Single(e => e.Id == id);

            var target = new LegalAuthorityUpdate
            {
                Active = false,
                AuthorityName = "XYZ"
            };

            var mock = new Mock<IWritableRepository<LegalAuthority>>();
            mock.Setup(l => l.CompleteAsync()).ReturnsAsync(1);
            mock.Setup(l => l.GetByIdAsync(id, null, null))
                .ReturnsAsync(item);

            var controller = new LegalAuthoritiesController(mock.Object);

            var result = await controller.Put(id, target)
                .ConfigureAwait(false);

            result.Should().BeOfType<NoContentResult>();
            (result as NoContentResult).StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task UpdateWithNullFails()
        {
            var id = 1;

            var mock = new Mock<IWritableRepository<LegalAuthority>>();
            var controller = new LegalAuthoritiesController(mock.Object);

            var result = await controller.Put(id, null).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            (result as BadRequestObjectResult).StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task UpdateWithMissingIdFails()
        {
            var id = 9999;

            var mock = new Mock<IWritableRepository<LegalAuthority>>();
            mock.Setup(l => l.CompleteAsync()).ReturnsAsync(1);
            mock.Setup(l => l.GetByIdAsync(id, null, null))
                .ReturnsAsync((LegalAuthority)null);

            var controller = new LegalAuthoritiesController(mock.Object);

            var target = new LegalAuthorityUpdate
            {
                Active = false,
                AuthorityName = "XYZ"
            };

            var result = await controller.Put(id, target).ConfigureAwait(false);

            result.Should().BeOfType<NotFoundObjectResult>();
            (result as NotFoundObjectResult).StatusCode.Should().Be(404);
            (result as NotFoundObjectResult).Value.Should().Be(id);
        }
    }
}
