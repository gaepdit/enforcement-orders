using System;
using System.ComponentModel;
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
            IsPublicProposedOrder = item.GetIsPublicProposedOrder();
            IsProposedOrder = item.GetIsPublicProposedOrder() && item.IsProposedOrder;
            CommentPeriodClosesDate = item.GetIsPublicProposedOrder() ? item.CommentPeriodClosesDate : null;
            ProposedOrderPostedDate = item.GetIsPublicProposedOrder() ? item.ProposedOrderPostedDate : null;
            IsPublicExecutedOrder = item.GetIsPublicExecutedOrder();
            IsExecutedOrder = item.GetIsPublicExecutedOrder() && item.IsExecutedOrder;
            ExecutedDate = item.GetIsPublicExecutedOrder() ? item.ExecutedDate : null;
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
        
        public DateTime? LastPostedDate => ExecutedDate ?? ProposedOrderPostedDate;

        // Proposed orders

        public bool IsPublicProposedOrder { get; }

        [DisplayName("Proposed Order Public Noticed")]
        public bool IsProposedOrder { get; }

        [DisplayName("Date Comment Period Closes")]
        public DateTime? CommentPeriodClosesDate { get; }

        [DisplayName("Publication Date For Proposed Order")]
        public DateTime? ProposedOrderPostedDate { get; }

        // Executed orders

        public bool IsPublicExecutedOrder { get; }

        [DisplayName("Enforcement Order Executed")]
        public bool IsExecutedOrder { get; }

        [DisplayName("Date Executed")]
        public DateTime? ExecutedDate { get; }
    }
}