using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Enfo.Repository.Validation;

namespace Enfo.Repository.Resources.EnforcementOrder
{
    public class EnforcementOrderCreate
    {
        //  Determines the type of Enforcement Order created
        public enum NewEnforcementOrderType
        {
            Proposed,
            Executed
        }

        // Common data elements

        [DisplayName("Facility")]
        [Required(ErrorMessage = "Facility Name is required.")]
        [StringLength(205)]
        public string FacilityName { get; set; }

        [Required(ErrorMessage = "County Name is required.")]
        [StringLength(25)]
        public string County { get; set; }

        [DisplayName("Legal Authority")]
        [Required(ErrorMessage = "Legal Authority is required.")]
        public int LegalAuthorityId { get; set; }

        [DisplayName("Cause of Order")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Cause of Order is required.")]
        [StringLength(3990)]
        public string Cause { get; set; }

        [DisplayName("Requirements of Order")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Requirements of Order is required.")]
        [StringLength(3990)]
        public string Requirements { get; set; }

        [DisplayName("Settlement Amount")]
        [DataType(DataType.Currency)]
        [NonNegative]
        public decimal? SettlementAmount { get; set; }

        [DisplayName("Progress")]
        public PublicationState PublicationStatus { get; set; }

        [DisplayName("Order Number")]
        [Required(ErrorMessage = "Order Number is required.")]
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
    }
}