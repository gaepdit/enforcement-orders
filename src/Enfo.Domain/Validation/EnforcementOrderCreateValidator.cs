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

        RuleFor(e => e.CreateAs).IsInEnum();

        RuleFor(e => e.OrderNumber)
            .MustAsync(async (orderNumber, _) => await NotBeADuplicate(orderNumber))
            .WithMessage(e => $"An Order with the same number ({e.OrderNumber.Trim()}) already exists.");

        RuleFor(e => e.SettlementAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Settlement Amount cannot be less than zero.");

        // The following rules don't necessarily apply to draft orders
        When(e => e.Progress == PublicationProgress.Published, () =>
        {
            // The following rules only apply to Proposed Orders
            When(e => e.CreateAs == NewEnforcementOrderType.Proposed, () =>
            {
                RuleFor(e => e.CommentContactId)
                    .NotNull().WithMessage("A contact for comments is required for proposed orders.");

                RuleFor(e => e.CommentPeriodClosesDate)
                    .NotNull().WithMessage("A closing date for comments is required for proposed orders.");

                RuleFor(e => e.ProposedOrderPostedDate)
                    .NotNull().WithMessage("A publication date is required for proposed orders.");
            });

            // The following rules only apply to Executed Orders
            When(e => e.CreateAs == NewEnforcementOrderType.Executed, () =>
            {
                RuleFor(e => e.ExecutedDate)
                    .NotNull().WithMessage("An execution date is required for executed orders.");

                RuleFor(e => e.ExecutedOrderPostedDate)
                    .NotNull().WithMessage("A publication date is required for executed orders.");
            });
        });

        // The following rules only apply if a hearing is scheduled
        When(e => e.IsHearingScheduled, () =>
        {
            RuleFor(e => e.HearingDate)
                .NotNull().WithMessage("A hearing date is required if a hearing is scheduled.");

            RuleFor(e => e.HearingLocation)
                .NotEmpty().WithMessage("A hearing location is required if a hearing is scheduled.");

            RuleFor(e => e.HearingCommentPeriodClosesDate)
                .NotNull().WithMessage("A closing date for hearing comments is required if a hearing is scheduled.");

            RuleFor(e => e.HearingContactId)
                .NotNull().WithMessage("A contact for hearings is required if a hearing is scheduled.");
        });
    }

    private async Task<bool> NotBeADuplicate(string orderNumber) =>
        !await _repository.OrderNumberExistsAsync(orderNumber?.Trim()).ConfigureAwait(false);
}
