using Enfo.API.Classes;
using Enfo.API.Controllers;
using Enfo.Domain.Entities;
using Enfo.Repository.Querying;
using Enfo.Repository.Repositories;
using Enfo.Infrastructure.SeedData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Enfo.Repository.Resources;
using Enfo.Repository.Resources.EnforcementOrder;
using Xunit;
using static Enfo.Domain.Entities.Enums;

namespace Enfo.API.Tests.UnitTests
{
    public class OrdersGetUnitTests
    {
        private readonly EnforcementOrder[] _orders;

        public OrdersGetUnitTests()
        {
            _orders = DevSeedData.GetEnforcementOrders();

            var epdContacts = ProdSeedData.GetEpdContacts();
            var addresses = ProdSeedData.GetAddresses();
            var legalAuthorities = ProdSeedData.GetLegalAuthorities();

            foreach (var contact in epdContacts)
            {
                contact.Address = addresses.SingleOrDefault(e => e.Id == contact.AddressId);
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
        public async Task GetReturnsCorrectly()
        {
            var mock = new Mock<IEnforcementOrderRepository>();
            mock.Setup(l => l.FindEnforcementOrdersAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<int?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<ActivityStatus>(),
                    It.IsAny<PublicationStatus>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<EnforcementOrderSorting>(),
                    It.IsAny<IPagination>()))
                .ReturnsAsync(_orders.ToList())
                .Verifiable();
            mock.Setup(l => l.CountEnforcementOrdersAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<int?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<ActivityStatus>(),
                    It.IsAny<PublicationStatus>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()))
                .Verifiable();

            var controller = new EnforcementOrdersController(mock.Object);

            var result = await controller.Get().ConfigureAwait(false);

            mock.Verify();
            mock.VerifyNoOtherCalls();

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = result.Result as OkObjectResult;
            Assert.IsAssignableFrom<PaginatedList<EnforcementOrderSummaryView>>(actionResult.Value);
            actionResult.StatusCode.Should().Be(200);
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

            var result = await controller.Get(id).ConfigureAwait(false);

            mock.Verify();
            mock.VerifyNoOtherCalls();

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = result.Result as OkObjectResult;
            actionResult.Value.Should().BeOfType<EnforcementOrderView>();
            actionResult.StatusCode.Should().Be(200);
        }

        [Theory]
        [InlineData(140)]
        [InlineData(27)]
        [InlineData(70789)]
        [InlineData(71715)]
        public async Task GetByIdReturnsCorrectItem(int id)
        {
            var item = _orders.Single(e => e.Id == id);

            var mock = new Mock<IEnforcementOrderRepository>();
            mock.Setup(l => l.GetEnforcementOrder(id, It.IsAny<bool>()))
                .ReturnsAsync(item);

            var controller = new EnforcementOrdersController(mock.Object);

            var value = ((await controller.Get(id).ConfigureAwait(false))
                .Result as OkObjectResult).Value;

            var expected = new EnforcementOrderView(item);

            value.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetByMissingIdReturnsNotFound(int id)
        {
            var mock = new Mock<IEnforcementOrderRepository>();
            mock.Setup(l => l.GetEnforcementOrder(id, It.IsAny<bool>()))
                .ReturnsAsync((EnforcementOrder)null);

            var controller = new EnforcementOrdersController(mock.Object);

            var result = await controller.Get(id).ConfigureAwait(false);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
            result.Value.Should().BeNull();
            (result.Result as NotFoundObjectResult).StatusCode.Should().Be(404);
            (result.Result as NotFoundObjectResult).Value.Should().Be(id);
        }
    }
}
