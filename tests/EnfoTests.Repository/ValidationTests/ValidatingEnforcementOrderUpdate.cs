using System.Linq;
using Enfo.Repository.Resources.EnforcementOrder;
using FluentAssertions;
using EnfoTests.Helpers;
using Xunit;
using Enfo.Repository.Mapping;
using static Enfo.Repository.Validation.EnforcementOrderValidation;
using static EnfoTests.Helpers.ResourceHelper;

namespace EnfoTests.Repository.ValidationTests
{
    public class ValidatingEnforcementOrderUpdate
    {
        [Fact]
        public void SucceedsGivenValidUpdates()
        {
            ValidateEnforcementOrderUpdate(GetEnforcementOrderAdminView(1), GetValidEnforcementOrderUpdate())
                .IsValid.Should().BeTrue();
        }

        [Fact]
        public void FailsGivenDeletedOrder()
        {
            var order = DataHelper.GetEnforcementOrders.First(e => e.Deleted);
            var result = ValidateEnforcementOrderUpdate(new EnforcementOrderAdminView(order),
                GetValidEnforcementOrderUpdate());

            result.IsValid.Should().BeFalse();
            result.ErrorMessages.Should().NotBeEmpty()
                .And.HaveCount(1)
                .And.ContainKey("Id");
        }

        [Fact]
        public void SucceedsWhenRemovingExecutedOrderGivenProposedOrder()
        {
            var order = GetEnforcementOrderAdminView(DataHelper.GetEnforcementOrders.First(e => e.IsProposedOrder).Id);

            var orderUpdate = EnforcementOrderMapping.ToEnforcementOrderUpdate(order);
            orderUpdate.ExecutedDate = null;
            orderUpdate.ExecutedOrderPostedDate = null;
            orderUpdate.IsExecutedOrder = false;

            var result = ValidateEnforcementOrderUpdate(order, orderUpdate);

            result.IsValid.Should().BeTrue();
            result.ErrorMessages.Should().BeEmpty();
        }

        [Fact]
        public void FailsWhenRemovingExecutedOrderIfNotProposedOrder()
        {
            var order = GetEnforcementOrderAdminView(DataHelper.GetEnforcementOrders.First(e => !e.IsProposedOrder).Id);
            var orderUpdate = EnforcementOrderMapping.ToEnforcementOrderUpdate(order);
            orderUpdate.ExecutedDate = null;
            orderUpdate.ExecutedOrderPostedDate = null;
            orderUpdate.IsExecutedOrder = false;

            var result = ValidateEnforcementOrderUpdate(order, orderUpdate);

            result.IsValid.Should().BeFalse();
            result.ErrorMessages.Should().NotBeEmpty()
                .And.HaveCount(1)
                .And.ContainKey("IsExecutedOrder");
        }

        [Fact]
        public void FailsWhenRemovingExecutedOrderDetails()
        {
            var orderUpdate = GetValidEnforcementOrderUpdate();
            orderUpdate.ExecutedDate = null;
            orderUpdate.ExecutedOrderPostedDate = null;

            var result = ValidateEnforcementOrderUpdate(GetEnforcementOrderAdminView(1), orderUpdate);

            result.IsValid.Should().BeFalse();
            result.ErrorMessages.Should().NotBeEmpty()
                .And.HaveCount(2)
                .And.ContainKeys("ExecutedDate", "ExecutedOrderPostedDate");
        }

        [Fact]
        public void SucceedsWhenRemovingHearing()
        {
            var orderUpdate = GetValidEnforcementOrderUpdate();
            orderUpdate.IsHearingScheduled = false;

            var result = ValidateEnforcementOrderUpdate(GetEnforcementOrderAdminView(1), orderUpdate);

            result.IsValid.Should().BeTrue();
            result.ErrorMessages.Should().BeEmpty();
        }

        [Fact]
        public void FailsWhenRemovingHearingDetailsIfHearingStillTrue()
        {
            var orderUpdate = GetValidEnforcementOrderUpdate();
            orderUpdate.HearingCommentPeriodClosesDate = null;
            orderUpdate.HearingContactId = null;
            orderUpdate.HearingDate = null;
            orderUpdate.HearingLocation = null;

            var result = ValidateEnforcementOrderUpdate(GetEnforcementOrderAdminView(1), orderUpdate);

            result.IsValid.Should().BeFalse();
            result.ErrorMessages.Should().NotBeEmpty()
                .And.HaveCount(4)
                .And.ContainKeys(
                    "HearingDate",
                    "HearingLocation",
                    "HearingCommentPeriodClosesDate",
                    "HearingContactId");
        }
    }
}