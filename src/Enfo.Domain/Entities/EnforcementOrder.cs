using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Enfo.Domain.Repositories;
using Enfo.Domain.Validation;

namespace Enfo.Domain.Entities
{
    public class EnforcementOrder : BaseEntity
    {
        // enums

        public enum PublicationState
        {
            Draft,
            Published
        }

        // Common data elements

        [DisplayName("Facility")]
        [StringLength(205)]
        [Required(ErrorMessage = "Facility Name is required")]
        public string FacilityName { get; set; }

        [StringLength(25)]
        [Required(ErrorMessage = "County Name is required")]
        public string County { get; set; }

        [DisplayName("Legal Authority")]
        public virtual LegalAuthority LegalAuthority { get; set; }
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
        [Column(TypeName = "money")]
        [NonNegative]
        public decimal? SettlementAmount { get; set; }

        public bool Deleted { get; set; } = false;

        [DisplayName("Progress")]
        public PublicationState PublicationStatus { get; set; }

        [DisplayName("Order Number")]
        [Required(ErrorMessage = "Order Number is required")]
        [StringLength(50)]
        public string OrderNumber { get; set; }

        public DateTime? GetLastPostedDate()
        {
            return ExecutedDate ?? ProposedOrderPostedDate;
        }

        public bool GetIsPublic()
        {
            return GetIsPublicExecutedOrder() || GetIsPublicProposedOrder();
        }

        // Proposed orders

        [DisplayName("Proposed Order Public Noticed")]
        public bool IsProposedOrder { get; set; } = false;

        [DisplayName("Date Comment Period Closes")]
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? CommentPeriodClosesDate { get; set; }

        [DisplayName("Send Comments To")]
        public virtual EpdContact CommentContact { get; set; }
        public int? CommentContactId { get; set; }

        [DisplayName("Publication Date For Proposed Order")]
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? ProposedOrderPostedDate { get; set; }

        public bool GetIsPublicProposedOrder()
        {
            return (
                !Deleted
                && PublicationStatus == PublicationState.Published
                && IsProposedOrder
                && ProposedOrderPostedDate.HasValue
                && ProposedOrderPostedDate.Value <= DateTime.Today
                );
        }

        // Executed orders

        [DisplayName("Enforcement Order Executed")]
        public bool IsExecutedOrder { get; set; } = false;

        [DisplayName("Date Executed")]
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? ExecutedDate { get; set; }

        [DisplayName("Publication Date For Executed Order")]
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? ExecutedOrderPostedDate { get; set; }

        public bool GetIsPublicExecutedOrder()
        {
            return (
                !Deleted
                && PublicationStatus == PublicationState.Published
                && IsExecutedOrder
                && ExecutedOrderPostedDate.HasValue
                && ExecutedOrderPostedDate.Value <= DateTime.Today
                );
        }

        // Hearing info

        [DisplayName("Public Hearing Scheduled")]
        public bool IsHearingScheduled { get; set; } = false;

        [DisplayName("Hearing Date/Time")]
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime? HearingDate { get; set; }

        [DisplayName("Hearing Location")]
        [DataType(DataType.MultilineText)]
        [StringLength(3990)]
        public string HearingLocation { get; set; }

        [DisplayName("Date Hearing Comment Period Closes")]
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? HearingCommentPeriodClosesDate { get; set; }

        [DisplayName("Hearing Information Contact")]
        public virtual EpdContact HearingContact { get; set; }
        public int? HearingContactId { get; set; }

        // Create/Update methods

        public enum NewEnforcementOrderType
        {
            Proposed,
            Executed
        }

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
                        result.AddErrorMessage("CommentContact", "A contact is required for comments for proposed orders.");

                    if (!commentPeriodClosesDate.HasValue)
                        result.AddErrorMessage("CommentPeriodClosesDate", "A closing date is required for comments for proposed orders.");

                    if (!proposedOrderPostedDate.HasValue)
                        result.AddErrorMessage("ProposedOrderPostedDate", "A publication date is required for proposed orders.");
                }

                if (createAs == NewEnforcementOrderType.Executed)
                {
                    if (!executedDate.HasValue)
                        result.AddErrorMessage("ExecutedDate", "An execution date is required for executed orders.");

                    if (!executedOrderPostedDate.HasValue)
                        result.AddErrorMessage("ExecutedOrderPostedDate", "A publication date is required for executed orders.");
                }

                if (isHearingScheduled)
                {
                    if (!hearingDate.HasValue)
                        result.AddErrorMessage("HearingDate", "A hearing date is required if a hearing is scheduled.");

                    if (string.IsNullOrWhiteSpace(hearingLocation))
                        result.AddErrorMessage("HearingLocation", "A hearing location is required if a hearing is scheduled.");

                    if (!hearingCommentPeriodClosesDate.HasValue)
                        result.AddErrorMessage("HearingCommentPeriodClosesDate", "A closing date is required for hearing comments if a hearing is scheduled.");

                    if (hearingContactId is null)
                        result.AddErrorMessage("HearingContact", "A contact is required for hearings if a hearing is scheduled.");
                }
            }

            if (result.Success)
            {
                result.NewItem = new EnforcementOrder()
                {
                    Cause = cause,
                    CommentContactId = (createAs == NewEnforcementOrderType.Proposed) ? commentContactId : null,
                    CommentPeriodClosesDate = (createAs == NewEnforcementOrderType.Proposed) ? commentPeriodClosesDate : null,
                    County = county,
                    ExecutedDate = (createAs == NewEnforcementOrderType.Executed) ? executedDate : null,
                    ExecutedOrderPostedDate = (createAs == NewEnforcementOrderType.Executed) ? executedOrderPostedDate : null,
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
                    ProposedOrderPostedDate = (createAs == NewEnforcementOrderType.Proposed) ? proposedOrderPostedDate : null,
                    PublicationStatus = publicationStatus,
                    Requirements = requirements,
                    SettlementAmount = settlementAmount
                };
            }

            return result;
        }

        public UpdateEntityResult Update(
            string cause, int? commentContactId, DateTime? commentPeriodClosesDate, string county, string facilityName,
            DateTime? executedDate, DateTime? executedOrderPostedDate, DateTime? hearingCommentPeriodClosesDate,
            int? hearingContactId, DateTime? hearingDate, string hearingLocation, bool isExecutedOrder,
            bool isHearingScheduled, int legalAuthorityId, string orderNumber, DateTime? proposedOrderPostedDate,
            PublicationState publicationStatus, string requirements, decimal? settlementAmount)
        {
            var result = new UpdateEntityResult();

            if (Deleted)
            {
                result.AddErrorMessage("Id", "A deleted Enforcement Order cannot be modified");
                return result;
            }

            // Executed order info can only be removed if proposed order info exists.
            if (!isExecutedOrder && IsExecutedOrder && !IsProposedOrder)
            {
                result.AddErrorMessage("IsExecutedOrder", "Executed Order details are required for this Enforcement Order.");
            }

            if (publicationStatus == PublicationState.Published)
            {
                // Proposed order info cannot be removed from an existing order.
                if (IsProposedOrder)
                {
                    if (commentContactId is null)
                        result.AddErrorMessage("CommentContact", "A contact is required for comments for proposed orders.");

                    if (!commentPeriodClosesDate.HasValue)
                        result.AddErrorMessage("CommentPeriodClosesDate", "A closing date is required for comments for proposed orders.");

                    if (!proposedOrderPostedDate.HasValue)
                        result.AddErrorMessage("ProposedOrderPostedDate", "A publication date is required for proposed orders.");
                }

                if (isExecutedOrder)
                {
                    if (!executedDate.HasValue)
                        result.AddErrorMessage("ExecutedDate", "An execution date is required for executed orders.");

                    if (!executedOrderPostedDate.HasValue)
                        result.AddErrorMessage("ExecutedOrderPostedDate", "A publication date is required for executed orders.");
                }

                if (isHearingScheduled)
                {
                    if (!hearingDate.HasValue)
                        result.AddErrorMessage("HearingDate", "A hearing date is required if a hearing is scheduled.");

                    if (string.IsNullOrWhiteSpace(hearingLocation))
                        result.AddErrorMessage("HearingLocation", "A hearing location is required if a hearing is scheduled.");

                    if (!hearingCommentPeriodClosesDate.HasValue)
                        result.AddErrorMessage("HearingCommentPeriodClosesDate", "A closing date is required for hearing comments if a hearing is scheduled.");

                    if (hearingContactId is null)
                        result.AddErrorMessage("HearingContact", "A contact is required for hearings if a hearing is scheduled.");
                }
            }

            if (result.Success)
            {
                Cause = cause;
                CommentContactId = IsProposedOrder ? commentContactId : null;
                CommentPeriodClosesDate = IsProposedOrder ? commentPeriodClosesDate : null;
                County = county;
                ExecutedDate = isExecutedOrder ? executedDate : null;
                ExecutedOrderPostedDate = isExecutedOrder ? executedOrderPostedDate : null;
                FacilityName = facilityName;
                HearingCommentPeriodClosesDate = isHearingScheduled ? hearingCommentPeriodClosesDate : null;
                HearingContactId = isHearingScheduled ? hearingContactId : null;
                HearingDate = isHearingScheduled ? hearingDate : null;
                HearingLocation = isHearingScheduled ? hearingLocation : null;
                IsExecutedOrder = isExecutedOrder;
                IsHearingScheduled = isHearingScheduled;
                LegalAuthorityId = legalAuthorityId;
                OrderNumber = orderNumber;
                ProposedOrderPostedDate = IsProposedOrder ? proposedOrderPostedDate : null;
                PublicationStatus = publicationStatus;
                Requirements = requirements;
                SettlementAmount = settlementAmount;
            }

            return result;
        }
    }
}
