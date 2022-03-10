using Enfo.Domain.Repositories;
using Enfo.Domain.Utils;

namespace Enfo.Domain.Resources.EnforcementOrder;

public class EnforcementOrderCreate
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
    public int? LegalAuthorityId { get; init; }

    [DisplayName("Cause of Order")]
    [Required(ErrorMessage = "Cause of Order is required.")]
    [DataType(DataType.MultilineText)]
    public string Cause { get; set; }

    [DisplayName("Requirements of Order")]
    [DataType(DataType.MultilineText)]
    public string Requirements { get; set; }

    [DisplayName("Settlement Amount")]
    [DataType(DataType.Currency)]
    public decimal? SettlementAmount { get; init; }

    public PublicationProgress Progress { get; set; } = PublicationProgress.Published;

    [DisplayName("Order Number")]
    [Required(ErrorMessage = "Order Number is required.")]
    [StringLength(50)]
    public string OrderNumber { get; set; }

    //  Determines the type of Enforcement Order created
    [DisplayName("Status")]
    public NewEnforcementOrderType CreateAs { get; init; } = NewEnforcementOrderType.Proposed;

    // Proposed orders

    [DisplayName("Date Comment Period Closes")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DisplayFormats.EditDate, ApplyFormatInEditMode = true)]
    public DateTime? CommentPeriodClosesDate { get; set; }

    [DisplayName("Send Comments To")]
    public int? CommentContactId { get; set; }

    [DisplayName("Publication Date For Proposed Order")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DisplayFormats.EditDate, ApplyFormatInEditMode = true)]
    public DateTime? ProposedOrderPostedDate { get; set; } = DateUtils.NextMonday();

    // Executed orders

    [DisplayName("Date Executed")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DisplayFormats.EditDate, ApplyFormatInEditMode = true)]
    public DateTime? ExecutedDate { get; init; }

    [DisplayName("Publication Date For Executed Order")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DisplayFormats.EditDate, ApplyFormatInEditMode = true)]
    public DateTime? ExecutedOrderPostedDate { get; init; } = DateUtils.NextMonday();

    // Hearing info

    [DisplayName("Public Hearing Scheduled")]
    public bool IsHearingScheduled { get; init; }

    [DisplayName("Hearing Date/Time")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = DisplayFormats.EditDateTime, ApplyFormatInEditMode = true)]
    public DateTime? HearingDate { get; init; } = DateTime.Today.AddHours(12);

    [DisplayName("Hearing Location")]
    [DataType(DataType.MultilineText)]
    public string HearingLocation { get; set; }

    [DisplayName("Date Hearing Comment Period Closes")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DisplayFormats.EditDate, ApplyFormatInEditMode = true)]
    public DateTime? HearingCommentPeriodClosesDate { get; init; }

    [DisplayName("Hearing Information Contact")]
    public int? HearingContactId { get; init; }

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
    Executed,
}
