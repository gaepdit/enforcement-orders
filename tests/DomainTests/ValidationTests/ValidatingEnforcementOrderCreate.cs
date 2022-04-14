using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Resources.Validation;
using FluentAssertions;
using FluentValidation.TestHelper;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static EnfoTests.Helpers.DataHelper;
using static EnfoTests.Helpers.RepositoryHelper;

namespace EnfoTests.Domain.ValidationTests;

public class ValidatingEnforcementOrderCreate
{
    // Sample data for create
    private readonly EnforcementOrderCreate _sampleCreate = new()
    {
        Cause = "Cause of order",
        Requirements = "Requirements of order",
        FacilityName = "abc",
        County = "Fulton",
        LegalAuthorityId = GetLegalAuthorities.First().Id,
        Progress = PublicationProgress.Draft,
        OrderNumber = "NEW-1",
        CreateAs = NewEnforcementOrderType.Proposed,
        CommentPeriodClosesDate = DateTime.Today.AddDays(1),
        CommentContactId = GetEpdContacts.First().Id,
        ProposedOrderPostedDate = DateTime.Today,
    };

    [Fact]
    public async Task SucceedsGivenValidUpdates()
    {
        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var validator = new EnforcementOrderCreateValidator(repository);

        var result = await validator.TestValidateAsync(_sampleCreate);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task FailsIfMissingRequiredProperties()
    {
        _sampleCreate.Progress = PublicationProgress.Published;
        _sampleCreate.CommentContactId = null;
        _sampleCreate.CommentPeriodClosesDate = null;

        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var validator = new EnforcementOrderCreateValidator(repository);

        var result = await validator.TestValidateAsync(_sampleCreate);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty().And.HaveCount(2);
        result.ShouldHaveValidationErrorFor(e => e.CommentContactId);
        result.ShouldHaveValidationErrorFor(e => e.CommentPeriodClosesDate);
    }

    [Fact]
    public async Task FailsWithDuplicateOrderNumber()
    {
        _sampleCreate.Progress = PublicationProgress.Published;
        _sampleCreate.OrderNumber = GetEnforcementOrders.First(e => !e.Deleted).OrderNumber;

        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var validator = new EnforcementOrderCreateValidator(repository);

        var result = await validator.TestValidateAsync(_sampleCreate);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.ShouldHaveValidationErrorFor(e => e.OrderNumber);
    }
}
