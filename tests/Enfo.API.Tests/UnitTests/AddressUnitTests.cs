using Enfo.API.Controllers;
using Enfo.API.Resources;
using Enfo.Domain.Entities;
using Enfo.Domain.Querying;
using Enfo.Domain.Repositories;
using Enfo.Infrastructure.SeedData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Enfo.API.Tests.UnitTests
{
    public class AddressUnitTests
    {
        private readonly Address[] _addresses;

        public AddressUnitTests()
        {
            _addresses = ProdSeedData.GetAddresses();
        }

        [Fact]
        public async Task GetReturnsCorrectly()
        {
            var mock = new Mock<IWritableRepository<Address>>();

            mock.Setup(l => l.ListAsync(
                It.IsAny<Specification<Address>>(),
                It.IsAny<Pagination>(),
                null,
                null))
                .ReturnsAsync(_addresses.ToList())
                .Verifiable();

            var controller = new AddressesController(mock.Object);

            var result = await controller.Get().ConfigureAwait(false);

            mock.Verify(l => l.ListAsync(
                It.IsAny<ISpecification<Address>>(),
                It.IsAny<IPagination>(),
                null,
                null));
            mock.VerifyNoOtherCalls();

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = result.Result as OkObjectResult;
            Assert.IsAssignableFrom<IEnumerable<AddressResource>>(actionResult.Value);
            actionResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetEmptySetReturnsCorrectly()
        {
            var emptyList = new List<Address>();

            var mock = new Mock<IWritableRepository<Address>>();

            mock.Setup(l => l.ListAsync(
                It.IsAny<ISpecification<Address>>(),
                It.IsAny<IPagination>(),
                null,
                null))
                .ReturnsAsync(emptyList)
                .Verifiable();

            var controller = new AddressesController(mock.Object);

            var result = await controller.Get().ConfigureAwait(false);

            mock.Verify(l => l.ListAsync(
                It.IsAny<ISpecification<Address>>(),
                It.IsAny<IPagination>(),
                null,
                null));
            mock.VerifyNoOtherCalls();

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = result.Result as OkObjectResult;
            Assert.IsAssignableFrom<IEnumerable<AddressResource>>(actionResult.Value);
            actionResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetByIdReturnsCorrectly()
        {
            var id = 2000;
            var item = _addresses.Single(e => e.Id == id);

            var mock = new Mock<IWritableRepository<Address>>();
            mock.Setup(l => l.GetByIdAsync(id, null, null))
                .ReturnsAsync(item)
                .Verifiable();

            var controller = new AddressesController(mock.Object);

            var result = await controller.Get(id).ConfigureAwait(false);

            mock.Verify(l => l.GetByIdAsync(id, null, null));
            mock.VerifyNoOtherCalls();

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
            var item = _addresses.Single(e => e.Id == id);

            var mock = new Mock<IWritableRepository<Address>>();
            mock.Setup(l => l.GetByIdAsync(id, null, null))
                .ReturnsAsync(item);

            var controller = new AddressesController(mock.Object);

            var value = ((await controller.Get(id).ConfigureAwait(false))
                .Result as OkObjectResult).Value;

            var expected = new AddressResource(item);

            value.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetByMissingIdReturnsNotFound(int id)
        {
            var mock = new Mock<IWritableRepository<Address>>();
            mock.Setup(l => l.GetByIdAsync(id, null, null))
                .ReturnsAsync((Address)null);

            var controller = new AddressesController(mock.Object);

            var result = await controller.Get(id).ConfigureAwait(false);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
            result.Value.Should().BeNull();
            (result.Result as NotFoundObjectResult).StatusCode.Should().Be(404);
            (result.Result as NotFoundObjectResult).Value.Should().Be(id);
        }

        [Fact]
        public async Task AddNewItemReturnsCorrectly()
        {
            var item = new AddressCreateResource()
            {
                City = "Atlanta",
                PostalCode = "33333",
                State = "GA",
                Street = "123 Fake St"
            };

            var mock = new Mock<IWritableRepository<Address>>();
            mock.Setup(l => l.CompleteAsync()).ReturnsAsync(1);
            mock.Setup(l => l.Add(item.NewAddress()));

            var controller = new AddressesController(mock.Object);

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
            var mock = new Mock<IWritableRepository<Address>>();
            var controller = new AddressesController(mock.Object);

            var result = await controller.Post(null).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            (result as BadRequestObjectResult).StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task AddInvalidItemFails()
        {
            var mock = new Mock<IWritableRepository<Address>>();
            var controller = new AddressesController(mock.Object);

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
            objectResultValue.Keys.Should()
                .BeEquivalentTo(new List<string>()
                    {
                        nameof(AddressCreateResource.City),
                        nameof(AddressCreateResource.PostalCode)
                    });
        }

        [Fact]
        public async Task UpdateItemReturnCorrectly()
        {
            var id = 2000;
            var item = _addresses.Single(e => e.Id == id);

            var target = new AddressUpdateResource
            {
                City = "Atlanta",
                PostalCode = "33333",
                State = "GA",
                Street = "123 Fake St"
            };

            var mock = new Mock<IWritableRepository<Address>>();
            mock.Setup(l => l.CompleteAsync()).ReturnsAsync(1);
            mock.Setup(l => l.GetByIdAsync(id, null, null))
                .ReturnsAsync(item);

            var controller = new AddressesController(mock.Object);

            var result = await controller.Put(id, target)
                .ConfigureAwait(false);

            result.Should().BeOfType<NoContentResult>();
            (result as NoContentResult).StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task UpdateWithNullFails()
        {
            var id = 2000;

            var mock = new Mock<IWritableRepository<Address>>();
            var controller = new AddressesController(mock.Object);

            var result = await controller.Put(id, null).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            (result as BadRequestObjectResult).StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task UpdateWithMissingIdFails()
        {
            var id = 9999;

            var mock = new Mock<IWritableRepository<Address>>();
            mock.Setup(l => l.CompleteAsync()).ReturnsAsync(1);
            mock.Setup(l => l.GetByIdAsync(id, null, null))
                .ReturnsAsync((Address)null);

            var controller = new AddressesController(mock.Object);

            var target = new AddressUpdateResource
            {
                City = "Atlanta",
                PostalCode = "33333",
                State = "GA",
                Street = "123 Fake St"
            };

            var result = await controller.Put(id, target).ConfigureAwait(false);

            result.Should().BeOfType<NotFoundObjectResult>();
            (result as NotFoundObjectResult).StatusCode.Should().Be(404);
            (result as NotFoundObjectResult).Value.Should().Be(id);
        }
    }
}
