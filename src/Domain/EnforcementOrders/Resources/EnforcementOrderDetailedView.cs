using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.Domain.EpdContacts.Resources;
using Enfo.Domain.Utils;

namespace Enfo.Domain.EnforcementOrders.Resources;

public class EnforcementOrderDetailedView : EnforcementOrderSummaryView
{
    public EnforcementOrderDetailedView([NotNull] EnforcementOrder item) : base(item)
    {
        Guard.NotNull(item, nameof(item));

        Cause = item.Cause;
        Requirements = item.Requirements;
        SettlementAmount = item.SettlementAmount;
        CommentContact = item.CommentContactId.HasValue &&
            item.GetIsPublicProposedOrder &&
            item.CommentPeriodClosesDate.HasValue &&
            item.CommentPeriodClosesDate >= DateTime.Today
                ? new EpdContactView(item.CommentContact)
                : null;
        IsHearingScheduled = item.IsHearingScheduled;
        HearingDate = item.HearingDate;
        HearingLocation = item.HearingLocation;
        HearingCommentPeriodClosesDate = item.HearingCommentPeriodClosesDate;
        HearingContact = item.HearingContactId.HasValue &&
            item.HearingCommentPeriodClosesDate.HasValue &&
            item.HearingCommentPeriodClosesDate >= DateTime.Today
                ? new EpdContactView(item.HearingContact)
                : null;
        Attachments = item.Attachments?
                .Where(a => !a.Deleted)
                .Select(a => new AttachmentView(a)).ToList() 
            ?? new List<AttachmentView>();
    }

    // Common data elements

    [DisplayName("Cause of Order")]
    public string Cause { get; }

    [DisplayName("Requirements of Order")]
    public string Requirements { get; }

    [DisplayName("Settlement Amount")]
    [DataType(DataType.Currency)]
    public decimal? SettlementAmount { get; }

    // Proposed orders

    [DisplayName("Send Comments To")]
    public EpdContactView CommentContact { get; }

    // Executed orders

    // Attachments

    [DisplayName("File Attachments")]
    public List<AttachmentView> Attachments { get; }

    // Hearing info

    [DisplayName("Public Hearing Scheduled")]
    public bool IsHearingScheduled { get; }

    [DisplayName("Hearing Date/Time")]
    [DisplayFormat(DataFormatString = DisplayFormats.DateTimeComposite)]
    public DateTime? HearingDate { get; }

    [DisplayName("Hearing Location")]
    public string HearingLocation { get; }

    [DisplayName("Date Hearing Comment Period Closes")]
    [DisplayFormat(DataFormatString = DisplayFormats.ShortDateComposite)]
    public DateTime? HearingCommentPeriodClosesDate { get; }

    [DisplayName("Hearing Information Contact")]
    public EpdContactView HearingContact { get; }
}
