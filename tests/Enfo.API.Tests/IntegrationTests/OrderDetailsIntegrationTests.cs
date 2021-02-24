using Enfo.API.Controllers;
using Enfo.API.Tests.Helpers;
using Enfo.Domain.Entities;
using Enfo.Infrastructure.SeedData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Enfo.Domain.Data;
using Enfo.Repository.Resources;
using Enfo.Repository.Resources.EnforcementOrder;
using Xunit;

namespace Enfo.API.Tests.IntegrationTests
{
    public class OrderDetailsIntegrationTests
    {
        private readonly EnforcementOrder[] _orders;

        public OrderDetailsIntegrationTests()
        {
            _orders = TestData.GetEnforcementOrders();

            var epdContacts = DomainData.GetEpdContacts();
            var addresses = DomainData.GetAddresses();
            var legalAuthorities = DomainData.GetLegalAuthorities();

            foreach (var contact in epdContacts)
            {
                contact.Address = addresses.SingleOrDefault(e => e.Id == contact.AddressId);
            }

            foreach (var order in _orders)
            {
                order.LegalAuthority = legalAuthorities.SingleOrDefault(e => e.Id == order.LegalAuthorityId);
                order.CommentContact = epdContacts.SingleOrDefault(e => e.Id == order.CommentContactId);
                order.HearingContact = epdContacts.SingleOrDefault(e => e.Id == order.HearingContactId);
            }
        }

        [Fact]
        public async Task GetByIdReturnsCorrectly()
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var result = await controller.Details(140).ConfigureAwait(false);

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = result.Result as OkObjectResult;
            actionResult.Value.Should().BeOfType<EnforcementOrderDetailedView>();
            actionResult.StatusCode.Should().Be(200);
        }

        [Theory]
        [InlineData(140)]
        [InlineData(27)]
        [InlineData(70789)]
        [InlineData(71715)]
        public async Task GetDetailedByIdReturnsCorrectItem(int id)
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var value = ((await controller.Details(id).ConfigureAwait(false))
                .Result as OkObjectResult).Value;

            var expected = new EnforcementOrderDetailedView(
                _orders.Single(e => e.Id == id));

            value.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetDetailedByMissingIdReturnsNotFound(int id)
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var result = await controller.Get(id).ConfigureAwait(false);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
            result.Value.Should().BeNull();
            (result.Result as NotFoundObjectResult).StatusCode.Should().Be(404);
            (result.Result as NotFoundObjectResult).Value.Should().Be(id);
        }
    }
}
