using System;
using System.Linq;
using System.Threading.Tasks;
using Enfo.Domain.Entities;
using Enfo.Repository.Mapping;
using Enfo.Repository.Resources.EnforcementOrder;
using Enfo.Repository.Specs;
using FluentAssertions;
using Xunit;
using static Enfo.Repository.Utils.DateUtils;
using static Enfo.Infrastructure.Tests.RepositoryHelper;
using static Enfo.Infrastructure.Tests.RepositoryHelperData;

namespace Enfo.Infrastructure.Tests
{
    public class EnforcementOrderRepositoryTests
    {
        // Data helpers
        private static EnforcementOrderDetailedView GetEnforcementOrderView(int id) =>
            new EnforcementOrderDetailedView(FillNavigationProperties(GetEnforcementOrders.Single(e => e.Id == id)));

        private static EnforcementOrderDetailedView GetEnforcementOrderAdminView(int id) =>
            new EnforcementOrderAdminView(FillNavigationProperties(GetEnforcementOrders.Single(e => e.Id == id)));

        private static EnforcementOrderSummaryView GetEnforcementOrderSummaryView(int id) =>
            new EnforcementOrderSummaryView(FillNavigationProperties(GetEnforcementOrders.Single(e => e.Id == id)));

        private static EnforcementOrder FillNavigationProperties(EnforcementOrder order)
        {
            order.LegalAuthority = GetLegalAuthorities.SingleOrDefault(e => e.Id == order.LegalAuthorityId);
            order.CommentContact = GetEpdContacts.SingleOrDefault(e => e.Id == order.CommentContactId);
            if (order.CommentContact != null)
                order.CommentContact.Address =
                    GetAddresses.SingleOrDefault(e => e.Id == order.CommentContact.AddressId);
            order.HearingContact = GetEpdContacts.SingleOrDefault(e => e.Id == order.HearingContactId);
            if (order.HearingContact != null)
                order.HearingContact.Address =
                    GetAddresses.SingleOrDefault(e => e.Id == order.HearingContact.AddressId);

            return order;
        }

        // GetAsync

        [Fact]
        public async Task Get_ReturnsItem()
        {
            var itemId = GetEnforcementOrders.First(e => e.GetIsPublic()).Id;

            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
            (await repository.GetAsync(itemId))
                .Should().BeEquivalentTo(GetEnforcementOrderView(itemId));
        }

        [Fact]
        public async Task Get_GivenMissingId_ReturnsNull()
        {
            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
            (await repository.GetAsync(-1)).Should().BeNull();
        }

        [Fact]
        public async Task Get_GivenNonpublicOrder_ReturnsNull()
        {
            var itemId = GetEnforcementOrders.First(e => !e.GetIsPublic()).Id;

            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
            (await repository.GetAsync(itemId)).Should().BeNull();
        }

        [Fact]
        public async Task Get_GivenNonpublicOrderButAllowed_ReturnsItem()
        {
            var itemId = GetEnforcementOrders.First(e => !e.GetIsPublic()).Id;

            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
            (await repository.GetAsync(itemId, false))
                .Should().BeEquivalentTo(GetEnforcementOrderView(itemId));
        }

        // GetAdminViewAsync

        [Fact]
        public async Task GetAdminView_ReturnsItem()
        {
            var itemId = GetEnforcementOrders.First(e => e.GetIsPublic()).Id;

            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
            (await repository.GetAdminViewAsync(itemId))
                .Should().BeEquivalentTo(GetEnforcementOrderAdminView(itemId));
        }

        [Fact]
        public async Task GetAdminView_GivenMissingId_ReturnsNull()
        {
            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
            (await repository.GetAdminViewAsync(-1)).Should().BeNull();
        }

        [Fact]
        public async Task GetAdminView_GivenNonpublicOrder_ReturnsItem()
        {
            var itemId = GetEnforcementOrders.First(e => !e.GetIsPublic()).Id;

            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
            (await repository.GetAdminViewAsync(itemId))
                .Should().BeEquivalentTo(GetEnforcementOrderAdminView(itemId));
        }

        // ListAsync

        [Fact]
        public async Task List_ByDefault_ReturnsOnlyPublic()
        {
            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
            var result = await repository.ListAsync(new EnforcementOrderSpec(), new PaginationSpec());

            result.CurrentCount.Should().Be(GetEnforcementOrders.Count(e => e.GetIsPublic()));
            result.Items.Should().HaveCount(GetEnforcementOrders.Count(e => e.GetIsPublic()));
            result.PageNumber.Should().Be(1);
            result.Items[0].Should().BeEquivalentTo(
                GetEnforcementOrderSummaryView(GetEnforcementOrders
                    .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
                    .ThenBy(e => e.FacilityName.Trim().Trim('\n', '\r', '\t'))
                    .First(e => e.GetIsPublic()).Id));
        }

        [Fact]
        public async Task List_WithSpecShowDeleted_ReturnsOnlyDeleted()
        {
            var spec = new EnforcementOrderSpec {ShowDeleted = true};

            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
            var result = await repository.ListAsync(spec, new PaginationSpec());

            result.CurrentCount.Should().Be(GetEnforcementOrders.Count(e => e.Deleted));
            result.Items.Should().HaveCount(GetEnforcementOrders.Count(e => e.Deleted));
            result.PageNumber.Should().Be(1);
            result.Items[0].Should().BeEquivalentTo(
                GetEnforcementOrderSummaryView(GetEnforcementOrders
                    .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
                    .ThenBy(e => e.FacilityName.Trim().Trim('\n', '\r', '\t'))
                    .First(e => e.Deleted).Id));
        }

        [Fact]
        public async Task List_IncludeNonPublic_ReturnsAllActive()
        {
            var spec = new EnforcementOrderSpec {OnlyPublic = false};

            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
            var result = await repository.ListAsync(spec, new PaginationSpec());

            result.CurrentCount.Should().Be(GetEnforcementOrders.Count(e => !e.Deleted));
            result.Items.Should().HaveCount(GetEnforcementOrders.Count(e => !e.Deleted));
            result.PageNumber.Should().Be(1);
            result.Items[0].Should().BeEquivalentTo(
                GetEnforcementOrderSummaryView(GetEnforcementOrders
                    .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
                    .ThenBy(e => e.FacilityName.Trim().Trim('\n', '\r', '\t'))
                    .First(e => !e.Deleted).Id));
        }

        [Fact]
        public async Task List_WithFacilityNameSpec_ReturnsMatches()
        {
            var spec = new EnforcementOrderSpec
            {
                FacilityFilter = GetEnforcementOrders.First(e => !e.Deleted).FacilityName
            };

            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
            var result = await repository.ListAsync(spec, new PaginationSpec());

            var expectedList = GetEnforcementOrders
                .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
                .ThenBy(e => e.FacilityName.Trim().Trim('\n', '\r', '\t'))
                .Where(e =>
                    !e.Deleted
                    && string.Equals(e.FacilityName, spec.FacilityFilter, StringComparison.InvariantCulture))
                .ToList();

            result.CurrentCount.Should().Be(expectedList.Count);
            result.Items.Should().HaveCount(expectedList.Count);
            result.PageNumber.Should().Be(1);
            result.Items[0].Should().BeEquivalentTo(
                GetEnforcementOrderSummaryView(expectedList[0].Id));
        }

        // CountAsync

        [Fact]
        public async Task Count_ByDefault_CountsOnlyPublic()
        {
            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
            (await repository.CountAsync(new EnforcementOrderSpec()))
                .Should().Be(GetEnforcementOrders.Count(e => e.GetIsPublic()));
        }

        [Fact]
        public async Task Count_WithSpecShowDeleted_CountsOnlyDeleted()
        {
            var spec = new EnforcementOrderSpec {ShowDeleted = true};

            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
            (await repository.CountAsync(spec))
                .Should().Be(GetEnforcementOrders.Count(e => e.Deleted));
        }

        [Fact]
        public async Task Count_IncludeNonPublic_CountsAllActive()
        {
            var spec = new EnforcementOrderSpec {OnlyPublic = false};

            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
            (await repository.CountAsync(spec))
                .Should().Be(GetEnforcementOrders.Count(e => !e.Deleted));
        }

        [Fact]
        public async Task Count_WithFacilityNameSpec_CountsMatches()
        {
            // NOTE: Sqlite, used for these tests, is case-sensitive by default
            // so case-insensitive testing is not possible. SQL Server is
            // case-insensitive by default.
            var spec = new EnforcementOrderSpec
            {
                FacilityFilter = GetEnforcementOrders.First(e => !e.Deleted).FacilityName
            };

            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
            (await repository.CountAsync(spec))
                .Should().Be(GetEnforcementOrders.Count(e =>
                    !e.Deleted
                    && string.Equals(e.FacilityName, spec.FacilityFilter, StringComparison.InvariantCulture)));
        }

        // ExistsAsync

        [Fact]
        public async Task Exists_GivenExists_ReturnsTrue()
        {
            var itemId = GetEnforcementOrders.First(e => e.GetIsPublic()).Id;
            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
            (await repository.ExistsAsync(itemId)).Should().BeTrue();
        }

        [Fact]
        public async Task Exists_GivenNotExists_ReturnsFalse()
        {
            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
            (await repository.ExistsAsync(-1)).Should().BeFalse();
        }

        [Fact]
        public async Task Exists_GivenNonpublic_ReturnsFalse()
        {
            var itemId = GetEnforcementOrders.First(e => !e.GetIsPublic()).Id;
            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
            (await repository.ExistsAsync(itemId)).Should().BeFalse();
        }

        [Fact]
        public async Task Exists_GivenNonpublicButAllowed_ReturnsTrue()
        {
            var itemId = GetEnforcementOrders.First(e => !e.GetIsPublic()).Id;
            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
            (await repository.ExistsAsync(itemId, false)).Should().BeTrue();
        }

        // OrderNumberExists

        [Fact]
        public async Task OrderNumberExists_GivenExists_ReturnsTrue()
        {
            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
            (await repository.OrderNumberExistsAsync(GetEnforcementOrders[0].OrderNumber))
                .Should().BeTrue();
        }

        [Fact]
        public async Task OrderNumberExists_GivenNotExists_ReturnsFalse()
        {
            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
            (await repository.OrderNumberExistsAsync(Guid.NewGuid().ToString()))
                .Should().BeFalse();
        }

        [Fact]
        public async Task OrderNumberExists_GivenExistsAndIgnore_ReturnsFalse()
        {
            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
            (await repository.OrderNumberExistsAsync(GetEnforcementOrders[0].OrderNumber, GetEnforcementOrders[0].Id))
                .Should().BeFalse();
        }

        // ListCurrentProposedEnforcementOrders

        [Fact]
        public async Task ListCurrentProposedEnforcementOrders_ReturnsCorrectly()
        {
            var order = GetEnforcementOrders.First(e => e.GetIsPublic() && e.IsProposedOrder);
            order.CommentPeriodClosesDate = DateTime.Today.AddDays(1);

            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
            var result = await repository.ListCurrentProposedEnforcementOrdersAsync();

            result.Count.Should().Be(GetEnforcementOrders.Count(e =>
                e.GetIsPublic() && e.IsProposedOrder && e.CommentPeriodClosesDate >= DateTime.Today));
            result[0].Should().BeEquivalentTo(GetEnforcementOrderSummaryView(order.Id));
        }

        // ListDraftEnforcementOrders

        [Fact]
        public async Task ListDraftEnforcementOrders_ReturnsCorrectly()
        {
            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
            var result = await repository.ListDraftEnforcementOrdersAsync();

            result.Count.Should().Be(GetEnforcementOrders.Count(e =>
                !e.Deleted && e.PublicationStatus == EnforcementOrder.PublicationState.Draft));
            result[0].Should().BeEquivalentTo(
                GetEnforcementOrderSummaryView(GetEnforcementOrders.First(e =>
                    !e.Deleted && e.PublicationStatus == EnforcementOrder.PublicationState.Draft).Id));
        }

        // ListPendingEnforcementOrders

        [Fact]
        public async Task ListPendingEnforcementOrders_ReturnsCorrectly()
        {
            var order = GetEnforcementOrders.First(e => e.GetIsPublicProposedOrder() || e.GetIsPublicExecutedOrder());
            order.ExecutedDate = DateTime.Today.AddDays(8);

            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
            var result = await repository.ListPendingEnforcementOrdersAsync();

            result.Count.Should().Be(GetEnforcementOrders.Count(e =>
                (e.GetIsPublicProposedOrder() || e.GetIsPublicExecutedOrder())
                && e.GetLastPostedDate() > MostRecentMonday()));
            result[0].Should().BeEquivalentTo(GetEnforcementOrderSummaryView(order.Id));
        }

        // ListRecentlyExecutedEnforcementOrders

        [Fact]
        public async Task ListRecentlyExecutedEnforcementOrders_ReturnsCorrectly()
        {
            var order = GetEnforcementOrders.First(e => e.GetIsPublicExecutedOrder());
            order.ExecutedOrderPostedDate = MostRecentMonday();

            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
            var result = await repository.ListRecentlyExecutedEnforcementOrdersAsync();

            result.Count.Should().Be(
                GetEnforcementOrders.Count(e =>
                    e.GetIsPublicExecutedOrder()
                    && e.ExecutedOrderPostedDate >= MostRecentMonday()
                    && e.ExecutedOrderPostedDate <= DateTime.Today));
            result[0].Should().BeEquivalentTo(GetEnforcementOrderSummaryView(order.Id));
        }

        // Write Methods

        private readonly EnforcementOrder _oldSampleOrder = new EnforcementOrder
        {
            Cause = "Integer feugiat scelerisque varius morbi enim nunc faucibus a.",
            CommentContactId = 2004,
            CommentPeriodClosesDate = new DateTime(2012, 11, 15),
            County = "Liberty",
            Deleted = false,
            ExecutedDate = new DateTime(1998, 06, 29),
            ExecutedOrderPostedDate = new DateTime(1998, 07, 06),
            FacilityName = "A diam maecenas",
            HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
            HearingContactId = 2004,
            HearingDate = new DateTime(2012, 11, 15),
            HearingLocation = "venenatis urna cursus in aliquam sem",
            IsExecutedOrder = true,
            IsHearingScheduled = true,
            IsProposedOrder = true,
            LegalAuthorityId = 7,
            OrderNumber = "EPD-NEW-9999",
            ProposedOrderPostedDate = new DateTime(2012, 10, 16),
            PublicationStatus = EnforcementOrder.PublicationState.Published,
            Requirements = "Duis ut diam quam nulla porttitor massa id neque.",
            SettlementAmount = 2000
        };

        // CreateAsync

        // Sample data for create
        private readonly EnforcementOrderCreate _sampleCreate = new EnforcementOrderCreate
        {
            FacilityName = "abc",
            County = "Fulton",
            LegalAuthorityId = GetLegalAuthorities.First().Id,
            PublicationStatus = PublicationState.Draft,
            OrderNumber = "NEW-1",
            CreateAs = EnforcementOrderCreate.NewEnforcementOrderType.Proposed,
            CommentPeriodClosesDate = DateTime.Today.AddDays(1),
            CommentContactId = GetEpdContacts.First().Id,
            ProposedOrderPostedDate = DateTime.Today,
        };

        [Fact]
        public async Task Create_AddsNewItem()
        {
            int newId;
            using var repositoryHelper = CreateRepositoryHelper();

            using (var repository = repositoryHelper.GetEnforcementOrderRepository())
            {
                newId = await repository.CreateAsync(_sampleCreate);
            }

            using (var repository = repositoryHelper.GetEnforcementOrderRepository())
            {
                var expected = FillNavigationProperties(_sampleCreate.ToEnforcementOrder());
                (await repository.GetAsync(newId)).Should().BeEquivalentTo(expected);
            }
        }

        [Fact]
        public async Task CreateOrder_WithDuplicateOrderNumber_Fails()
        {
            _sampleCreate.OrderNumber = GetEnforcementOrders.First(e => !e.Deleted).OrderNumber;

            Func<Task> action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
                await repository.CreateAsync(_sampleCreate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .And.ParamName.Should().Be(nameof(_sampleCreate.OrderNumber));
        }

        // UpdateAsync

        // Sample data for update
        private static EnforcementOrderUpdate NewSampleUpdate(EnforcementOrder order) => new EnforcementOrderUpdate
        {
            Cause = order.Cause,
            County = order.County,
            Requirements = order.Requirements,
            ExecutedDate = order.ExecutedDate,
            FacilityName = order.FacilityName,
            HearingDate = order.HearingDate,
            HearingLocation = order.HearingLocation,
            OrderNumber = order.OrderNumber,
            PublicationStatus =
                order.PublicationStatus == EnforcementOrder.PublicationState.Draft
                    ? PublicationState.Draft
                    : PublicationState.Published,
            SettlementAmount = order.SettlementAmount,
            CommentContactId = order.CommentContactId,
            HearingContactId = order.HearingContactId,
            IsExecutedOrder = order.IsExecutedOrder,
            IsHearingScheduled = order.IsHearingScheduled,
            LegalAuthorityId = order.LegalAuthorityId,
            CommentPeriodClosesDate = order.CommentPeriodClosesDate,
            ExecutedOrderPostedDate = order.ExecutedOrderPostedDate,
            ProposedOrderPostedDate = order.ProposedOrderPostedDate,
            HearingCommentPeriodClosesDate = order.HearingCommentPeriodClosesDate,
        };

        [Fact]
        public async Task Update_Succeeds()
        {
            var existingOrder = GetEnforcementOrders.First(e => !e.Deleted);
            var itemId = existingOrder.Id;

            var itemUpdate = NewSampleUpdate(existingOrder);
            itemUpdate.Cause = "abc";

            using var repositoryHelper = CreateRepositoryHelper();

            using (var repository = repositoryHelper.GetEnforcementOrderRepository())
            {
                await repository.UpdateAsync(itemId, itemUpdate);
            }

            using (var repository = repositoryHelper.GetEnforcementOrderRepository())
            {
                (await repository.GetAsync(itemId)).Cause.Should().Be("abc");
            }
        }

        [Fact]
        public async Task Update_DeletedOrder_ThrowsException()
        {
            var existingOrder = GetEnforcementOrders.First(e => e.Deleted);
            var itemId = existingOrder.Id;
            var itemUpdate = NewSampleUpdate(existingOrder);

            Func<Task> action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
                await repository.UpdateAsync(itemId, itemUpdate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .WithMessage($"ID ({itemId}) is deleted. (Parameter 'id')");
        }

        [Fact]
        public async Task Update_WithDuplicateOrderNumber_Fails()
        {
            var existingOrder = GetEnforcementOrders.First(e => !e.Deleted);
            var itemId = existingOrder.Id;

            var itemUpdate = NewSampleUpdate(existingOrder);
            itemUpdate.OrderNumber = GetEnforcementOrders.Last(e => !e.Deleted).OrderNumber;

            Func<Task> action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
                await repository.UpdateAsync(itemId, itemUpdate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .And.ParamName.Should().Be(nameof(_sampleCreate.OrderNumber));
        }

        // DeleteAsync

        [Fact]
        public async Task Delete_Succeeds()
        {
            var itemId = GetEnforcementOrders.First(e => !e.Deleted).Id;

            using var repositoryHelper = CreateRepositoryHelper();

            using (var repository = repositoryHelper.GetEnforcementOrderRepository())
            {
                await repository.DeleteAsync(itemId);
            }

            using (var repository = repositoryHelper.GetEnforcementOrderRepository())
            {
                (await repository.GetAdminViewAsync(itemId))
                    .Deleted.Should().BeTrue();
            }
        }

        [Fact]
        public async Task Delete_AlreadyDeletedItem_DoesNotChange()
        {
            var itemId = GetEnforcementOrders.First(e => e.Deleted).Id;

            using var repositoryHelper = CreateRepositoryHelper();

            using (var repository = repositoryHelper.GetEnforcementOrderRepository())
            {
                await repository.DeleteAsync(itemId);
            }

            using (var repository = repositoryHelper.GetEnforcementOrderRepository())
            {
                (await repository.GetAdminViewAsync(itemId))
                    .Deleted.Should().BeTrue();
            }
        }

        // RestoreAsync

        [Fact]
        public async Task Restore_Succeeds()
        {
            var itemId = GetEnforcementOrders.First(e => e.Deleted).Id;

            using var repositoryHelper = CreateRepositoryHelper();

            using (var repository = repositoryHelper.GetEnforcementOrderRepository())
            {
                await repository.RestoreAsync(itemId);
            }

            using (var repository = repositoryHelper.GetEnforcementOrderRepository())
            {
                (await repository.GetAdminViewAsync(itemId))
                    .Deleted.Should().BeFalse();
            }
        }

        [Fact]
        public async Task Restore_AlreadyDeletedItem_DoesNotChange()
        {
            var itemId = GetEnforcementOrders.First(e => !e.Deleted).Id;

            using var repositoryHelper = CreateRepositoryHelper();

            using (var repository = repositoryHelper.GetEnforcementOrderRepository())
            {
                await repository.RestoreAsync(itemId);
            }

            using (var repository = repositoryHelper.GetEnforcementOrderRepository())
            {
                (await repository.GetAdminViewAsync(itemId))
                    .Deleted.Should().BeFalse();
            }
        }
    }
}