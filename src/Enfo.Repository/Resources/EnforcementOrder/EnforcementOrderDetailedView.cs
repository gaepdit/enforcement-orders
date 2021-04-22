using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Enfo.Repository.Resources.EpdContact;
using Enfo.Repository.Utils;
using JetBrains.Annotations;

namespace Enfo.Repository.Resources.EnforcementOrder
{
    public class EnforcementOrderDetailedView : EnforcementOrderSummaryView
    {
        public EnforcementOrderDetailedView([NotNull] Domain.Entities.EnforcementOrder item) : base(item)
        {
            Guard.NotNull(item, nameof(item));

            Cause = item.Cause;
            Requirements = item.Requirements;
            SettlementAmount = item.SettlementAmount;
            CommentContact = item.CommentContactId.HasValue &&
                item.GetIsPublicProposedOrder &&
                item.CommentPeriodClosesDate.HasValue &&
                item.CommentPeriodClosesDate >= DateTime.Today
                    ? new EpdContactView(item.CommentContact)
                    : null;
            IsHearingScheduled = item.IsHearingScheduled;
            HearingDate = item.HearingDate;
            HearingLocation = item.HearingLocation;
            HearingCommentPeriodClosesDate = item.HearingCommentPeriodClosesDate;
            HearingContact = item.HearingContactId.HasValue &&
                item.HearingCommentPeriodClosesDate.HasValue &&
                item.HearingCommentPeriodClosesDate >= DateTime.Today
                    ? new EpdContactView(item.HearingContact)
                    : null;
        }

        // Common data elements

        [DisplayName("Cause of Order")]
        public string Cause { get; }

        [DisplayName("Requirements of Order")]
        public string Requirements { get; }

        [DisplayName("Settlement Amount")]
        [DataType(DataType.Currency)]
        public decimal? SettlementAmount { get; }

        // Proposed orders

        [DisplayName("Send Comments To")]
        public EpdContactView CommentContact { get; }

        // Executed orders

        // Hearing info

        [DisplayName("Public Hearing Scheduled")]
        public bool IsHearingScheduled { get; }

        [DisplayName("Hearing Date/Time")]
        [DisplayFormat(DataFormatString = DisplayFormats.ShortDateComposite)]
        public DateTime? HearingDate { get; }

        [DisplayName("Hearing Location")]
        public string HearingLocation { get; }

        [DisplayName("Date Hearing Comment Period Closes")]
        [DisplayFormat(DataFormatString = DisplayFormats.ShortDateComposite)]
        public DateTime? HearingCommentPeriodClosesDate { get; }

        [DisplayName("Hearing Information Contact")]
        public EpdContactView HearingContact { get; }
    }
}