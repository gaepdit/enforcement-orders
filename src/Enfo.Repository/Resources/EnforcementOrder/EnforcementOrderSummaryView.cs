using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Enfo.Repository.Resources.LegalAuthority;
using Enfo.Repository.Utils;
using JetBrains.Annotations;

namespace Enfo.Repository.Resources.EnforcementOrder
{
    public class EnforcementOrderSummaryView
    {
        public EnforcementOrderSummaryView([NotNull] Domain.Entities.EnforcementOrder item)
        {
            Guard.NotNull(item, nameof(item));

            Id = item.Id;
            FacilityName = item.FacilityName;
            County = item.County;
            LegalAuthority = new LegalAuthorityView(item.LegalAuthority);
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
        public bool IsProposedOrder { get; }

        [DisplayName("Date Comment Period Closes")]
        [DisplayFormat(DataFormatString = DisplayFormats.FormatDateShortComposite)]
        public DateTime? CommentPeriodClosesDate { get; }

        [DisplayName("Publication Date For Proposed Order")]
        [DisplayFormat(DataFormatString = DisplayFormats.FormatDateShortComposite)]
        public DateTime? ProposedOrderPostedDate { get; }

        // Executed orders

        public bool IsPublicExecutedOrder { get; }

        [DisplayName("Enforcement Order Executed")]
        public bool IsExecutedOrder { get; }

        [DisplayName("Date Executed")]
        [DisplayFormat(DataFormatString = DisplayFormats.FormatDateShortComposite)]
        public DateTime? ExecutedDate { get; }

        [DisplayName("Publication Date For Executed Order")]
        [DisplayFormat(DataFormatString = DisplayFormats.FormatDateShortComposite)]
        public DateTime? ExecutedOrderPostedDate { get; }

        // Calculated properties

        [DisplayFormat(DataFormatString = DisplayFormats.FormatDateShortComposite)]
        public DateTime? LastPostedDate => ExecutedDate ?? ProposedOrderPostedDate;

        public bool IsPublic => IsPublicExecutedOrder || IsPublicProposedOrder;
    }
}