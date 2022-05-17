using Enfo.Domain.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Enfo.Domain.EnforcementOrders.Resources;

public class EnforcementOrderUpdate
{
    public EnforcementOrderUpdate() { }

    public EnforcementOrderUpdate(EnforcementOrderAdminView item)
    {
        Id = item.Id;
        Cause = item.Cause;
        County = item.County;
        Requirements = item.Requirements;
        ExecutedDate = item.ExecutedDate;
        FacilityName = item.FacilityName;
        HearingDate = item.HearingDate;
        HearingLocation = item.HearingLocation;
        OrderNumber = item.OrderNumber;
        Progress = item.PublicationStatus;
        SettlementAmount = item.SettlementAmount;
        CommentContactId = item.CommentContact?.Id;
        IsInactiveCommentContact = !item.CommentContact?.Active ?? false;
        HearingContactId = item.HearingContact?.Id;
        IsInactiveHearingContact = !item.HearingContact?.Active ?? false;
        IsProposedOrder = item.IsProposedOrder;
        IsExecutedOrder = item.IsExecutedOrder;
        IsHearingScheduled = item.IsHearingScheduled;
        LegalAuthorityId = item.LegalAuthority.Id;
        CommentPeriodClosesDate = item.CommentPeriodClosesDate;
        ExecutedOrderPostedDate = item.ExecutedOrderPostedDate;
        ProposedOrderPostedDate = item.ProposedOrderPostedDate;
        HearingCommentPeriodClosesDate = item.HearingCommentPeriodClosesDate;
    }

    // Common data elements

    [HiddenInput]
    public int Id { get; init; }

    [DisplayName("Facility")]
    [Required(ErrorMessage = "Facility Name is required.")]
    public string FacilityName { get; set; }

    [Required(ErrorMessage = "County Name is required.")]
    [StringLength(20)]
    public string County { get; set; }

    [DisplayName("Legal Authority")]
    [Required(ErrorMessage = "Legal Authority is required.")]
    public int LegalAuthorityId { get; init; }

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

    public PublicationProgress Progress { get; init; }

    [DisplayName("Order Number")]
    [Required(ErrorMessage = "Order Number is required.")]
    [StringLength(50)]
    public string OrderNumber { get; set; }

    // Proposed orders

    [DisplayName("Proposed Order Public Noticed")]
    public bool IsProposedOrder { get; init; }

    [DisplayName("Date Comment Period Closes")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DisplayFormats.EditDate, ApplyFormatInEditMode = true)]
    public DateTime? CommentPeriodClosesDate { get; init; }

    [DisplayName("Send Comments To")]
    public int? CommentContactId { get; init; }

    public bool IsInactiveCommentContact { get; }

    [DisplayName("Publication Date For Proposed Order")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DisplayFormats.EditDate, ApplyFormatInEditMode = true)]
    public DateTime? ProposedOrderPostedDate { get; init; }

    // Executed orders

    [DisplayName("Enforcement Order Executed")]
    public bool IsExecutedOrder { get; init; }

    [DisplayName("Date Executed")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DisplayFormats.EditDate, ApplyFormatInEditMode = true)]
    public DateTime? ExecutedDate { get; init; }

    [DisplayName("Publication Date For Executed Order")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DisplayFormats.EditDate, ApplyFormatInEditMode = true)]
    public DateTime? ExecutedOrderPostedDate { get; init; }

    // Hearing info

    [DisplayName("Public Hearing Scheduled")]
    public bool IsHearingScheduled { get; init; }

    [DisplayName("Hearing Date/Time")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = DisplayFormats.EditDateTime, ApplyFormatInEditMode = true)]
    public DateTime? HearingDate { get; init; }

    [DisplayName("Hearing Location")]
    [DataType(DataType.MultilineText)]
    public string HearingLocation { get; set; }

    [DisplayName("Date Hearing Comment Period Closes")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DisplayFormats.EditDate, ApplyFormatInEditMode = true)]
    public DateTime? HearingCommentPeriodClosesDate { get; init; }

    [DisplayName("Hearing Information Contact")]
    public int? HearingContactId { get; init; }

    public bool IsInactiveHearingContact { get; }

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
