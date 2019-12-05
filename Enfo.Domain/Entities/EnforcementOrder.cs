using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public decimal? SettlementAmount { get; set; }

        public bool Deleted { get; set; } = false;

        [DisplayName("Progress")]
        public PublicationState PublicationStatus { get; set; }

        [DisplayName("Order Number")]
        [Required(ErrorMessage = "Order Number is required")]
        [StringLength(50)]
        public string OrderNumber { get; set; }

        public DateTime? LastPostedDate
        {
            get
            {
                return ExecutedDate ?? ProposedOrderPostedDate;
            }
        }

        public bool IsPublic
        {
            get
            {
                return IsPublicExecutedOrder || IsPublicProposedOrder;
            }
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

        public bool IsPublicProposedOrder
        {
            get
            {
                return (
                    !Deleted
                    && Active
                    && PublicationStatus == PublicationState.Published
                    && IsProposedOrder
                    && ProposedOrderPostedDate.HasValue
                    && ProposedOrderPostedDate.Value <= DateTime.Today
                    );
            }
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

        public bool IsPublicExecutedOrder
        {
            get
            {
                return (
                    !Deleted
                    && Active
                    && PublicationStatus == PublicationState.Published
                    && IsExecutedOrder
                    && ExecutedOrderPostedDate.HasValue
                    && ExecutedOrderPostedDate.Value <= DateTime.Today
                    );
            }
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

    }
}
