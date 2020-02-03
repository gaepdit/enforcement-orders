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

namespace Enfo.API.Tests
{
    public class EpdContactUnitTests
    {
        private readonly EpdContact[] _epdContacts;
        private readonly Address[] _addresses;

        public EpdContactUnitTests()
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
            var mock = new Mock<IAsyncWritableRepository<EpdContact>>();

            mock.Setup(l => l.ListAsync(
                It.IsAny<Specification<EpdContact>>(),
                It.IsAny<Pagination>(),
                null,
                It.IsAny<Inclusion<EpdContact>>()))
                .ReturnsAsync(_epdContacts.ToList())
                .Verifiable();

            var controller = new EpdContactsController(mock.Object);

            var result = await controller.Get().ConfigureAwait(false);

            mock.Verify(l => l.ListAsync(
                It.IsAny<Specification<EpdContact>>(),
                It.IsAny<Pagination>(),
                null,
                It.IsAny<Inclusion<EpdContact>>()));
            mock.VerifyNoOtherCalls();

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = result.Result as OkObjectResult;
            Assert.IsAssignableFrom<IEnumerable<EpdContactResource>>(actionResult.Value);
            actionResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetEmptySetReturnsCorrectly()
        {
            var emptyList = new List<EpdContact>();

            var mock = new Mock<IAsyncWritableRepository<EpdContact>>();

            mock.Setup(l => l.ListAsync(
                It.IsAny<Specification<EpdContact>>(),
                It.IsAny<Pagination>(),
                null,
                It.IsAny<Inclusion<EpdContact>>()))
                .ReturnsAsync(emptyList)
                .Verifiable();

            var controller = new EpdContactsController(mock.Object);

            var result = await controller.Get().ConfigureAwait(false);

            mock.Verify(l => l.ListAsync(
                It.IsAny<Specification<EpdContact>>(),
                It.IsAny<Pagination>(),
                null,
                It.IsAny<Inclusion<EpdContact>>()));
            mock.VerifyNoOtherCalls();

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = result.Result as OkObjectResult;
            Assert.IsAssignableFrom<IEnumerable<EpdContactResource>>(actionResult.Value);
            actionResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetByIdReturnsCorrectly()
        {
            var id = 2000;

            var mock = new Mock<IAsyncWritableRepository<EpdContact>>();
            mock.Setup(l => l.GetByIdAsync(
                id,
                null,
                It.IsAny<Inclusion<EpdContact>>()))
                .ReturnsAsync(_epdContacts.Single(e => e.Id == id))
                .Verifiable();

            var controller = new EpdContactsController(mock.Object);

            var result = await controller.Get(id).ConfigureAwait(false);

            mock.Verify(l => l.GetByIdAsync(
                id,
                null,
                It.IsAny<EpdContactIncludingAddress>()));
            mock.VerifyNoOtherCalls();

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
            var mock = new Mock<IAsyncWritableRepository<EpdContact>>();
            mock.Setup(l => l.GetByIdAsync(id, null, It.IsAny<EpdContactIncludingAddress>()))
                .ReturnsAsync(_epdContacts.Single(e => e.Id == id));

            var controller = new EpdContactsController(mock.Object);

            var value = ((await controller.Get(id).ConfigureAwait(false))
                .Result as OkObjectResult).Value;

            var expected = new EpdContactResource(_epdContacts
                .Single(e => e.Id == id));

            value.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetByMissingIdReturnsNotFound(int id)
        {
            var mock = new Mock<IAsyncWritableRepository<EpdContact>>();
            EpdContact nullItem = null;
            mock.Setup(l => l.GetByIdAsync(id, null, null))
                .ReturnsAsync(nullItem);

            var controller = new EpdContactsController(mock.Object);

            var result = await controller.Get(id).ConfigureAwait(false);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
            result.Value.Should().BeNull();
            (result.Result as NotFoundObjectResult).StatusCode.Should().Be(404);
            (result.Result as NotFoundObjectResult).Value.Should().Be(id);
        }

        [Fact]
        public async Task AddNewItemReturnsCorrectly()
        {
            var item = new EpdContactCreateResource()
            {
                AddressId = 2000,
                ContactName = "Mr. Fake Name",
                Email = "fake.name@example.com",
                Organization = "Environmental Protection Division",
                Title = "Ombudsman"
            };

            var mock = new Mock<IAsyncWritableRepository<EpdContact>>();
            mock.Setup(l => l.CompleteAsync()).ReturnsAsync(1);
            mock.Setup(l => l.Add(item.NewEpdContact()));

            var controller = new EpdContactsController(mock.Object);

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
            var mock = new Mock<IAsyncWritableRepository<EpdContact>>();
            var controller = new EpdContactsController(mock.Object);

            var result = await controller.Post(null).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            (result as BadRequestObjectResult).StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task AddInvalidItemFails()
        {
            var mock = new Mock<IAsyncWritableRepository<EpdContact>>();
            var controller = new EpdContactsController(mock.Object);

            var item = new EpdContactCreateResource()
            {
                AddressId = 2000,
                ContactName = null,
                Email = "fake.name@example.com",
                Organization = "Environmental Protection Division",
                Title = "Ombudsman"
            };

            controller.ModelState.AddModelError(nameof(item.ContactName), "Contact Name is required");

            var result = await controller.Post(item).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            var objectResult = (result as BadRequestObjectResult);
            objectResult.StatusCode.Should().Be(400);
            var objectResultValue = (objectResult.Value as Microsoft.AspNetCore.Mvc.SerializableError);
            objectResultValue.Count.Should().Be(1);
            objectResultValue.Keys.Should()
                .BeEquivalentTo(new List<string>()
                    {
                        nameof(EpdContactCreateResource.ContactName)
                    });
        }

        [Fact]
        public async Task UpdateItemReturnCorrectly()
        {
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

            var mock = new Mock<IAsyncWritableRepository<EpdContact>>();
            mock.Setup(l => l.CompleteAsync()).ReturnsAsync(1);
            mock.Setup(l => l.GetByIdAsync(id, null, null))
                .ReturnsAsync(_epdContacts.Single(e => e.Id == id));

            var controller = new EpdContactsController(mock.Object);

            var result = await controller.Put(id, target)
                .ConfigureAwait(false);

            result.Should().BeOfType<NoContentResult>();
            (result as NoContentResult).StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task UpdateWithNullFails()
        {
            var id = 2000;

            var mock = new Mock<IAsyncWritableRepository<EpdContact>>();
            var controller = new EpdContactsController(mock.Object);

            var result = await controller.Put(id, null).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            (result as BadRequestObjectResult).StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task UpdateWithMissingIdFails()
        {
            var id = 9999;
            EpdContact nullItem = null;

            var mock = new Mock<IAsyncWritableRepository<EpdContact>>();
            mock.Setup(l => l.CompleteAsync()).ReturnsAsync(1);
            mock.Setup(l => l.GetByIdAsync(id, null, null))
                .ReturnsAsync(nullItem);

            var controller = new EpdContactsController(mock.Object);

            var target = new EpdContactUpdateResource
            {
                AddressId = 2002,
                ContactName = "Name Update",
                Email = "email@example.com",
                Organization = "Com",
                Telephone = "555-1212",
                Title = "Title"
            };

            var result = await controller.Put(id, target).ConfigureAwait(false);

            result.Should().BeOfType<NotFoundObjectResult>();
            (result as NotFoundObjectResult).StatusCode.Should().Be(404);
            (result as NotFoundObjectResult).Value.Should().Be(id);
        }
    }
}
