﻿using Enfo.Domain.Entities;
using Enfo.Domain.Resources.EnforcementOrder;

namespace Enfo.Domain.Validation;

public static class EnforcementOrderValidation
{

    public static ResourceValidationResult ValidateEnforcementOrderUpdate(
        [NotNull] EnforcementOrderAdminView order,
        [NotNull] EnforcementOrderUpdate resource)
    {
        var result = new ResourceValidationResult();

        if (order.Deleted)
        {
            result.AddErrorMessage(nameof(EnforcementOrder.Id), "A deleted Enforcement Order cannot be modified.");
            return result;
        }

        // Executed order info can only be removed if proposed order info exists.
        if (!resource.IsExecutedOrder && order.IsExecutedOrder && !order.IsProposedOrder)
        {
            result.AddErrorMessage(nameof(EnforcementOrderUpdate.IsExecutedOrder),
                "Executed Order details are required for this Enforcement Order.");
        }

        if (resource.Progress != PublicationProgress.Published) return result;

        if (resource.SettlementAmount is < 0)
            result.AddErrorMessage(nameof(EnforcementOrderCreate.SettlementAmount),
                "Settlement Amount cannot be less than zero.");

        // Proposed order info cannot be removed from an existing order.
        if (order.IsProposedOrder)
        {
            if (resource.CommentContactId is null)
                result.AddErrorMessage(nameof(EnforcementOrderUpdate.CommentContactId),
                    "A contact for comments is required for proposed orders.");

            if (!resource.CommentPeriodClosesDate.HasValue)
                result.AddErrorMessage(nameof(EnforcementOrderUpdate.CommentPeriodClosesDate),
                    "A closing date for comments is required for proposed orders.");

            if (!resource.ProposedOrderPostedDate.HasValue)
                result.AddErrorMessage(nameof(EnforcementOrderUpdate.ProposedOrderPostedDate),
                    "A publication date is required for proposed orders.");
        }

        if (resource.IsExecutedOrder)
        {
            if (!resource.ExecutedDate.HasValue)
                result.AddErrorMessage(nameof(EnforcementOrderUpdate.ExecutedDate),
                    "An execution date is required for executed orders.");

            if (!resource.ExecutedOrderPostedDate.HasValue)
                result.AddErrorMessage(nameof(EnforcementOrderUpdate.ExecutedOrderPostedDate),
                    "A publication date is required for executed orders.");
        }

        if (!resource.IsHearingScheduled) return result;

        if (!resource.HearingDate.HasValue)
            result.AddErrorMessage(nameof(EnforcementOrderUpdate.HearingDate),
                "A hearing date is required if a hearing is scheduled.");

        if (string.IsNullOrEmpty(resource.HearingLocation))
            result.AddErrorMessage(nameof(EnforcementOrderUpdate.HearingLocation),
                "A hearing location is required if a hearing is scheduled.");

        if (!resource.HearingCommentPeriodClosesDate.HasValue)
            result.AddErrorMessage(nameof(EnforcementOrderUpdate.HearingCommentPeriodClosesDate),
                "A closing date for hearing comments is required if a hearing is scheduled.");

        if (resource.HearingContactId is null)
            result.AddErrorMessage(nameof(EnforcementOrderUpdate.HearingContactId),
                "A contact for hearings is required if a hearing is scheduled.");

        return result;
    }
}
