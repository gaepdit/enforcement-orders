using System;
using System.ComponentModel;
using Enfo.Domain.Entities;
using Enfo.Domain.Utils;

namespace Enfo.API.Resources
{
    public class EnforcementOrderListResource
    {
        public int Id { get; set; }

        // Common data elements

        [DisplayName("Facility")]
        public string FacilityName { get; set; }

        public string County { get; set; }

        [DisplayName("Legal Authority")]
        public LegalAuthorityResource LegalAuthority { get; set; }

        [DisplayName("Order Number")]
        public string OrderNumber { get; set; }

        // Proposed orders

        public bool IsPublicProposedOrder { get; set; }

        [DisplayName("Proposed Order Public Noticed")]
        public bool IsProposedOrder { get; set; }

        [DisplayName("Date Comment Period Closes")]
        public DateTime? CommentPeriodClosesDate { get; set; }

        [DisplayName("Publication Date For Proposed Order")]
        public DateTime? ProposedOrderPostedDate { get; set; }

        // Executed orders

        public bool IsPublicExecutedOrder { get; set; }

        [DisplayName("Enforcement Order Executed")]
        public bool IsExecutedOrder { get; set; }

        [DisplayName("Date Executed")]
        public DateTime? ExecutedDate { get; set; }

        // Constructor

        public EnforcementOrderListResource(EnforcementOrder item)
        {
            Guard.NotNull(item, nameof(item));

            Id = item.Id;

            FacilityName = item.FacilityName;
            County = item.County;
            LegalAuthority = new LegalAuthorityResource(item.LegalAuthority);
            OrderNumber = item.OrderNumber;

            IsPublicProposedOrder = item.GetIsPublicProposedOrder();
            IsProposedOrder = item.IsProposedOrder;
            CommentPeriodClosesDate = item.CommentPeriodClosesDate;
            ProposedOrderPostedDate = item.ProposedOrderPostedDate;

            IsPublicExecutedOrder = item.GetIsPublicExecutedOrder();
            IsExecutedOrder = item.IsExecutedOrder;
            ExecutedDate = item.ExecutedDate;

            if (!IsPublicProposedOrder)
            {
                IsProposedOrder = false;
                CommentPeriodClosesDate = null;
                ProposedOrderPostedDate = null;
            }

            if (!IsPublicExecutedOrder)
            {
                IsExecutedOrder = false;
                ExecutedDate = null;
            }
        }
    }
}
