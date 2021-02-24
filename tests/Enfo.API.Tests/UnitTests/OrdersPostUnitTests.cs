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
using Enfo.Domain.Data;
using Enfo.Repository.Resources;
using Enfo.Repository.Resources.EnforcementOrder;
using Xunit;
using static Enfo.Domain.Entities.EnforcementOrder;

namespace Enfo.API.Tests.IntegrationTests
{
    public class OrdersPostUnitTests
    {
        private readonly EnforcementOrder[] _enforcementOrders;
        private readonly EpdContact[] _epdContacts;
        private readonly Address[] _addresses;
        private readonly LegalAuthority[] _legalAuthorities;

        public OrdersPostUnitTests()
        {
            _enforcementOrders = TestData.GetEnforcementOrders();
            _epdContacts = DomainData.GetEpdContacts();
            _addresses = DomainData.GetAddresses();
            _legalAuthorities = DomainData.GetLegalAuthorities();

            foreach (var contact in _epdContacts)
            {
                contact.Address = _addresses.SingleOrDefault(e => e.Id == contact.AddressId);
            }

            foreach (var order in _enforcementOrders)
            {
                order.LegalAuthority = _legalAuthorities.SingleOrDefault(e => e.Id == order.LegalAuthorityId);
                order.CommentContact = order.CommentContactId.HasValue ? _epdContacts.SingleOrDefault(e => e.Id == order.CommentContactId) : null;
                order.HearingContact = order.HearingContactId.HasValue ? _epdContacts.SingleOrDefault(e => e.Id == order.HearingContactId) : null;
            }
        }

        [Fact]
        public async Task AddNewItemReturnsCorrectly()
        {
            var item = new EnforcementOrderCreate()
            {
                FacilityName = "TEST FACILITY",
                County = "Appling",
                LegalAuthorityId = 1,
                OrderNumber = "TEST-123",
                CreateAs = NewEnforcementOrderType.Executed
            };

            var mock = new Mock<IEnforcementOrderRepository>();
            mock.Setup(l => l.OrderNumberExists(item.OrderNumber, -1))
                .ReturnsAsync(false);
            mock.Setup(l => l.CreateEnforcementOrderAsync(
                    It.IsAny<NewEnforcementOrderType>(),
                    It.IsAny<string>(),
                    It.IsAny<int?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<int?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<PublicationState>(),
                    It.IsAny<string>(),
                    It.IsAny<decimal?>()))
                .ReturnsAsync(new CreateEntityResult<EnforcementOrder>(item.NewEnforcementOrder()));

            var controller = new EnforcementOrdersController(mock.Object);

            var result = await controller.Post(item).ConfigureAwait(false);

            result.Should().BeOfType<CreatedAtActionResult>();
            var actionResult = result as CreatedAtActionResult;
            actionResult.ActionName.Should().Be("Get");
            actionResult.StatusCode.Should().Be(201);
            actionResult.Value.Should().BeOfType<int>();
        }

        [Fact]
        public async Task CreateWithDuplicateOrderNumberFails()
        {
            var item = new EnforcementOrderCreate()
            {
                FacilityName = "TEST FACILITY",
                County = "Appling",
                LegalAuthorityId = 1,
                OrderNumber = "TEST-123",
                CreateAs = NewEnforcementOrderType.Executed
            };

            var createResult = new CreateEntityResult<EnforcementOrder>("OrderNumber", "An Order with the same number already exists.");

            var mock = new Mock<IEnforcementOrderRepository>();
            mock.Setup(l => l.CreateEnforcementOrderAsync(
                    It.IsAny<NewEnforcementOrderType>(),
                    It.IsAny<string>(),
                    It.IsAny<int?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<int?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<PublicationState>(),
                    It.IsAny<string>(),
                    It.IsAny<decimal?>()))
                .ReturnsAsync(createResult);

            var controller = new EnforcementOrdersController(mock.Object);

            var result = await controller.Post(item).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            (result as BadRequestObjectResult).StatusCode.Should().Be(400);
            ((result as BadRequestObjectResult).Value as SerializableError)
                .Should().HaveCount(1)
                .And.ContainKey("OrderNumber");
        }

        [Fact]
        public async Task AddNullItemFails()
        {
            var mock = new Mock<IEnforcementOrderRepository>();
            var controller = new EnforcementOrdersController(mock.Object);

            var result = await controller.Post(null).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            (result as BadRequestObjectResult).StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task UpdateOrderSucceeds()
        {
            var id = 140;
            var target = new EnforcementOrderUpdate()
            {
                Cause = "Integer feugiat scelerisque varius morbi enim nunc faucibus a.",
                CommentContactId = null,
                CommentPeriodClosesDate = null,
                County = "Liberty",
                ExecutedDate = new DateTime(1998, 06, 29),
                ExecutedOrderPostedDate = new DateTime(1998, 07, 06),
                FacilityName = "A diam maecenas",
                HearingCommentPeriodClosesDate = null,
                HearingDate = null,
                HearingLocation = null,
                IsExecutedOrder = true,
                IsHearingScheduled = false,
                LegalAuthorityId = 7,
                OrderNumber = "TEST-123",
                ProposedOrderPostedDate = null,
                PublicationStatus = PublicationState.Published,
                Requirements = "Duis ut diam quam nulla porttitor massa id neque. A lacus vestibulum sed arcu non. Amet massa vitae tortor condimentum. Magnis dis parturient montes nascetur ridiculus mus mauris. Arcu risus quis varius quam quisque id diam. Pellentesque massa placerat duis ultricies lacus sed. Tellus in hac habitasse platea dictumst vestibulum. Justo nec ultrices dui sapien eget. Ac odio tempor orci dapibus ultrices in. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Donec et odio pellentesque diam volutpat. Nunc faucibus a pellentesque sit amet porttitor eget dolor morbi. Neque ornare aenean euismod elementum nisi quis eleifend quam. Praesent elementum facilisis leo vel fringilla est ullamcorper eget. Et netus et malesuada fames. Urna et pharetra pharetra massa massa ultricies mi quis. Sit amet consectetur adipiscing elit. Felis donec et odio pellentesque diam volutpat commodo sed egestas. Adipiscing elit pellentesque habitant morbi."
                + Environment.NewLine
                + "Duis ut diam quam nulla porttitor massa id neque. A lacus vestibulum sed arcu non. Amet massa vitae tortor condimentum. Magnis dis parturient montes nascetur ridiculus mus mauris. Arcu risus quis varius quam quisque id diam. Pellentesque massa placerat duis ultricies lacus sed. Tellus in hac habitasse platea dictumst vestibulum. Justo nec ultrices dui sapien eget. Ac odio tempor orci dapibus ultrices in. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Donec et odio pellentesque diam volutpat. Nunc faucibus a pellentesque sit amet porttitor eget dolor morbi. Neque ornare aenean euismod elementum nisi quis eleifend quam. Praesent elementum facilisis leo vel fringilla est ullamcorper eget. Et netus et malesuada fames. Urna et pharetra pharetra massa massa ultricies mi quis. Sit amet consectetur adipiscing elit. Felis donec et odio pellentesque diam volutpat commodo sed egestas. Adipiscing elit pellentesque habitant morbi.",
                SettlementAmount = 2000
            };

            var mock = new Mock<IEnforcementOrderRepository>();
            mock.Setup(l => l.IdExists(
                    id,
                    It.IsAny<ISpecification<EnforcementOrder>>()))
                .ReturnsAsync(true);
            mock.Setup(l => l.UpdateEnforcementOrderAsync(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<int?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<int?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<PublicationState>(),
                    It.IsAny<string>(),
                    It.IsAny<decimal?>()))
                .ReturnsAsync(new UpdateEntityResult());

            var controller = new EnforcementOrdersController(mock.Object);

            var result = await controller.Put(id, target);

            result.Should().BeOfType<NoContentResult>();
            (result as NoContentResult).StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task UpdateWithNullFails()
        {
            var id = 140;

            var mock = new Mock<IEnforcementOrderRepository>();
            var controller = new EnforcementOrdersController(mock.Object);

            var result = await controller.Put(id, null).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            (result as BadRequestObjectResult).StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task UpdateWithMissingIdFails()
        {
            var id = 9999;
            var target = new EnforcementOrderUpdate()
            {
                Cause = "Integer feugiat scelerisque varius morbi enim nunc faucibus a.",
                CommentContactId = null,
                CommentPeriodClosesDate = null,
                County = "Liberty",
                ExecutedDate = new DateTime(1998, 06, 29),
                ExecutedOrderPostedDate = new DateTime(1998, 07, 06),
                FacilityName = "A diam maecenas",
                HearingCommentPeriodClosesDate = null,
                HearingDate = null,
                HearingLocation = null,
                IsExecutedOrder = true,
                IsHearingScheduled = false,
                LegalAuthorityId = 7,
                OrderNumber = "TEST-123",
                ProposedOrderPostedDate = null,
                PublicationStatus = PublicationState.Published,
                Requirements = "Duis ut diam quam nulla porttitor massa id neque. A lacus vestibulum sed arcu non. Amet massa vitae tortor condimentum. Magnis dis parturient montes nascetur ridiculus mus mauris. Arcu risus quis varius quam quisque id diam. Pellentesque massa placerat duis ultricies lacus sed. Tellus in hac habitasse platea dictumst vestibulum. Justo nec ultrices dui sapien eget. Ac odio tempor orci dapibus ultrices in. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Donec et odio pellentesque diam volutpat. Nunc faucibus a pellentesque sit amet porttitor eget dolor morbi. Neque ornare aenean euismod elementum nisi quis eleifend quam. Praesent elementum facilisis leo vel fringilla est ullamcorper eget. Et netus et malesuada fames. Urna et pharetra pharetra massa massa ultricies mi quis. Sit amet consectetur adipiscing elit. Felis donec et odio pellentesque diam volutpat commodo sed egestas. Adipiscing elit pellentesque habitant morbi."
                + Environment.NewLine
                + "Duis ut diam quam nulla porttitor massa id neque. A lacus vestibulum sed arcu non. Amet massa vitae tortor condimentum. Magnis dis parturient montes nascetur ridiculus mus mauris. Arcu risus quis varius quam quisque id diam. Pellentesque massa placerat duis ultricies lacus sed. Tellus in hac habitasse platea dictumst vestibulum. Justo nec ultrices dui sapien eget. Ac odio tempor orci dapibus ultrices in. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Donec et odio pellentesque diam volutpat. Nunc faucibus a pellentesque sit amet porttitor eget dolor morbi. Neque ornare aenean euismod elementum nisi quis eleifend quam. Praesent elementum facilisis leo vel fringilla est ullamcorper eget. Et netus et malesuada fames. Urna et pharetra pharetra massa massa ultricies mi quis. Sit amet consectetur adipiscing elit. Felis donec et odio pellentesque diam volutpat commodo sed egestas. Adipiscing elit pellentesque habitant morbi.",
                SettlementAmount = 2000
            };

            var mock = new Mock<IEnforcementOrderRepository>();
            mock.Setup(l => l.IdExists(
                    id,
                    It.IsAny<ISpecification<EnforcementOrder>>()))
                .ReturnsAsync(false);

            var controller = new EnforcementOrdersController(mock.Object);

            var result = await controller.Put(id, target).ConfigureAwait(false);

            result.Should().BeOfType<NotFoundObjectResult>();
            (result as NotFoundObjectResult).StatusCode.Should().Be(404);
            (result as NotFoundObjectResult).Value.Should().Be(id);
        }

        [Fact]
        public async Task UpdateWithDuplicateOrderNumberFails()
        {
            var id = 140;

            var target = new EnforcementOrderUpdate()
            {
                Cause = "Integer feugiat scelerisque varius morbi enim nunc faucibus a.",
                CommentContactId = null,
                CommentPeriodClosesDate = null,
                County = "Liberty",
                ExecutedDate = new DateTime(1998, 06, 29),
                ExecutedOrderPostedDate = new DateTime(1998, 07, 06),
                FacilityName = "A diam maecenas",
                HearingCommentPeriodClosesDate = null,
                HearingDate = null,
                HearingLocation = "",
                IsExecutedOrder = true,
                IsHearingScheduled = false,
                LegalAuthorityId = 7,
                OrderNumber = "TEST-123",
                ProposedOrderPostedDate = null,
                PublicationStatus = PublicationState.Published,
                Requirements = "Duis ut diam quam nulla porttitor massa id neque. A lacus vestibulum sed arcu non. Amet massa vitae tortor condimentum. Magnis dis parturient montes nascetur ridiculus mus mauris. Arcu risus quis varius quam quisque id diam. Pellentesque massa placerat duis ultricies lacus sed. Tellus in hac habitasse platea dictumst vestibulum. Justo nec ultrices dui sapien eget. Ac odio tempor orci dapibus ultrices in. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Donec et odio pellentesque diam volutpat. Nunc faucibus a pellentesque sit amet porttitor eget dolor morbi. Neque ornare aenean euismod elementum nisi quis eleifend quam. Praesent elementum facilisis leo vel fringilla est ullamcorper eget. Et netus et malesuada fames. Urna et pharetra pharetra massa massa ultricies mi quis. Sit amet consectetur adipiscing elit. Felis donec et odio pellentesque diam volutpat commodo sed egestas. Adipiscing elit pellentesque habitant morbi."
                 + Environment.NewLine
                 + "Duis ut diam quam nulla porttitor massa id neque. A lacus vestibulum sed arcu non. Amet massa vitae tortor condimentum. Magnis dis parturient montes nascetur ridiculus mus mauris. Arcu risus quis varius quam quisque id diam. Pellentesque massa placerat duis ultricies lacus sed. Tellus in hac habitasse platea dictumst vestibulum. Justo nec ultrices dui sapien eget. Ac odio tempor orci dapibus ultrices in. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Donec et odio pellentesque diam volutpat. Nunc faucibus a pellentesque sit amet porttitor eget dolor morbi. Neque ornare aenean euismod elementum nisi quis eleifend quam. Praesent elementum facilisis leo vel fringilla est ullamcorper eget. Et netus et malesuada fames. Urna et pharetra pharetra massa massa ultricies mi quis. Sit amet consectetur adipiscing elit. Felis donec et odio pellentesque diam volutpat commodo sed egestas. Adipiscing elit pellentesque habitant morbi.",
                SettlementAmount = 2000
            };

            var updateResult = new UpdateEntityResult();
            updateResult.AddErrorMessage("OrderNumber", "An Order with the same number already exists.");

            var mock = new Mock<IEnforcementOrderRepository>();
            mock.Setup(l => l.IdExists(
                    id,
                    It.IsAny<ISpecification<EnforcementOrder>>()))
                .ReturnsAsync(true);

            mock.Setup(l => l.OrderNumberExists(target.OrderNumber, id))
                .ReturnsAsync(false);
            mock.Setup(l => l.UpdateEnforcementOrderAsync(
                    id,
                    It.IsAny<string>(),
                    It.IsAny<int?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<int?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<PublicationState>(),
                    It.IsAny<string>(),
                    It.IsAny<decimal?>()))
                .ReturnsAsync(updateResult);

            var controller = new EnforcementOrdersController(mock.Object);

            var result = await controller.Put(id, target).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            (result as BadRequestObjectResult).StatusCode.Should().Be(400);
            ((result as BadRequestObjectResult).Value as SerializableError)
            .Should().HaveCount(1);
        }

        [Fact]
        public async Task DeleteItemReturnsCorrectly()
        {
            var id = 140;
            var item = new EnforcementOrder
            {
                Id = 140,
                County = "Liberty",
                Deleted = false,
                FacilityName = "A diam maecenas",
                IsExecutedOrder = true,
                LegalAuthorityId = 7,
                OrderNumber = "TEST-123",
                PublicationStatus = PublicationState.Published
            };

            var mock = new Mock<IEnforcementOrderRepository>();
            mock.Setup(l => l.GetByIdAsync(id, null, null))
                .ReturnsAsync(item);

            var controller = new EnforcementOrdersController(mock.Object);

            var result = await controller.Delete(140).ConfigureAwait(false);

            result.Should().BeOfType<NoContentResult>();
            var actionResult = result as NoContentResult;
            actionResult.StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task UnDeleteItemReturnsCorrectly()
        {
            var id = 58310;
            var item = new EnforcementOrder
            {
                Id = 140,
                County = "Liberty",
                Deleted = true,
                FacilityName = "A diam maecenas",
                IsExecutedOrder = true,
                LegalAuthorityId = 7,
                OrderNumber = "TEST-123",
                PublicationStatus = PublicationState.Published
            };

            var mock = new Mock<IEnforcementOrderRepository>();
            mock.Setup(l => l.GetByIdAsync(id, null, null))
                .ReturnsAsync(item);
            mock.Setup(l => l.CompleteAsync())
                .ReturnsAsync(1);

            var controller = new EnforcementOrdersController(mock.Object);

            var result = await controller.Undelete(id).ConfigureAwait(false);

            result.Should().BeOfType<NoContentResult>();
            var actionResult = result as NoContentResult;
            actionResult.StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task DeleteOfAlreadyDeletedOrderFails()
        {
            var id = 140;
            var item = new EnforcementOrder
            {
                Id = 140,
                County = "Liberty",
                Deleted = true,
                FacilityName = "A diam maecenas",
                IsExecutedOrder = true,
                LegalAuthorityId = 7,
                OrderNumber = "TEST-123",
                PublicationStatus = PublicationState.Published
            };

            var mock = new Mock<IEnforcementOrderRepository>();
            mock.Setup(l => l.GetByIdAsync(id, null, null))
                .ReturnsAsync(item);

            var controller = new EnforcementOrdersController(mock.Object);

            var result = await controller.Delete(id).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            var actionResult = result as BadRequestObjectResult;
            actionResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task UndeleteOfNonDeletedOrderFails()
        {
            var id = 140;
            var item = new EnforcementOrder
            {
                Id = 140,
                County = "Liberty",
                Deleted = false,
                FacilityName = "A diam maecenas",
                IsExecutedOrder = true,
                LegalAuthorityId = 7,
                OrderNumber = "TEST-123",
                PublicationStatus = PublicationState.Published
            };

            var mock = new Mock<IEnforcementOrderRepository>();
            mock.Setup(l => l.GetByIdAsync(id, null, null))
                .ReturnsAsync(item);

            var controller = new EnforcementOrdersController(mock.Object);

            var result = await controller.Undelete(140).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            var actionResult = result as BadRequestObjectResult;
            actionResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task DeleteWithNonexistantIdFails()
        {
            int id = -1;

            var mock = new Mock<IEnforcementOrderRepository>();
            mock.Setup(l => l.GetByIdAsync(id, null, null))
                .ReturnsAsync((EnforcementOrder)null);

            var controller = new EnforcementOrdersController(mock.Object);

            var result = await controller.Delete(id).ConfigureAwait(false);

            result.Should().BeOfType<NotFoundObjectResult>();
            (result as NotFoundObjectResult).StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task UndeleteWithNonexistantIdFails()
        {
            int id = -1;

            var mock = new Mock<IEnforcementOrderRepository>();
            mock.Setup(l => l.GetByIdAsync(id, null, null))
                .ReturnsAsync((EnforcementOrder)null);

            var controller = new EnforcementOrdersController(mock.Object);

            var result = await controller.Undelete(id).ConfigureAwait(false);

            result.Should().BeOfType<NotFoundObjectResult>();
            (result as NotFoundObjectResult).StatusCode.Should().Be(404);
        }
    }
}
