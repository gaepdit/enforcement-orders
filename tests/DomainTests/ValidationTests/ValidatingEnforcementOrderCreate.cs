using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Resources.Validation;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace EnfoTests.Domain.ValidationTests;

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

        var validator = new EnforcementOrderCreateValidator(new Mock<IEnforcementOrderRepository>().Object);

        var result = await validator.TestValidateAsync(sampleCreate);

        Assert.Multiple(() =>
        {
            result.Errors.Should().BeEmpty();
            result.IsValid.Should().BeTrue();
        });
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

        var validator = new EnforcementOrderCreateValidator(new Mock<IEnforcementOrderRepository>().Object);

        var result = await validator.TestValidateAsync(sampleCreate);

        Assert.Multiple(() =>
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty().And.HaveCount(2);
            result.ShouldHaveValidationErrorFor(e => e.CommentContactId);
            result.ShouldHaveValidationErrorFor(e => e.CommentPeriodClosesDate);
        });
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

        var repoMock = new Mock<IEnforcementOrderRepository>();
        repoMock.Setup(l => l.OrderNumberExistsAsync(It.IsAny<string>(), It.IsAny<int?>()))
            .ReturnsAsync(true);
        var validator = new EnforcementOrderCreateValidator(repoMock.Object);

        var result = await validator.TestValidateAsync(sampleCreate);

        Assert.Multiple(() =>
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.ShouldHaveValidationErrorFor(e => e.OrderNumber);
        });
    }
}
