using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Enfo.Repository.Utils;

namespace Enfo.Repository.Resources.EnforcementOrder
{
    public class EnforcementOrderCreate
    {
        //  Determines the type of Enforcement Order created

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
        public int? LegalAuthorityId { get; set; }

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
        public decimal? SettlementAmount { get; set; }

        public PublicationState Progress { get; set; } = PublicationState.Published;

        [DisplayName("Order Number")]
        [Required(ErrorMessage = "Order Number is required.")]
        [StringLength(50)]
        public string OrderNumber { get; set; }

        [DisplayName("Status")]
        public NewEnforcementOrderType CreateAs { get; set; } = NewEnforcementOrderType.Proposed;

        // Proposed orders

        [DisplayName("Date Comment Period Closes")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = DisplayFormats.FormatDateEdit, ApplyFormatInEditMode = true)]
        public DateTime? CommentPeriodClosesDate { get; set; }

        [DisplayName("Send Comments To")]
        public int? CommentContactId { get; set; }

        [DisplayName("Publication Date For Proposed Order")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = DisplayFormats.FormatDateEdit, ApplyFormatInEditMode = true)]
        public DateTime? ProposedOrderPostedDate { get; set; } = DateUtils.NextMonday();

        // Executed orders

        [DisplayName("Date Executed")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = DisplayFormats.FormatDateEdit, ApplyFormatInEditMode = true)]
        public DateTime? ExecutedDate { get; set; }

        [DisplayName("Publication Date For Executed Order")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = DisplayFormats.FormatDateEdit, ApplyFormatInEditMode = true)]
        public DateTime? ExecutedOrderPostedDate { get; set; } = DateUtils.NextMonday();

        // Hearing info

        [DisplayName("Public Hearing Scheduled")]
        public bool IsHearingScheduled { get; set; }

        [DisplayName("Hearing Date/Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = DisplayFormats.FormatDateTimeEdit, ApplyFormatInEditMode = true)]
        public DateTime? HearingDate { get; set; } = DateTime.Today.AddHours(12);

        [DisplayName("Hearing Location")]
        [DataType(DataType.MultilineText)]
        [StringLength(3990)]
        public string HearingLocation { get; set; }

        [DisplayName("Date Hearing Comment Period Closes")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = DisplayFormats.FormatDateEdit, ApplyFormatInEditMode = true)]
        public DateTime? HearingCommentPeriodClosesDate { get; set; }

        [DisplayName("Hearing Information Contact")]
        public int? HearingContactId { get; set; }

        public void TrimAll()
        {
            County = County?.Trim();
            FacilityName = FacilityName?.Trim();
            Cause = Cause?.Trim();
            Requirements = Requirements?.Trim();
            OrderNumber = OrderNumber?.Trim();
            HearingLocation = HearingLocation?.Trim();
        }
    }

    public enum NewEnforcementOrderType
    {
        Proposed,
        Executed
    }
}