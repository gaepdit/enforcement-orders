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
using Xunit;

namespace Enfo.API.Tests.UnitTests
{
    public class CountyUnitTests
    {
        private readonly County[] _counties;

        public CountyUnitTests()
        {
            _counties = ProdSeedData.GetCounties();
        }

        [Fact]
        public async Task GetReturnsCorrectly()
        {
            var mock = new Mock<IReadableRepository<County>>();

            mock.Setup(l => l.ListAsync(
                    It.IsAny<ISpecification<County>>(),
                    It.IsAny<IPagination>(),
                    null,
                    null))
                .ReturnsAsync(_counties.ToList())
                .Verifiable();
            mock.Setup(l => l.CountAsync(It.IsAny<ISpecification<County>>()))
                .Verifiable();

            var controller = new CountiesController(mock.Object);

            var result = await controller.Get().ConfigureAwait(false);

            mock.Verify();
            mock.VerifyNoOtherCalls();

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = result.Result as OkObjectResult;
            Assert.IsAssignableFrom<PaginatedList<CountyResource>>(actionResult.Value);
            actionResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetEmptySetReturnsCorrectly()
        {
            var emptyList = new List<County>();

            var mock = new Mock<IWritableRepository<County>>();

            mock.Setup(l => l.ListAsync(
                    It.IsAny<ISpecification<County>>(),
                    It.IsAny<IPagination>(),
                    null,
                    null))
                .ReturnsAsync(emptyList)
                .Verifiable();
            mock.Setup(l => l.CountAsync(It.IsAny<ISpecification<County>>()))
                .Verifiable();

            var controller = new CountiesController(mock.Object);

            var result = await controller.Get().ConfigureAwait(false);

            mock.Verify();
            mock.VerifyNoOtherCalls();

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = result.Result as OkObjectResult;
            Assert.IsAssignableFrom<PaginatedList<CountyResource>>(actionResult.Value);
            actionResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetByIdReturnsCorrectly()
        {
            var id = 1;
            var item = _counties.Single(e => e.Id == id);

            var mock = new Mock<IWritableRepository<County>>();
            mock.Setup(l => l.GetByIdAsync(id, null, null))
                .ReturnsAsync(item)
                .Verifiable();

            var controller = new CountiesController(mock.Object);

            var result = await controller.Get(id).ConfigureAwait(false);

            mock.Verify();
            mock.VerifyNoOtherCalls();

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = result.Result as OkObjectResult;
            actionResult.Value.Should().BeOfType<CountyResource>();
            actionResult.StatusCode.Should().Be(200);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetByIdReturnsCorrectItem(int id)
        {
            var item = _counties.Single(e => e.Id == id);

            var mock = new Mock<IWritableRepository<County>>();
            mock.Setup(l => l.GetByIdAsync(id, null, null))
                .ReturnsAsync(item);

            var controller = new CountiesController(mock.Object);

            var value = ((await controller.Get(id).ConfigureAwait(false))
                .Result as OkObjectResult).Value;

            var expected = new CountyResource(item);

            value.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetByMissingIdReturnsNotFound(int id)
        {
            var mock = new Mock<IWritableRepository<County>>();
            County nullAddress = null;
            mock.Setup(l => l.GetByIdAsync(id, null, null))
                .ReturnsAsync(nullAddress);

            var controller = new CountiesController(mock.Object);

            var result = await controller.Get(id).ConfigureAwait(false);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
            result.Value.Should().BeNull();
            (result.Result as NotFoundObjectResult).StatusCode.Should().Be(404);
            (result.Result as NotFoundObjectResult).Value.Should().Be(id);
        }
    }
}
