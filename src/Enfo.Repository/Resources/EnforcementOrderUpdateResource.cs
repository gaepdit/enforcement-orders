using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Enfo.Domain.Entities;
using Enfo.Repository.Validation;
using Enfo.Repository.Repositories;
using static Enfo.Domain.Entities.EnforcementOrder;

namespace Enfo.Repository.Resources
{
    public class EnforcementOrderUpdateResource
    {
        // Common data elements

        [DisplayName("Facility")]
        [StringLength(205)]
        [Required(ErrorMessage = "Facility Name is required")]
        public string FacilityName { get; set; }

        [StringLength(25)]
        [Required(ErrorMessage = "County Name is required")]
        public string County { get; set; }

        [DisplayName("Legal Authority")]
        [Required(ErrorMessage = "Legal Authority is required")]
        public int LegalAuthorityId { get; set; }

        [DisplayName("Cause of Order")]
        [DataType(DataType.MultilineText)]
        [StringLength(3990)]
        public string Cause { get; set; }

        [DisplayName("Requirements of Order")]
        [DataType(DataType.MultilineText)]
        [StringLength(3990)]
        public string Requirements { get; set; }

        [DisplayName("Settlement Amount")]
        [DataType(DataType.Currency)]
        [NonNegative]
        public decimal? SettlementAmount { get; set; }

        [DisplayName("Progress")]
        public PublicationState PublicationStatus { get; set; }

        [DisplayName("Order Number")]
        [Required(ErrorMessage = "Order Number is required")]
        [StringLength(50)]
        public string OrderNumber { get; set; }

        // Proposed orders

        [DisplayName("Date Comment Period Closes")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? CommentPeriodClosesDate { get; set; }

        [DisplayName("Send Comments To")]
        public int? CommentContactId { get; set; }

        [DisplayName("Publication Date For Proposed Order")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? ProposedOrderPostedDate { get; set; }

        // Executed orders

        [DisplayName("Enforcement Order Executed")]
        public bool IsExecutedOrder { get; set; }

        [DisplayName("Date Executed")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? ExecutedDate { get; set; }

        [DisplayName("Publication Date For Executed Order")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? ExecutedOrderPostedDate { get; set; }

        // Hearing info

        [DisplayName("Public Hearing Scheduled")]
        public bool IsHearingScheduled { get; set; }

        [DisplayName("Hearing Date/Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime? HearingDate { get; set; }

        [DisplayName("Hearing Location")]
        [DataType(DataType.MultilineText)]
        [StringLength(3990)]
        public string HearingLocation { get; set; }

        [DisplayName("Date Hearing Comment Period Closes")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? HearingCommentPeriodClosesDate { get; set; }

        [DisplayName("Hearing Information Contact")]
        public int? HearingContactId { get; set; }

        public static UpdateEntityResult UpdateEnforcementOrder(EnforcementOrder order,
            string cause, int? commentContactId, DateTime? commentPeriodClosesDate, string county, string facilityName,
            DateTime? executedDate, DateTime? executedOrderPostedDate, DateTime? hearingCommentPeriodClosesDate,
            int? hearingContactId, DateTime? hearingDate, string hearingLocation, bool isExecutedOrder,
            bool isHearingScheduled, int legalAuthorityId, string orderNumber, DateTime? proposedOrderPostedDate,
            PublicationState publicationStatus, string requirements, decimal? settlementAmount)
        {
            var result = new UpdateEntityResult();

            if (order.Deleted)
            {
                result.AddErrorMessage("Id", "A deleted Enforcement Order cannot be modified");
                return result;
            }

            // Executed order info can only be removed if proposed order info exists.
            if (!isExecutedOrder && order.IsExecutedOrder && !order.IsProposedOrder)
            {
                result.AddErrorMessage("IsExecutedOrder",
                    "Executed Order details are required for this Enforcement Order.");
            }

            if (publicationStatus == PublicationState.Published)
            {
                // Proposed order info cannot be removed from an existing order.
                if (order.IsProposedOrder)
                {
                    if (commentContactId is null)
                        result.AddErrorMessage("CommentContact",
                            "A contact is required for comments for proposed orders.");

                    if (!commentPeriodClosesDate.HasValue)
                        result.AddErrorMessage("CommentPeriodClosesDate",
                            "A closing date is required for comments for proposed orders.");

                    if (!proposedOrderPostedDate.HasValue)
                        result.AddErrorMessage("ProposedOrderPostedDate",
                            "A publication date is required for proposed orders.");
                }

                if (isExecutedOrder)
                {
                    if (!executedDate.HasValue)
                        result.AddErrorMessage("ExecutedDate", "An execution date is required for executed orders.");

                    if (!executedOrderPostedDate.HasValue)
                        result.AddErrorMessage("ExecutedOrderPostedDate",
                            "A publication date is required for executed orders.");
                }

                if (isHearingScheduled)
                {
                    if (!hearingDate.HasValue)
                        result.AddErrorMessage("HearingDate", "A hearing date is required if a hearing is scheduled.");

                    if (string.IsNullOrWhiteSpace(hearingLocation))
                        result.AddErrorMessage("HearingLocation",
                            "A hearing location is required if a hearing is scheduled.");

                    if (!hearingCommentPeriodClosesDate.HasValue)
                        result.AddErrorMessage("HearingCommentPeriodClosesDate",
                            "A closing date is required for hearing comments if a hearing is scheduled.");

                    if (hearingContactId is null)
                        result.AddErrorMessage("HearingContact",
                            "A contact is required for hearings if a hearing is scheduled.");
                }
            }

            if (result.Success)
            {
                order.Cause = cause;
                order.CommentContactId = order.IsProposedOrder ? commentContactId : null;
                order.CommentPeriodClosesDate = order.IsProposedOrder ? commentPeriodClosesDate : null;
                order.County = county;
                order.ExecutedDate = isExecutedOrder ? executedDate : null;
                order.ExecutedOrderPostedDate = isExecutedOrder ? executedOrderPostedDate : null;
                order.FacilityName = facilityName;
                order.HearingCommentPeriodClosesDate = isHearingScheduled ? hearingCommentPeriodClosesDate : null;
                order.HearingContactId = isHearingScheduled ? hearingContactId : null;
                order.HearingDate = isHearingScheduled ? hearingDate : null;
                order.HearingLocation = isHearingScheduled ? hearingLocation : null;
                order.IsExecutedOrder = isExecutedOrder;
                order.IsHearingScheduled = isHearingScheduled;
                order.LegalAuthorityId = legalAuthorityId;
                order.OrderNumber = orderNumber;
                order.ProposedOrderPostedDate = order.IsProposedOrder ? proposedOrderPostedDate : null;
                order.PublicationStatus = publicationStatus;
                order.Requirements = requirements;
                order.SettlementAmount = settlementAmount;
            }

            return result;
        }
    }
}