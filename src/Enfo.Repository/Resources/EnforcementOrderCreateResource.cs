using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Enfo.Domain.Entities;
using Enfo.Repository.Validation;
using Enfo.Repository.Repositories;
using static Enfo.Domain.Entities.EnforcementOrder;

namespace Enfo.Repository.Resources
{
    public class EnforcementOrderCreateResource
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

        [DisplayName("Status")]
        public NewEnforcementOrderType CreateAs { get; set; }

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

        public EnforcementOrder NewEnforcementOrder() => new EnforcementOrder
        {
            Cause = Cause,
            CommentContactId = (CreateAs == NewEnforcementOrderType.Proposed) ? CommentContactId : null,
            CommentPeriodClosesDate = (CreateAs == NewEnforcementOrderType.Proposed) ? CommentPeriodClosesDate : null,
            County = County,
            ExecutedDate = (CreateAs == NewEnforcementOrderType.Executed) ? ExecutedDate : null,
            ExecutedOrderPostedDate = (CreateAs == NewEnforcementOrderType.Executed) ? ExecutedOrderPostedDate : null,
            FacilityName = FacilityName,
            HearingCommentPeriodClosesDate = IsHearingScheduled ? HearingCommentPeriodClosesDate : null,
            HearingContactId = IsHearingScheduled ? HearingContactId : null,
            HearingDate = IsHearingScheduled ? HearingDate : null,
            HearingLocation = IsHearingScheduled ? HearingLocation : null,
            IsExecutedOrder = CreateAs == NewEnforcementOrderType.Executed,
            IsHearingScheduled = IsHearingScheduled,
            IsProposedOrder = CreateAs == NewEnforcementOrderType.Proposed,
            LegalAuthorityId = LegalAuthorityId,
            OrderNumber = OrderNumber,
            ProposedOrderPostedDate = (CreateAs == NewEnforcementOrderType.Proposed) ? ProposedOrderPostedDate : null,
            PublicationStatus = PublicationStatus,
            Requirements = Requirements,
            SettlementAmount = SettlementAmount
        };

        public static CreateEntityResult<EnforcementOrder> CreateNewEnforcementOrderEntity(
            NewEnforcementOrderType createAs,
            string cause,
            int? commentContactId,
            DateTime? commentPeriodClosesDate,
            string county,
            string facilityName,
            DateTime? executedDate,
            DateTime? executedOrderPostedDate,
            DateTime? hearingCommentPeriodClosesDate,
            int? hearingContactId,
            DateTime? hearingDate,
            string hearingLocation,
            bool isHearingScheduled,
            int legalAuthorityId,
            string orderNumber,
            DateTime? proposedOrderPostedDate,
            PublicationState publicationStatus,
            string requirements,
            decimal? settlementAmount)
        {
            var result = new CreateEntityResult<EnforcementOrder>();

            if (publicationStatus == PublicationState.Published)
            {
                if (createAs == NewEnforcementOrderType.Proposed)
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

                if (createAs == NewEnforcementOrderType.Executed)
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
                result.AddItem(new EnforcementOrder()
                {
                    Cause = cause,
                    CommentContactId = (createAs == NewEnforcementOrderType.Proposed) ? commentContactId : null,
                    CommentPeriodClosesDate =
                        (createAs == NewEnforcementOrderType.Proposed) ? commentPeriodClosesDate : null,
                    County = county,
                    ExecutedDate = (createAs == NewEnforcementOrderType.Executed) ? executedDate : null,
                    ExecutedOrderPostedDate =
                        (createAs == NewEnforcementOrderType.Executed) ? executedOrderPostedDate : null,
                    FacilityName = facilityName,
                    HearingCommentPeriodClosesDate = isHearingScheduled ? hearingCommentPeriodClosesDate : null,
                    HearingContactId = isHearingScheduled ? hearingContactId : null,
                    HearingDate = isHearingScheduled ? hearingDate : null,
                    HearingLocation = isHearingScheduled ? hearingLocation : null,
                    IsExecutedOrder = createAs == NewEnforcementOrderType.Executed,
                    IsHearingScheduled = isHearingScheduled,
                    IsProposedOrder = createAs == NewEnforcementOrderType.Proposed,
                    LegalAuthorityId = legalAuthorityId,
                    OrderNumber = orderNumber,
                    ProposedOrderPostedDate =
                        (createAs == NewEnforcementOrderType.Proposed) ? proposedOrderPostedDate : null,
                    PublicationStatus = publicationStatus,
                    Requirements = requirements,
                    SettlementAmount = settlementAmount
                });
            }

            return result;
        }
    }
}