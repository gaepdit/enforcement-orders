using Enfo.API.Controllers;
using Enfo.API.Resources;
using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using Enfo.Infrastructure.SeedData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Enfo.API.Tests.UnitTests
{
    public class OrderDetailsUnitTests
    {
        private readonly EnforcementOrder[] _orders;

        public OrderDetailsUnitTests()
        {
            _orders = DevSeedData.GetEnforcementOrders();

            var epdContacts = ProdSeedData.GetEpdContacts();
            var addresses = ProdSeedData.GetAddresses();
            var legalAuthorities = ProdSeedData.GetLegalAuthorities();

            foreach (var contact in epdContacts)
            {
                contact.Address = addresses
                    .SingleOrDefault(e => e.Id == contact.AddressId);
            }

            foreach (var order in _orders)
            {
                order.LegalAuthority = legalAuthorities
                    .SingleOrDefault(e => e.Id == order.LegalAuthorityId);
                order.CommentContact = epdContacts
                    .SingleOrDefault(e => e.Id == order.CommentContactId);
                order.HearingContact = epdContacts
                    .SingleOrDefault(e => e.Id == order.HearingContactId);
            }
        }

        [Fact]
        public async Task GetByIdReturnsCorrectly()
        {
            var id = 140;
            var item = _orders.Single(e => e.Id == id);

            var mock = new Mock<IEnforcementOrderRepository>();
            mock.Setup(l => l.GetEnforcementOrder(id, It.IsAny<bool>()))
                .ReturnsAsync(item)
                .Verifiable();

            var controller = new EnforcementOrdersController(mock.Object);
            
            var result = await controller.Details(id).ConfigureAwait(false);

            mock.Verify(l => l.GetEnforcementOrder(id, It.IsAny<bool>()));
            mock.VerifyNoOtherCalls();

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = result.Result as OkObjectResult;
            actionResult.Value.Should().BeOfType<EnforcementOrderDetailedResource>();
            actionResult.StatusCode.Should().Be(200);
        }

        [Theory]
        [InlineData(140)]
        [InlineData(27)]
        [InlineData(70789)]
        [InlineData(71715)]
        public async Task GetDetailedByIdReturnsCorrectItem(int id)
        {
            var item = _orders.Single(e => e.Id == id);

            var mock = new Mock<IEnforcementOrderRepository>();
            mock.Setup(l => l.GetEnforcementOrder(id, It.IsAny<bool>()))
                .ReturnsAsync(item)
                .Verifiable();
            var controller = new EnforcementOrdersController(mock.Object);

            var value = ((await controller.Details(id).ConfigureAwait(false))
                .Result as OkObjectResult).Value;

            var expected = new EnforcementOrderDetailedResource(item);

            value.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetDetailedByMissingIdReturnsNotFound(int id)
        {
            var mock = new Mock<IEnforcementOrderRepository>();
            mock.Setup(l => l.GetEnforcementOrder(id, It.IsAny<bool>()))
                .ReturnsAsync((EnforcementOrder)null)
                .Verifiable();
            var controller = new EnforcementOrdersController(mock.Object);

            var result = await controller.Get(id).ConfigureAwait(false);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
            result.Value.Should().BeNull();
            (result.Result as NotFoundObjectResult).StatusCode.Should().Be(404);
            (result.Result as NotFoundObjectResult).Value.Should().Be(id);
        }
    }
}
