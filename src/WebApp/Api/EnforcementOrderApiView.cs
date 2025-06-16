using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EpdContacts.Resources;
using Enfo.Domain.LegalAuthorities.Resources;
using JetBrains.Annotations;

namespace Enfo.WebApp.Api;

[PublicAPI]
public class EnforcementOrderApiView
{
    public EnforcementOrderApiView(EnforcementOrderDetailedView e, string baseUrl)
    {
        _baseUrl = new Uri(baseUrl);
        Id = e.Id;
        FacilityName = e.FacilityName;
        County = e.County;
        LegalAuthority = e.LegalAuthority;
        OrderNumber = e.OrderNumber;
        IsProposedOrder = e.IsPublicProposedOrder;
        CommentPeriodClosesDate = e.CommentPeriodClosesDate;
        ProposedOrderPostedDate = e.ProposedOrderPostedDate;
        IsExecutedOrder = e.IsPublicExecutedOrder;
        ExecutedDate = e.ExecutedDate;
        ExecutedOrderPostedDate = e.ExecutedOrderPostedDate;
        Cause = e.Cause;
        Requirements = e.Requirements;
        SettlementAmount = e.SettlementAmount;
        CommentContact = e.CommentContact;
        IsHearingScheduled = e.IsHearingScheduled;
        HearingDate = e.HearingDate;
        HearingLocation = e.HearingLocation;
        HearingCommentPeriodClosesDate = e.HearingCommentPeriodClosesDate;
        HearingContact = e.HearingContact;
        Attachments = e.Attachments.Select(a => new AttachmentApiView(a, _baseUrl)).ToList();
    }

    private readonly Uri _baseUrl;
    public string Link => new Uri(_baseUrl, $"Details/{Id}").AbsoluteUri;
    public int Id { get; }
    public string FacilityName { get; }
    public string County { get; }
    public LegalAuthorityView LegalAuthority { get; }
    public string OrderNumber { get; }
    public bool IsProposedOrder { get; }
    public DateTime? CommentPeriodClosesDate { get; }
    public DateTime? ProposedOrderPostedDate { get; }
    public bool IsExecutedOrder { get; }
    public DateTime? ExecutedDate { get; }
    public DateTime? ExecutedOrderPostedDate { get; }
    public string Cause { get; }
    public string Requirements { get; }
    public decimal? SettlementAmount { get; }
    public EpdContactView CommentContact { get; }
    public bool IsHearingScheduled { get; }
    public DateTime? HearingDate { get; }
    public string HearingLocation { get; }
    public DateTime? HearingCommentPeriodClosesDate { get; }
    public EpdContactView HearingContact { get; }
    public List<AttachmentApiView> Attachments { get; }

    [PublicAPI]
    public class AttachmentApiView(AttachmentView a, Uri baseUrl)
    {
        public Guid Id { get; } = a.Id;
        public string FileName { get; } = a.FileName;
        public long Size { get; } = a.Size;
        public DateTime DateUploaded { get; } = a.DateUploaded;
        public string Link => new Uri(baseUrl, $"Attachment/{Id}/").AbsoluteUri;
    }
}
