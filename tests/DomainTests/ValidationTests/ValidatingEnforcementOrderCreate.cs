using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Resources.Validation;
using FluentValidation.TestHelper;

namespace DomainTests.ValidationTests;

[TestFixture]
public class ValidatingEnforcementOrderCreate
{
    [Test]
    public async Task SucceedsGivenValidUpdates()
    {
        var sampleCreate = new EnforcementOrderCreate
        {
            Cause = "Cause of order",
            Requirements = "Requirements of order",
            FacilityName = "Facility 3",
            County = "Fulton",
            LegalAuthorityId = 0,
            Progress = PublicationProgress.Draft,
            OrderNumber = "NEW-3",
            CreateAs = NewEnforcementOrderType.Proposed,
            CommentPeriodClosesDate = DateTime.Today.AddDays(1),
            CommentContactId = 0,
            ProposedOrderPostedDate = DateTime.Today,
        };

        var validator = new EnforcementOrderCreateValidator(Substitute.For<IEnforcementOrderRepository>());

        var result = await validator.TestValidateAsync(sampleCreate);

        using (new AssertionScope())
        {
            result.Errors.Should().BeEmpty();
            result.IsValid.Should().BeTrue();
        }
    }

    [Test]
    public async Task FailsIfMissingRequiredProperties()
    {
        var sampleCreate = new EnforcementOrderCreate
        {
            Cause = "Cause of order",
            Requirements = "Requirements of order",
            FacilityName = "Facility 3",
            County = "Fulton",
            LegalAuthorityId = 0,
            Progress = PublicationProgress.Published,
            OrderNumber = "NEW-3",
            CreateAs = NewEnforcementOrderType.Proposed,
            CommentPeriodClosesDate = null,
            CommentContactId = null,
            ProposedOrderPostedDate = DateTime.Today,
        };

        var validator = new EnforcementOrderCreateValidator(Substitute.For<IEnforcementOrderRepository>());

        var result = await validator.TestValidateAsync(sampleCreate);

        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty().And.HaveCount(2);
            result.ShouldHaveValidationErrorFor(e => e.CommentContactId);
            result.ShouldHaveValidationErrorFor(e => e.CommentPeriodClosesDate);
        }
    }

    [Test]
    public async Task FailsWithDuplicateOrderNumber()
    {
        var sampleCreate = new EnforcementOrderCreate
        {
            Cause = "Cause of order",
            Requirements = "Requirements of order",
            FacilityName = "Facility 3",
            County = "Fulton",
            LegalAuthorityId = 0,
            Progress = PublicationProgress.Published,
            OrderNumber = "abc",
            CreateAs = NewEnforcementOrderType.Proposed,
            CommentPeriodClosesDate = DateTime.Today.AddDays(1),
            CommentContactId = 0,
            ProposedOrderPostedDate = DateTime.Today,
        };

        var repoMock = Substitute.For<IEnforcementOrderRepository>();
        repoMock.OrderNumberExistsAsync(Arg.Any<string>(), Arg.Any<int?>()).Returns(true);
        var validator = new EnforcementOrderCreateValidator(repoMock);

        var result = await validator.TestValidateAsync(sampleCreate);

        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.ShouldHaveValidationErrorFor(e => e.OrderNumber);
        }
    }
}
