﻿using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.Domain.Utils;

namespace Enfo.Domain.EnforcementOrders.Resources;

public class EnforcementOrderSummaryView
{
    public EnforcementOrderSummaryView([NotNull] EnforcementOrder item)
    {
        Guard.NotNull(item);

        Id = item.Id;
        FacilityName = item.FacilityName;
        County = item.County;
        LegalAuthority = item.LegalAuthority == null ? null : new LegalAuthorityView(item.LegalAuthority);
        OrderNumber = item.OrderNumber;
        IsPublicProposedOrder = item.GetIsPublicProposedOrder;
        IsProposedOrder = IsPublicProposedOrder && item.IsProposedOrder;
        CommentPeriodClosesDate = IsPublicProposedOrder ? item.CommentPeriodClosesDate : null;
        ProposedOrderPostedDate = IsPublicProposedOrder ? item.ProposedOrderPostedDate : null;
        IsPublicExecutedOrder = item.GetIsPublicExecutedOrder;
        IsExecutedOrder = IsPublicExecutedOrder && item.IsExecutedOrder;
        ExecutedDate = IsPublicExecutedOrder ? item.ExecutedDate : null;
        ExecutedOrderPostedDate = IsPublicExecutedOrder ? item.ExecutedOrderPostedDate : null;
    }

    public int Id { get; }

    // Common data elements

    [DisplayName("Facility")]
    public string FacilityName { get; }

    public string County { get; }

    [DisplayName("Legal Authority")]
    public LegalAuthorityView LegalAuthority { get; }

    [DisplayName("Order Number")]
    public string OrderNumber { get; }

    // Proposed orders

    public bool IsPublicProposedOrder { get; }

    [DisplayName("Proposed Order Public Noticed")]
    [JsonIgnore]
    public bool IsProposedOrder { get; }

    [DisplayName("Date Comment Period Closes")]
    [DisplayFormat(DataFormatString = DisplayFormats.ShortDateComposite)]
    public DateTime? CommentPeriodClosesDate { get; }

    [DisplayName("Publication Date For Proposed Order")]
    [DisplayFormat(DataFormatString = DisplayFormats.ShortDateComposite)]
    public DateTime? ProposedOrderPostedDate { get; }

    // Executed orders

    public bool IsPublicExecutedOrder { get; }

    [DisplayName("Enforcement Order Executed")]
    [JsonIgnore]
    public bool IsExecutedOrder { get; }

    [DisplayName("Date Executed")]
    [DisplayFormat(DataFormatString = DisplayFormats.ShortDateComposite)]
    public DateTime? ExecutedDate { get; }

    [DisplayName("Publication Date For Executed Order")]
    [DisplayFormat(DataFormatString = DisplayFormats.ShortDateComposite)]
    public DateTime? ExecutedOrderPostedDate { get; }
}
