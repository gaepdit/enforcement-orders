using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Enfo.Repository.Resources.EpdContact;
using Enfo.Repository.Resources.LegalAuthority;
using Enfo.Repository.Utils;
using JetBrains.Annotations;

namespace Enfo.Repository.Resources.EnforcementOrder
{
    public class EnforcementOrderAdminView : EnforcementOrderAdminSummaryView
    {
        public EnforcementOrderAdminView([NotNull] Domain.Entities.EnforcementOrder item) : base(item)
        {
            Guard.NotNull(item, nameof(item));

            Deleted = item.Deleted;
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
            LegalAuthority = item.LegalAuthority == null ? null : new LegalAuthorityView(item.LegalAuthority);
            OrderNumber = item.OrderNumber;
            IsPublicProposedOrder = item.GetIsPublicProposedOrder;
            IsProposedOrder = item.IsProposedOrder;
            CommentPeriodClosesDate = item.CommentPeriodClosesDate;
            ProposedOrderPostedDate = item.ProposedOrderPostedDate;
            IsPublicExecutedOrder = item.GetIsPublicExecutedOrder;
            IsExecutedOrder = IsPublicExecutedOrder && item.IsExecutedOrder;
            ExecutedDate = item.ExecutedDate;
            ExecutedOrderPostedDate = item.ExecutedOrderPostedDate;
        }

        private static PublicationState GetResourcePublicationState(
            Domain.Entities.EnforcementOrder.PublicationState status) =>
            status switch
            {
                Domain.Entities.EnforcementOrder.PublicationState.Draft => PublicationState.Draft,
                Domain.Entities.EnforcementOrder.PublicationState.Published => PublicationState.Published,
                _ => throw new InvalidEnumArgumentException(nameof(status), (int) status,
                    typeof(Domain.Entities.EnforcementOrder.PublicationState))
            };

        public bool Deleted { get; }

        [DisplayName("Progress")]
        public PublicationState PublicationStatus { get; }

        // Common data elements

        [DisplayName("Legal Authority")]
        public LegalAuthorityView LegalAuthority { get; }

        [DisplayName("Order Number")]
        public string OrderNumber { get; }

        [DisplayName("Cause of Order")]
        public string Cause { get; }

        [DisplayName("Requirements of Order")]
        public string Requirements { get; }

        [DisplayName("Settlement Amount")]
        [DataType(DataType.Currency)]
        public decimal? SettlementAmount { get; }

        // Proposed orders

        private bool IsPublicProposedOrder { get; }

        [DisplayName("Proposed Order Public Noticed")]
        public bool IsProposedOrder { get; }

        [DisplayName("Send Comments To")]
        public EpdContactView CommentContact { get; }

        [DisplayName("Date Comment Period Closes")]
        [DisplayFormat(DataFormatString = DisplayFormats.FormatDateShortComposite)]
        public DateTime? CommentPeriodClosesDate { get; }

        [DisplayName("Publication Date For Proposed Order")]
        [DisplayFormat(DataFormatString = DisplayFormats.FormatDateShortComposite)]
        public DateTime? ProposedOrderPostedDate { get; }

        // Executed orders

        private bool IsPublicExecutedOrder { get; }

        [DisplayName("Enforcement Order Executed")]
        public bool IsExecutedOrder { get; }

        [DisplayName("Date Executed")]
        [DisplayFormat(DataFormatString = DisplayFormats.FormatDateShortComposite)]
        public DateTime? ExecutedDate { get; }

        [DisplayName("Publication Date For Executed Order")]
        [DisplayFormat(DataFormatString = DisplayFormats.FormatDateShortComposite)]
        public DateTime? ExecutedOrderPostedDate { get; }

        // Hearing info

        [DisplayName("Public Hearing Scheduled")]
        public bool IsHearingScheduled { get; }

        [DisplayName("Hearing Date/Time")]
        [DisplayFormat(DataFormatString = DisplayFormats.FormatDateShortComposite)]
        public DateTime? HearingDate { get; }

        [DisplayName("Hearing Location")]
        public string HearingLocation { get; }

        [DisplayName("Date Hearing Comment Period Closes")]
        [DisplayFormat(DataFormatString = DisplayFormats.FormatDateShortComposite)]
        public DateTime? HearingCommentPeriodClosesDate { get; }

        [DisplayName("Hearing Information Contact")]
        public EpdContactView HearingContact { get; }

        // Calculated properties

        public bool IsPublic => IsPublicExecutedOrder || IsPublicProposedOrder;
    }
}