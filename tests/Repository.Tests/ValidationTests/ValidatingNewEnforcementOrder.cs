using System;
using Enfo.Repository.Resources.EnforcementOrder;
using FluentAssertions;
using Xunit;
using static Enfo.Repository.Validation.EnforcementOrderValidation;

namespace Repository.Tests.ValidationTests
{
    public class ValidatingNewEnforcementOrder
    {
        private readonly EnforcementOrderCreate _order = new EnforcementOrderCreate()
        {
            Cause = "Integer feugiat scelerisque varius morbi enim nunc faucibus.",
            CommentContactId = 2004,
            CommentPeriodClosesDate = new DateTime(2012, 11, 15),
            County = "Liberty",
            ExecutedDate = new DateTime(1998, 06, 29),
            ExecutedOrderPostedDate = new DateTime(1998, 07, 06),
            FacilityName = "A diam maecenas",
            HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
            HearingContactId = 2004,
            HearingDate = new DateTime(2012, 11, 15),
            HearingLocation = "venenatis urna cursus viverra mauris in aliquam sem",
            IsHearingScheduled = true,
            LegalAuthorityId = 7,
            OrderNumber = "EPD-ACQ-7936",
            ProposedOrderPostedDate = new DateTime(2012, 10, 16),
            PublicationStatus = PublicationState.Published,
            Requirements = "Duis ut diam quam nulla porttitor massa id neque. A lacus vestibulum.",
            SettlementAmount = 2000,
        };

        [Fact]
        public void SucceedsGivenValidOrder()
        {
            var result = ValidateNewEnforcementOrder(_order);

            result.IsValid.Should().BeTrue();
            result.ErrorMessages.Should().BeEmpty();
        }

        [Fact]
        public void FailsIfMissingRequiredProperties()
        {
            _order.CommentContactId = null;
            _order.CommentPeriodClosesDate = null;
            _order.ProposedOrderPostedDate = null;

            var result = ValidateNewEnforcementOrder(_order);

            result.IsValid.Should().BeFalse();
            result.ErrorMessages.Should().NotBeEmpty()
                .And.HaveCount(3)
                .And.ContainKeys(
                    "CommentContact",
                    "CommentPeriodClosesDate",
                    "ProposedOrderPostedDate");
        }
    }
}