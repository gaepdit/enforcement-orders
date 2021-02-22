using System;
using System.ComponentModel;
using Enfo.Repository.Resources.LegalAuthority;

namespace Enfo.Repository.Resources.EnforcementOrder
{
    public class EnforcementOrderSummaryView
    {
        public int Id { get; set; }
        public bool Active { get; set; }

        // Common data elements

        [DisplayName("Facility")]
        public string FacilityName { get; set; }

        public string County { get; set; }

        [DisplayName("Legal Authority")]
        public LegalAuthorityView LegalAuthority { get; set; }

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
    }
}