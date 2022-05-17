using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Resources.Validation;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using static EnfoTests.Helpers.DataHelper;
using static EnfoTests.Helpers.RepositoryHelper;
using static EnfoTests.Helpers.ResourceHelper;

namespace EnfoTests.Domain.ValidationTests;

[TestFixture]
public class ValidatingEnforcementOrderUpdate
{
    [Test]
    public async Task SucceedsGivenValidUpdates()
    {
        var model = GetValidEnforcementOrderUpdate(1);
        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var validator = new EnforcementOrderUpdateValidator(repository);

        var result = await validator.TestValidateAsync(model);

        Assert.Multiple(() =>
        {
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        });
    }

    [Test]
    public async Task SucceedsWhenRemovingExecutedOrderGivenProposedOrder()
    {
        var order = GetEnforcementOrderAdminView(GetEnforcementOrders.First(e => e.IsProposedOrder).Id);

        var model = new EnforcementOrderUpdate(order)
        {
            ExecutedDate = null, ExecutedOrderPostedDate = null, IsExecutedOrder = false
        };

        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var validator = new EnforcementOrderUpdateValidator(repository);

        var result = await validator.TestValidateAsync(model);

        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task FailsWhenRemovingExecutedOrderIfNotProposedOrder()
    {
        var order = GetEnforcementOrderAdminView(GetEnforcementOrders.First(e => !e.IsProposedOrder).Id);
        var model = new EnforcementOrderUpdate(order)
        {
            ExecutedDate = null, ExecutedOrderPostedDate = null, IsExecutedOrder = false,
        };

        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var validator = new EnforcementOrderUpdateValidator(repository);

        var result = await validator.TestValidateAsync(model);

        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(e => e.IsProposedOrder);
    }

    [Test]
    public async Task FailsWhenRemovingExecutedOrderDetails()
    {
        var model = GetValidEnforcementOrderUpdate(1);
        model.ExecutedDate = null;
        model.ExecutedOrderPostedDate = null;

        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var validator = new EnforcementOrderUpdateValidator(repository);

        var result = await validator.TestValidateAsync(model);

        Assert.Multiple(() =>
        {
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(e => e.ExecutedDate);
            result.ShouldHaveValidationErrorFor(e => e.ExecutedOrderPostedDate);
        });
    }

    [Test]
    public async Task SucceedsWhenRemovingHearing()
    {
        var model = GetValidEnforcementOrderUpdate(1);
        model.IsHearingScheduled = false;

        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var validator = new EnforcementOrderUpdateValidator(repository);

        var result = await validator.TestValidateAsync(model);

        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task FailsWhenRemovingHearingDetailsIfHearingStillTrue()
    {
        var model = GetValidEnforcementOrderUpdate(1);
        model.HearingCommentPeriodClosesDate = null;
        model.HearingContactId = null;
        model.HearingDate = null;
        model.HearingLocation = null;

        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var validator = new EnforcementOrderUpdateValidator(repository);

        var result = await validator.TestValidateAsync(model);

        Assert.Multiple(() =>
        {
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(e => e.HearingDate);
            result.ShouldHaveValidationErrorFor(e => e.HearingLocation);
            result.ShouldHaveValidationErrorFor(e => e.HearingCommentPeriodClosesDate);
            result.ShouldHaveValidationErrorFor(e => e.HearingContactId);
        });
    }
}
