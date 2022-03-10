using Enfo.Domain.Repositories;
using Enfo.Domain.Resources.EnforcementOrder;
using FluentValidation;

namespace Enfo.Domain.Validation;

public class EnforcementOrderCreateValidator : AbstractValidator<EnforcementOrderCreate>
{
    private readonly IEnforcementOrderRepository _repository;

    public EnforcementOrderCreateValidator(IEnforcementOrderRepository repository)
    {
        _repository = repository;

        RuleFor(e => e.OrderNumber)
            .MustAsync(async (orderNumber, _) => await NotBeADuplicate(orderNumber))
            .WithMessage(e => $"An Order with the same number ({e.OrderNumber.Trim()}) already exists.");

        RuleFor(e => e.SettlementAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Settlement Amount cannot be less than zero.");

        // The following rules don't necessarily apply to draft orders
        
        RuleFor(e => e.CommentContactId)
            .Must(RequiredForProposedOrder)
            .WithMessage("A contact for comments is required for proposed orders.");
        
        RuleFor(e => e.CommentPeriodClosesDate)
            .Must(RequiredForProposedOrder)
            .WithMessage("A closing date for comments is required for proposed orders.");

        RuleFor(e => e.ProposedOrderPostedDate)
            .Must(RequiredForProposedOrder)
            .WithMessage("A publication date is required for proposed orders.");

        RuleFor(e => e.ExecutedDate)
            .Must(RequiredForExecutedOrder)
            .WithMessage("An execution date is required for executed orders.");

        RuleFor(e => e.ExecutedOrderPostedDate)
            .Must(RequiredForExecutedOrder)
            .WithMessage("A publication date is required for executed orders.");
        
        // The following rules only apply if a hearing is scheduled

        RuleFor(e => e.HearingDate)
            .Must(RequiredForAHearing)
            .WithMessage("A hearing date is required if a hearing is scheduled.");

        RuleFor(e => e.HearingLocation)
            .Must(RequiredForAHearing)
            .WithMessage("A hearing location is required if a hearing is scheduled.");

        RuleFor(e => e.HearingCommentPeriodClosesDate)
            .Must(RequiredForAHearing)
            .WithMessage("A closing date for hearing comments is required if a hearing is scheduled.");

        RuleFor(e => e.HearingContactId)
            .Must(RequiredForAHearing)
            .WithMessage("A contact for hearings is required if a hearing is scheduled.");
    }
    
    private async Task<bool> NotBeADuplicate(string orderNumber) =>
        !await _repository.OrderNumberExistsAsync(orderNumber?.Trim()).ConfigureAwait(false);

    private static bool RequiredForProposedOrder(EnforcementOrderCreate e, int? value) =>
        e.Progress != PublicationProgress.Published || 
        e.CreateAs != NewEnforcementOrderType.Proposed || 
        value is not null;

    private static bool RequiredForProposedOrder(EnforcementOrderCreate e, DateTime? value) =>
        e.Progress != PublicationProgress.Published || 
        e.CreateAs != NewEnforcementOrderType.Proposed || 
        value is not null;

    private static bool RequiredForExecutedOrder(EnforcementOrderCreate e, DateTime? value) =>
        e.Progress != PublicationProgress.Published || 
        e.CreateAs != NewEnforcementOrderType.Executed || 
        value is not null;

    private static bool RequiredForAHearing(EnforcementOrderCreate e, DateTime? value) =>
        !e.IsHearingScheduled || 
        value is not null;

    private static bool RequiredForAHearing(EnforcementOrderCreate e, int? value) =>
        !e.IsHearingScheduled || 
        value is not null;

    private static bool RequiredForAHearing(EnforcementOrderCreate e, string value) =>
        !e.IsHearingScheduled || 
        !string.IsNullOrWhiteSpace(value);
}
