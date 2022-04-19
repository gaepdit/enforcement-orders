using Enfo.Domain.EnforcementOrders.Repositories;
using FluentValidation;

namespace Enfo.Domain.EnforcementOrders.Resources.Validation;

public class EnforcementOrderUpdateValidator : BaseEnforcementOrderValidator<EnforcementOrderUpdate>
{
    public EnforcementOrderUpdateValidator(IEnforcementOrderRepository repository) : base(repository)
    {
        RuleFor(e => e.OrderNumber)
            .MustAsync(async (e, orderNumber, _) => await NotDuplicateOrderNumber(orderNumber, e.Id))
            .WithMessage(e => $"An Order with the same number ({e.OrderNumber.Trim()}) already exists.");

        RuleFor(e => e.IsProposedOrder)
            .Equal(true).When(e => !e.IsExecutedOrder)
            .WithMessage("Executed Order details cannot be removed from this Enforcement Order.");

        RuleFor(e => e.SettlementAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Settlement Amount cannot be less than zero.");

        // The following rules don't necessarily apply to draft orders
        When(e => e.Progress == PublicationProgress.Published, () =>
        {
            // The following rules apply to Proposed Orders
            When(e => e.IsProposedOrder, () =>
            {
                RuleFor(e => e.CommentContactId)
                    .NotNull().WithMessage("A contact for comments is required for proposed orders.");

                RuleFor(e => e.CommentPeriodClosesDate)
                    .NotNull().WithMessage("A closing date for comments is required for proposed orders.");

                RuleFor(e => e.ProposedOrderPostedDate)
                    .NotNull().WithMessage("A publication date is required for proposed orders.");
            });

            // The following rules only apply to Executed Orders
            When(e => e.IsExecutedOrder, () =>
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
}
