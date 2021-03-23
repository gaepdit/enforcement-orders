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

        [Required]
        [StringLength(205)]
        public string FacilityName { get; set; }

        [Required]
        [StringLength(25)]
        public string County { get; set; }

        [DisplayName("Legal Authority")]
        public LegalAuthority LegalAuthority { get; set; }

        [Required]
        public int LegalAuthorityId { get; set; }

        [Required]
        [StringLength(3990)]
        public string Cause { get; set; }

        [Required]
        [StringLength(3990)]
        public string Requirements { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal? SettlementAmount { get; set; }

        public bool Deleted { get; set; }

        public PublicationState PublicationStatus { get; set; }

        [Required]
        [StringLength(50)]
        public string OrderNumber { get; set; }

        // Calculated properties
        
        public DateTime? GetLastPostedDate => ExecutedDate ?? ProposedOrderPostedDate;
        public bool GetIsPublic => GetIsPublicExecutedOrder || GetIsPublicProposedOrder;

        // Proposed orders

        public bool IsProposedOrder { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime? CommentPeriodClosesDate { get; set; }

        public EpdContact CommentContact { get; set; }
        public int? CommentContactId { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime? ProposedOrderPostedDate { get; set; }

        public bool GetIsPublicProposedOrder =>
        (
            !Deleted
            && PublicationStatus == PublicationState.Published
            && IsProposedOrder
            && ProposedOrderPostedDate.HasValue
            && ProposedOrderPostedDate.Value <= DateTime.Today
        );

        // Executed orders

        public bool IsExecutedOrder { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime? ExecutedDate { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime? ExecutedOrderPostedDate { get; set; }

        public bool GetIsPublicExecutedOrder =>
        (
            !Deleted
            && PublicationStatus == PublicationState.Published
            && IsExecutedOrder
            && ExecutedOrderPostedDate.HasValue
            && ExecutedOrderPostedDate.Value <= DateTime.Today
        );

        // Hearing info

        public bool IsHearingScheduled { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime? HearingDate { get; set; }

        [StringLength(3990)]
        public string HearingLocation { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime? HearingCommentPeriodClosesDate { get; set; }

        public EpdContact HearingContact { get; set; }
        public int? HearingContactId { get; set; }
    }
}
