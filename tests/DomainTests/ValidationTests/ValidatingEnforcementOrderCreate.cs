using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Resources.Validation;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using static EnfoTests.Helpers.DataHelper;
using static EnfoTests.Helpers.RepositoryHelper;

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
            LegalAuthorityId = GetLegalAuthorities.First().Id,
            Progress = PublicationProgress.Draft,
            OrderNumber = "NEW-3",
            CreateAs = NewEnforcementOrderType.Proposed,
            CommentPeriodClosesDate = DateTime.Today.AddDays(1),
            CommentContactId = GetEpdContacts.First().Id,
            ProposedOrderPostedDate = DateTime.Today,
        };

        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var validator = new EnforcementOrderCreateValidator(repository);

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
            LegalAuthorityId = GetLegalAuthorities.First().Id,
            Progress = PublicationProgress.Published,
            OrderNumber = "NEW-3",
            CreateAs = NewEnforcementOrderType.Proposed,
            CommentPeriodClosesDate  = null,
            CommentContactId = null,
            ProposedOrderPostedDate = DateTime.Today,
        };

        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var validator = new EnforcementOrderCreateValidator(repository);

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
            LegalAuthorityId = GetLegalAuthorities.First().Id,
            Progress = PublicationProgress.Published,
            OrderNumber =  GetEnforcementOrders.First(e => !e.Deleted).OrderNumber,
            CreateAs = NewEnforcementOrderType.Proposed,
            CommentPeriodClosesDate = DateTime.Today.AddDays(1),
            CommentContactId = GetEpdContacts.First().Id,
            ProposedOrderPostedDate = DateTime.Today,
        };

        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var validator = new EnforcementOrderCreateValidator(repository);

        var result = await validator.TestValidateAsync(sampleCreate);

        Assert.Multiple(() =>
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.ShouldHaveValidationErrorFor(e => e.OrderNumber);
        });
    }
}
