using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Enfo.Repository.Resources.EpdContact;
using Enfo.Repository.Resources.LegalAuthority;

namespace Enfo.Repository.Resources.EnforcementOrder
{
    public class EnforcementOrderView
    {
        public int Id { get; set; }
        public bool Active { get; set; }

        // Common data elements

        [DisplayName("Facility")]
        public string FacilityName { get; set; }

        public string County { get; set; }

        [DisplayName("Legal Authority")]
        public LegalAuthorityView LegalAuthority { get; set; }

        [DisplayName("Cause of Order")]
        public string Cause { get; set; }

        [DisplayName("Requirements of Order")]
        public string Requirements { get; set; }

        [DisplayName("Settlement Amount")]
        [DataType(DataType.Currency)]
        public decimal? SettlementAmount { get; set; }

        [DisplayName("Order Number")]
        public string OrderNumber { get; set; }

        // Proposed orders

        public bool IsPublicProposedOrder { get; set; }

        [DisplayName("Proposed Order Public Noticed")]
        public bool IsProposedOrder { get; set; }

        [DisplayName("Date Comment Period Closes")]
        public DateTime? CommentPeriodClosesDate { get; set; }

        [DisplayName("Send Comments To")]
        public EpdContactView CommentContact { get; set; }

        [DisplayName("Publication Date For Proposed Order")]
        public DateTime? ProposedOrderPostedDate { get; set; }

        // Executed orders

        public bool IsPublicExecutedOrder { get; set; }

        [DisplayName("Enforcement Order Executed")]
        public bool IsExecutedOrder { get; set; }

        [DisplayName("Date Executed")]
        public DateTime? ExecutedDate { get; set; }

        // Hearing info

        [DisplayName("Public Hearing Scheduled")]
        public bool IsHearingScheduled { get; set; } = false;

        [DisplayName("Hearing Date/Time")]
        public DateTime? HearingDate { get; set; }

        [DisplayName("Hearing Location")]
        public string HearingLocation { get; set; }

        [DisplayName("Date Hearing Comment Period Closes")]
        public DateTime? HearingCommentPeriodClosesDate { get; set; }

        [DisplayName("Hearing Information Contact")]
        public EpdContactView HearingContact { get; set; }
    }
}