using Enfo.Domain.BaseEntities;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EpdContacts.Entities;
using Enfo.Domain.LegalAuthorities.Entities;

namespace Enfo.Domain.EnforcementOrders.Entities;

public class EnforcementOrder : IdentifiedEntity, IAuditable
{
    public EnforcementOrder() { }

    public EnforcementOrder(EnforcementOrderCreate resource)
    {
        Guard.NotNull(resource);

        Cause = Guard.NotNullOrWhiteSpace(resource.Cause);
        CommentContactId = resource.CreateAs == NewEnforcementOrderType.Proposed
            ? resource.CommentContactId
            : null;
        CommentPeriodClosesDate = resource.CreateAs == NewEnforcementOrderType.Proposed
            ? resource.CommentPeriodClosesDate
            : null;
        County = Guard.NotNullOrWhiteSpace(resource.County);
        ExecutedDate = resource.CreateAs == NewEnforcementOrderType.Executed ? resource.ExecutedDate : null;
        ExecutedOrderPostedDate = resource.CreateAs == NewEnforcementOrderType.Executed
            ? resource.ExecutedOrderPostedDate
            : null;
        FacilityName = Guard.NotNullOrWhiteSpace(resource.FacilityName);
        HearingCommentPeriodClosesDate =
            resource.IsHearingScheduled ? resource.HearingCommentPeriodClosesDate : null;
        HearingContactId = resource.IsHearingScheduled ? resource.HearingContactId : null;
        HearingDate = resource.IsHearingScheduled ? resource.HearingDate : null;
        HearingLocation = resource.IsHearingScheduled ? resource.HearingLocation : null;
        IsExecutedOrder = resource.CreateAs == NewEnforcementOrderType.Executed;
        IsHearingScheduled = resource.IsHearingScheduled;
        IsProposedOrder = resource.CreateAs == NewEnforcementOrderType.Proposed;
        LegalAuthorityId = resource.LegalAuthorityId ?? 0;
        OrderNumber = Guard.NotNullOrWhiteSpace(resource.OrderNumber);
        ProposedOrderPostedDate = resource.CreateAs == NewEnforcementOrderType.Proposed
            ? resource.ProposedOrderPostedDate
            : null;
        PublicationStatus = GetEntityPublicationProgress(resource.Progress);
        Requirements = resource.Requirements;
        SettlementAmount = resource.SettlementAmount;
    }

    // enums
    public enum PublicationState
    {
        Draft,
        Published,
    }

    // Common data elements

    [Required]
    public string FacilityName { get; set; }

    [Required]
    [StringLength(20)]
    public string County { get; set; }

    [DisplayName("Legal Authority")]
    public LegalAuthority LegalAuthority { get; set; }

    [Required]
    public int LegalAuthorityId { get; set; }

    [Required]
    public string Cause { get; set; }

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

    public ICollection<Attachment> Attachments { get; set; }

    // Hearing info

    public bool IsHearingScheduled { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? HearingDate { get; set; }

    public string HearingLocation { get; set; }

    [DataType(DataType.Date)]
    [Column(TypeName = "Date")]
    public DateTime? HearingCommentPeriodClosesDate { get; set; }

    public EpdContact HearingContact { get; set; }
    public int? HearingContactId { get; set; }

    public void ApplyUpdate(EnforcementOrderUpdate resource)
    {
        Guard.NotNull(resource);

        resource.TrimAll();
        Cause = Guard.NotNullOrWhiteSpace(resource.Cause);
        CommentContactId = IsProposedOrder ? resource.CommentContactId : null;
        CommentPeriodClosesDate = IsProposedOrder ? resource.CommentPeriodClosesDate : null;
        County = Guard.NotNullOrWhiteSpace(resource.County);
        ExecutedDate = resource.IsExecutedOrder ? resource.ExecutedDate : null;
        ExecutedOrderPostedDate = resource.IsExecutedOrder ? resource.ExecutedOrderPostedDate : null;
        FacilityName = Guard.NotNullOrWhiteSpace(resource.FacilityName);
        HearingCommentPeriodClosesDate =
            resource.IsHearingScheduled ? resource.HearingCommentPeriodClosesDate : null;
        HearingContactId = resource.IsHearingScheduled ? resource.HearingContactId : null;
        HearingDate = resource.IsHearingScheduled ? resource.HearingDate : null;
        HearingLocation = resource.IsHearingScheduled ? resource.HearingLocation : null;
        IsProposedOrder = resource.IsProposedOrder;
        IsExecutedOrder = resource.IsExecutedOrder;
        IsHearingScheduled = resource.IsHearingScheduled;
        LegalAuthorityId = resource.LegalAuthorityId;
        OrderNumber = Guard.NotNullOrWhiteSpace(resource.OrderNumber);
        ProposedOrderPostedDate = IsProposedOrder ? resource.ProposedOrderPostedDate : null;
        PublicationStatus = GetEntityPublicationProgress(resource.Progress);
        Requirements = resource.Requirements;
        SettlementAmount = resource.SettlementAmount;
    }

    private static PublicationState GetEntityPublicationProgress(PublicationProgress status) =>
        status switch
        {
            PublicationProgress.Draft => PublicationState.Draft,
            PublicationProgress.Published => PublicationState.Published,
            _ => throw new InvalidEnumArgumentException(nameof(status), (int)status, typeof(PublicationState)),
        };
}
