using Enfo.API.Controllers;
using Enfo.Domain.Entities;
using Enfo.Repository.Repositories;
using Enfo.Infrastructure.SeedData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enfo.Domain.Data;
using Enfo.Repository.Resources;
using Enfo.Repository.Resources.EnforcementOrder;
using Xunit;
using static Enfo.Domain.Entities.Enums;

namespace Enfo.API.Tests.IntegrationTests
{
    public class OrdersExtraUnitTests
    {
        private readonly EnforcementOrder[] _orders;

        public OrdersExtraUnitTests()
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
        public async Task GetCountReturnsCorrectly()
        {
            var mock = new Mock<IEnforcementOrderRepository>();
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
                .ReturnsAsync(10)
                .Verifiable();

            var controller = new EnforcementOrdersController(mock.Object);

            var result = await controller.Count().ConfigureAwait(false);

            mock.Verify();
            mock.VerifyNoOtherCalls();

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = result.Result as OkObjectResult;
            Assert.IsAssignableFrom<int>(actionResult.Value);
            actionResult.StatusCode.Should().Be(200);

            actionResult.Value.Should().Be(10);
        }

        [Fact]
        public async Task GetCurrentProposedReturnsCorrectItems()
        {
            var mock = new Mock<IEnforcementOrderRepository>();
            mock.Setup(l => l.FindCurrentProposedEnforcementOrders())
                .ReturnsAsync(_orders.ToList());

            var controller = new EnforcementOrdersController(mock.Object);

            var result = await controller.CurrentProposed().ConfigureAwait(false);

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = result.Result as OkObjectResult;
            Assert.IsAssignableFrom<IEnumerable<EnforcementOrderSummaryView>>(actionResult.Value);
            actionResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetRecentlyExecutedReturnsCorrectItems()
        {
            var mock = new Mock<IEnforcementOrderRepository>();
            mock.Setup(l => l.FindRecentlyExecutedEnforcementOrders())
                .ReturnsAsync(_orders.ToList());

            var controller = new EnforcementOrdersController(mock.Object);

            var result = await controller.RecentlyExecuted().ConfigureAwait(false);

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = result.Result as OkObjectResult;
            Assert.IsAssignableFrom<IEnumerable<EnforcementOrderSummaryView>>(actionResult.Value);
            actionResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetDraftsReturnsCorrectItems()
        {
            var mock = new Mock<IEnforcementOrderRepository>();
            mock.Setup(l => l.FindDraftEnforcementOrders())
                .ReturnsAsync(_orders.ToList());

            var controller = new EnforcementOrdersController(mock.Object);

            var result = await controller.Drafts().ConfigureAwait(false);

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = result.Result as OkObjectResult;
            Assert.IsAssignableFrom<IEnumerable<EnforcementOrderSummaryView>>(actionResult.Value);
            actionResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetPendingReturnsCorrectItems()
        {
            var mock = new Mock<IEnforcementOrderRepository>();
            mock.Setup(l => l.FindPendingEnforcementOrders())
                .ReturnsAsync(_orders.ToList());

            var controller = new EnforcementOrdersController(mock.Object);

            var result = await controller.Pending().ConfigureAwait(false);

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = result.Result as OkObjectResult;
            Assert.IsAssignableFrom<IEnumerable<EnforcementOrderSummaryView>>(actionResult.Value);
            actionResult.StatusCode.Should().Be(200);
        }
    }
}
