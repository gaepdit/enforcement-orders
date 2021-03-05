using System;
using Enfo.Domain.Entities;
using Enfo.Repository.Resources.EnforcementOrder;

namespace Enfo.Repository.Validation
{
    public static class EnforcementOrderValidation
    {
        public static ResourceValidationResult ValidateNewEnforcementOrder(EnforcementOrderCreate resource)
        {
            var result = new ResourceValidationResult();

            if (resource.PublicationStatus != PublicationState.Published) return result;

            switch (resource.CreateAs)
            {
                case EnforcementOrderCreate.NewEnforcementOrderType.Proposed:
                {
                    if (resource.CommentContactId is null)
                        result.AddErrorMessage("CommentContact",
                            "A contact is required for comments for proposed orders.");

                    if (!resource.CommentPeriodClosesDate.HasValue)
                        result.AddErrorMessage("CommentPeriodClosesDate",
                            "A closing date is required for comments for proposed orders.");

                    if (!resource.ProposedOrderPostedDate.HasValue)
                        result.AddErrorMessage("ProposedOrderPostedDate",
                            "A publication date is required for proposed orders.");

                    break;
                }
                case EnforcementOrderCreate.NewEnforcementOrderType.Executed:
                {
                    if (!resource.ExecutedDate.HasValue)
                        result.AddErrorMessage("ExecutedDate",
                            "An execution date is required for executed orders.");

                    if (!resource.ExecutedOrderPostedDate.HasValue)
                        result.AddErrorMessage("ExecutedOrderPostedDate",
                            "A publication date is required for executed orders.");

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!resource.IsHearingScheduled) return result;

            if (!resource.HearingDate.HasValue)
                result.AddErrorMessage("HearingDate",
                    "A hearing date is required if a hearing is scheduled.");

            if (string.IsNullOrWhiteSpace(resource.HearingLocation))
                result.AddErrorMessage("HearingLocation",
                    "A hearing location is required if a hearing is scheduled.");

            if (!resource.HearingCommentPeriodClosesDate.HasValue)
                result.AddErrorMessage("HearingCommentPeriodClosesDate",
                    "A closing date is required for hearing comments if a hearing is scheduled.");

            if (resource.HearingContactId is null)
                result.AddErrorMessage("HearingContact",
                    "A contact is required for hearings if a hearing is scheduled.");

            return result;
        }

        public static ResourceValidationResult ValidateEnforcementOrderUpdate(
            EnforcementOrder order,
            EnforcementOrderUpdate resource)
        {
            var result = new ResourceValidationResult();

            if (order.Deleted)
            {
                result.AddErrorMessage("Id", "A deleted Enforcement Order cannot be modified");
                return result;
            }

            // Executed order info can only be removed if proposed order info exists.
            if (!resource.IsExecutedOrder && order.IsExecutedOrder && !order.IsProposedOrder)
            {
                result.AddErrorMessage("IsExecutedOrder",
                    "Executed Order details are required for this Enforcement Order.");
            }

            if (resource.PublicationStatus != PublicationState.Published) return result;
            
            // Proposed order info cannot be removed from an existing order.
            if (order.IsProposedOrder)
            {
                if (resource.CommentContactId is null)
                    result.AddErrorMessage("CommentContact",
                        "A contact is required for comments for proposed orders.");

                if (!resource.CommentPeriodClosesDate.HasValue)
                    result.AddErrorMessage("CommentPeriodClosesDate",
                        "A closing date is required for comments for proposed orders.");

                if (!resource.ProposedOrderPostedDate.HasValue)
                    result.AddErrorMessage("ProposedOrderPostedDate",
                        "A publication date is required for proposed orders.");
            }

            if (resource.IsExecutedOrder)
            {
                if (!resource.ExecutedDate.HasValue)
                    result.AddErrorMessage("ExecutedDate", "An execution date is required for executed orders.");

                if (!resource.ExecutedOrderPostedDate.HasValue)
                    result.AddErrorMessage("ExecutedOrderPostedDate",
                        "A publication date is required for executed orders.");
            }

            if (!resource.IsHearingScheduled) return result;
                
            if (!resource.HearingDate.HasValue)
                result.AddErrorMessage("HearingDate", "A hearing date is required if a hearing is scheduled.");

            if (string.IsNullOrWhiteSpace(resource.HearingLocation))
                result.AddErrorMessage("HearingLocation",
                    "A hearing location is required if a hearing is scheduled.");

            if (!resource.HearingCommentPeriodClosesDate.HasValue)
                result.AddErrorMessage("HearingCommentPeriodClosesDate",
                    "A closing date is required for hearing comments if a hearing is scheduled.");

            if (resource.HearingContactId is null)
                result.AddErrorMessage("HearingContact",
                    "A contact is required for hearings if a hearing is scheduled.");

            return result;
        }
    }
}