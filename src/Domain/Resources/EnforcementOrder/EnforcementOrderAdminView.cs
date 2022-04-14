using Enfo.Domain.Resources.EpdContact;
using Enfo.Domain.Utils;

namespace Enfo.Domain.Resources.EnforcementOrder;

public class EnforcementOrderAdminView : EnforcementOrderAdminSummaryView
{
    public EnforcementOrderAdminView([NotNull] Domain.Entities.EnforcementOrder item) : base(item)
    {
        Guard.NotNull(item, nameof(item));

        PublicationStatus = GetResourcePublicationState(item.PublicationStatus);
        Cause = item.Cause;
        Requirements = item.Requirements;
        SettlementAmount = item.SettlementAmount;
        CommentContact = item.CommentContact == null ? null : new EpdContactView(item.CommentContact);
        IsHearingScheduled = item.IsHearingScheduled;
        HearingDate = item.HearingDate;
        HearingLocation = item.HearingLocation;
        HearingCommentPeriodClosesDate = item.HearingCommentPeriodClosesDate;
        HearingContact = item.HearingContact == null ? null : new EpdContactView(item.HearingContact);
        IsPublicProposedOrder = item.GetIsPublicProposedOrder;
        CommentPeriodClosesDate = item.CommentPeriodClosesDate;
        IsPublicExecutedOrder = item.GetIsPublicExecutedOrder;
        ExecutedOrderPostedDate = item.ExecutedOrderPostedDate;
    }

    private static PublicationProgress GetResourcePublicationState(
        Domain.Entities.EnforcementOrder.PublicationState status) =>
        status switch
        {
            Entities.EnforcementOrder.PublicationState.Draft => PublicationProgress.Draft,
            Entities.EnforcementOrder.PublicationState.Published => PublicationProgress.Published,
            _ => throw new InvalidEnumArgumentException(nameof(status), (int)status,
                typeof(Domain.Entities.EnforcementOrder.PublicationState))
        };

    [DisplayName("Progress")]
    public PublicationProgress PublicationStatus { get; }

    // Common data elements

    [DisplayName("Cause of Order")]
    public string Cause { get; }

    [DisplayName("Requirements of Order")]
    public string Requirements { get; }

    [DisplayName("Settlement Amount")]
    [DataType(DataType.Currency)]
    public decimal? SettlementAmount { get; }

    // Proposed orders

    private bool IsPublicProposedOrder { get; }

    [DisplayName("Send Comments To")]
    public EpdContactView CommentContact { get; }

    [DisplayName("Date Comment Period Closes")]
    [DisplayFormat(DataFormatString = DisplayFormats.ShortDateComposite)]
    public DateTime? CommentPeriodClosesDate { get; }

    // Executed orders

    private bool IsPublicExecutedOrder { get; }

    [DisplayName("Publication Date For Executed Order")]
    [DisplayFormat(DataFormatString = DisplayFormats.ShortDateComposite)]
    public DateTime? ExecutedOrderPostedDate { get; }

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

    // Calculated properties

    public bool IsPublic => IsPublicExecutedOrder || IsPublicProposedOrder;
}
