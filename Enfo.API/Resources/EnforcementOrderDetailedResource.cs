using Enfo.Domain.Entities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Enfo.Domain.Entities.EnforcementOrder;

namespace Enfo.API.Resources
{
    public class EnforcementOrderDetailedResource
    {
        public int Id { get; set; }

        // Common data elements
        [DisplayName("Facility")]
        public string FacilityName { get; set; }

        public string County { get; set; }

        [DisplayName("Legal Authority")]
        public LegalAuthorityResource LegalAuthority { get; set; }

        [DisplayName("Cause of Order")]
        public string Cause { get; set; }

        [DisplayName("Requirements of Order")]
        public string Requirements { get; set; }

        [DisplayName("Settlement Amount")]
        [DataType(DataType.Currency)]
        public decimal? SettlementAmount { get; set; }

        public bool Deleted { get; set; }

        [DisplayName("Progress")]
        public PublicationState PublicationStatus { get; set; }

        [DisplayName("Order Number")]
        public string OrderNumber { get; set; }

        [DisplayName("Last Posted Date")]
        public DateTime? LastPostedDate { get; set; }

        // Proposed orders

        private readonly bool IsPublicProposedOrder;

        [DisplayName("Proposed Order Public Noticed")]
        public bool IsProposedOrder { get; set; }

        [DisplayName("Date Comment Period Closes")]
        public DateTime? CommentPeriodClosesDate { get; set; }

        [DisplayName("Send Comments To")]
        public EpdContactResource CommentContact { get; set; }

        [DisplayName("Publication Date For Proposed Order")]
        public DateTime? ProposedOrderPostedDate { get; set; }

        // Executed orders

        private readonly bool IsPublicExecutedOrder;

        [DisplayName("Enforcement Order Executed")]
        public bool IsExecutedOrder { get; set; }

        [DisplayName("Date Executed")]
        public DateTime? ExecutedDate { get; set; }

        // Hearing info

        [DisplayName("Public Hearing Scheduled")]
        public bool IsHearingScheduled { get; set; }

        [DisplayName("Hearing Date/Time")]
        public DateTime? HearingDate { get; set; }

        [DisplayName("Hearing Location")]
        public string HearingLocation { get; set; }

        [DisplayName("Date Hearing Comment Period Closes")]
        public DateTime? HearingCommentPeriodClosesDate { get; set; }

        [DisplayName("Hearing Information Contact")]
        public EpdContactResource HearingContact { get; set; }

        // Constructors

        public EnforcementOrderDetailedResource() { }

        public EnforcementOrderDetailedResource(EnforcementOrder item)
        {
            if (item != null)
            {
                Id = item.Id;

                FacilityName = item.FacilityName;
                County = item.County;
                LegalAuthority = new LegalAuthorityResource(item.LegalAuthority);
                Cause = item.Cause;
                Requirements = item.Requirements;
                SettlementAmount = item.SettlementAmount;
                Deleted = item.Deleted;
                PublicationStatus = item.PublicationStatus;
                OrderNumber = item.OrderNumber;
                LastPostedDate = item.LastPostedDate;

                IsPublicProposedOrder = item.IsPublicProposedOrder;
                IsProposedOrder = item.IsProposedOrder;
                CommentPeriodClosesDate = item.CommentPeriodClosesDate;
                CommentContact = new EpdContactResource(item.CommentContact);
                ProposedOrderPostedDate = item.ProposedOrderPostedDate;

                IsPublicExecutedOrder = item.IsPublicExecutedOrder;
                IsExecutedOrder = item.IsExecutedOrder;
                ExecutedDate = item.ExecutedDate;

                IsHearingScheduled = item.IsHearingScheduled;
                HearingDate = item.HearingDate;
                HearingLocation = item.HearingLocation;
                HearingCommentPeriodClosesDate = item.HearingCommentPeriodClosesDate;
                HearingContact = new EpdContactResource(item.HearingContact);
            }
        }
    }
}
