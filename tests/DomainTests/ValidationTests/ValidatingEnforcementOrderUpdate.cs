using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Resources.Validation;
using FluentValidation.TestHelper;

namespace EnfoTests.Domain.ValidationTests;

[TestFixture]
public class ValidatingEnforcementOrderUpdate
{
    [Test]
    public async Task SucceedsGivenValidUpdates()
    {
        var model = new EnforcementOrderUpdate
        {
            Cause = "uvw-" + Guid.NewGuid(),
            CommentContactId = 2000,
            CommentPeriodClosesDate = new DateTime(2012, 11, 15),
            County = "Liberty",
            ExecutedDate = new DateTime(1998, 06, 29),
            ExecutedOrderPostedDate = new DateTime(1998, 07, 06),
            FacilityName = "uvw-" + Guid.NewGuid(),
            HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
            HearingContactId = 2000,
            HearingDate = new DateTime(2012, 11, 15),
            HearingLocation = "uvw-" + Guid.NewGuid(),
            IsExecutedOrder = true,
            IsHearingScheduled = true,
            IsProposedOrder = true,
            LegalAuthorityId = 1,
            OrderNumber = "EPD-ACQ-7936",
            Progress = PublicationProgress.Published,
            ProposedOrderPostedDate = new DateTime(2012, 10, 16),
            Requirements = "uvw-" + Guid.NewGuid(),
            SettlementAmount = 2000,
        };

        var validator = new EnforcementOrderUpdateValidator(Substitute.For<IEnforcementOrderRepository>());

        var result = await validator.TestValidateAsync(model);

        using (new AssertionScope())
        {
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }

    [Test]
    public async Task SucceedsWhenRemovingExecutedOrderGivenProposedOrder()
    {
        var model = new EnforcementOrderUpdate
        {
            Cause = "uvw-" + Guid.NewGuid(),
            CommentContactId = 2000,
            CommentPeriodClosesDate = new DateTime(2012, 11, 15),
            County = "Liberty",
            ExecutedDate = null,
            ExecutedOrderPostedDate = null,
            FacilityName = "uvw-" + Guid.NewGuid(),
            IsExecutedOrder = false,
            IsProposedOrder = true,
            OrderNumber = "EPD-ACQ-7936",
            Progress = PublicationProgress.Published,
            ProposedOrderPostedDate = new DateTime(2012, 10, 16),
            Requirements = "uvw-" + Guid.NewGuid(),
            SettlementAmount = 2000,
        };

        var validator = new EnforcementOrderUpdateValidator(Substitute.For<IEnforcementOrderRepository>());

        var result = await validator.TestValidateAsync(model);

        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task FailsWhenRemovingExecutedOrderIfNotProposedOrder()
    {
        var model = new EnforcementOrderUpdate
        {
            Cause = "uvw-" + Guid.NewGuid(),
            County = "Liberty",
            ExecutedDate = null,
            ExecutedOrderPostedDate = null,
            FacilityName = "uvw-" + Guid.NewGuid(),
            IsExecutedOrder = false,
            IsProposedOrder = false,
            OrderNumber = "EPD-ACQ-7936",
            Progress = PublicationProgress.Published,
            Requirements = "uvw-" + Guid.NewGuid(),
            SettlementAmount = 2000,
        };

        var validator = new EnforcementOrderUpdateValidator(Substitute.For<IEnforcementOrderRepository>());

        var result = await validator.TestValidateAsync(model);

        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(e => e.IsProposedOrder);
    }

    [Test]
    public async Task FailsWhenRemovingExecutedOrderDetailsIfExecuted()
    {
        var model = new EnforcementOrderUpdate
        {
            Cause = "uvw-" + Guid.NewGuid(),
            County = "Liberty",
            ExecutedDate = null,
            ExecutedOrderPostedDate = null,
            FacilityName = "uvw-" + Guid.NewGuid(),
            IsExecutedOrder = true,
            OrderNumber = "EPD-ACQ-7936",
            Progress = PublicationProgress.Published,
            Requirements = "uvw-" + Guid.NewGuid(),
            SettlementAmount = 2000,
        };

        var validator = new EnforcementOrderUpdateValidator(Substitute.For<IEnforcementOrderRepository>());

        var result = await validator.TestValidateAsync(model);

        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(e => e.ExecutedDate);
            result.ShouldHaveValidationErrorFor(e => e.ExecutedOrderPostedDate);
        }
    }

    [Test]
    public async Task FailsWhenRemovingHearingDetailsIfHearing()
    {
        var model = new EnforcementOrderUpdate
        {
            Cause = "uvw-" + Guid.NewGuid(),
            County = "Liberty",
            ExecutedDate = new DateTime(1998, 06, 29),
            ExecutedOrderPostedDate = new DateTime(1998, 07, 06),
            FacilityName = "uvw-" + Guid.NewGuid(),
            IsExecutedOrder = true,
            IsHearingScheduled = true,
            OrderNumber = "EPD-ACQ-7936",
            Progress = PublicationProgress.Published,
            Requirements = "uvw-" + Guid.NewGuid(),
            SettlementAmount = 2000,
        };

        var validator = new EnforcementOrderUpdateValidator(Substitute.For<IEnforcementOrderRepository>());

        var result = await validator.TestValidateAsync(model);

        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(e => e.HearingDate);
            result.ShouldHaveValidationErrorFor(e => e.HearingLocation);
            result.ShouldHaveValidationErrorFor(e => e.HearingCommentPeriodClosesDate);
            result.ShouldHaveValidationErrorFor(e => e.HearingContactId);
        }
    }
}
