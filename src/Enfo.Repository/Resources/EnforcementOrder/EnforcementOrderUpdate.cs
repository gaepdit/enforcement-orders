using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Enfo.Repository.Resources.EnforcementOrder
{
    public class EnforcementOrderUpdate
    {
        // Common data elements

        [DisplayName("Facility")]
        [Required(ErrorMessage = "Facility Name is required.")]
        public string FacilityName { get; set; }

        [Required(ErrorMessage = "County Name is required.")]
        [StringLength(20)]
        public string County { get; set; }

        [DisplayName("Legal Authority")]
        [Required(ErrorMessage = "Legal Authority is required.")]
        public int LegalAuthorityId { get; set; }

        [DisplayName("Cause of Order")]
        [DataType(DataType.MultilineText)]
        public string Cause { get; set; }

        [DisplayName("Requirements of Order")]
        [DataType(DataType.MultilineText)]
        public string Requirements { get; set; }

        [DisplayName("Settlement Amount")]
        [DataType(DataType.Currency)]
        public decimal? SettlementAmount { get; set; }

        public PublicationState Progress { get; set; }

        [DisplayName("Order Number")]
        [Required(ErrorMessage = "Order Number is required.")]
        [StringLength(50)]
        public string OrderNumber { get; set; }

        // Proposed orders

        [DisplayName("Proposed Order Public Noticed")]
        public bool IsProposedOrder { get; set; }

        [DisplayName("Date Comment Period Closes")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = DisplayFormats.FormatDateEdit, ApplyFormatInEditMode = true)]
        public DateTime? CommentPeriodClosesDate { get; set; }

        [DisplayName("Send Comments To")]
        public int? CommentContactId { get; set; }

        public bool IsInactiveCommentContact { get; init; }

        [DisplayName("Publication Date For Proposed Order")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = DisplayFormats.FormatDateEdit, ApplyFormatInEditMode = true)]
        public DateTime? ProposedOrderPostedDate { get; set; }

        // Executed orders

        [DisplayName("Enforcement Order Executed")]
        public bool IsExecutedOrder { get; set; }

        [DisplayName("Date Executed")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = DisplayFormats.FormatDateEdit, ApplyFormatInEditMode = true)]
        public DateTime? ExecutedDate { get; set; }

        [DisplayName("Publication Date For Executed Order")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = DisplayFormats.FormatDateEdit, ApplyFormatInEditMode = true)]
        public DateTime? ExecutedOrderPostedDate { get; set; }

        // Hearing info

        [DisplayName("Public Hearing Scheduled")]
        public bool IsHearingScheduled { get; set; }

        [DisplayName("Hearing Date/Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = DisplayFormats.FormatDateTimeEdit, ApplyFormatInEditMode = true)]
        public DateTime? HearingDate { get; set; }

        [DisplayName("Hearing Location")]
        [DataType(DataType.MultilineText)]
        public string HearingLocation { get; set; }

        [DisplayName("Date Hearing Comment Period Closes")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = DisplayFormats.FormatDateEdit, ApplyFormatInEditMode = true)]
        public DateTime? HearingCommentPeriodClosesDate { get; set; }

        [DisplayName("Hearing Information Contact")]
        public int? HearingContactId { get; set; }

        public bool IsInactiveHearingContact { get; init; }

        public void TrimAll()
        {
            Cause = Cause?.Trim();
            County = County?.Trim();
            Requirements = Requirements?.Trim();
            FacilityName = FacilityName?.Trim();
            HearingLocation = HearingLocation?.Trim();
            OrderNumber = OrderNumber?.Trim();
        }
    }
}