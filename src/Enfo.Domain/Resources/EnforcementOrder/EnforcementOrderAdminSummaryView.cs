using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Enfo.Domain.Resources.LegalAuthority;
using Enfo.Domain.Utils;
using JetBrains.Annotations;

namespace Enfo.Domain.Resources.EnforcementOrder
{
    public class EnforcementOrderAdminSummaryView
    {
        public EnforcementOrderAdminSummaryView([NotNull] Domain.Entities.EnforcementOrder item)
        {
            Guard.NotNull(item, nameof(item));

            Id = item.Id;
            Deleted = item.Deleted;
            FacilityName = item.FacilityName;
            County = item.County;
            LegalAuthority = item.LegalAuthority == null ? null : new LegalAuthorityView(item.LegalAuthority);
            OrderNumber = item.OrderNumber;
            IsProposedOrder = item.IsProposedOrder;
            ProposedOrderPostedDate = item.ProposedOrderPostedDate;
            IsExecutedOrder = item.IsExecutedOrder;
            ExecutedDate = item.ExecutedDate;
        }

        public int Id { get; }
        public bool Deleted { get; }

        [DisplayName("Facility")]
        public string FacilityName { get; }

        public string County { get; }

        [DisplayName("Legal Authority")]
        public LegalAuthorityView LegalAuthority { get; }

        [DisplayName("Order Number")]
        public string OrderNumber { get; }

        [DisplayFormat(DataFormatString = DisplayFormats.ShortDateComposite)]
        public DateTime? PendingPublicationDate => ExecutedDate ?? ProposedOrderPostedDate;

        // Proposed orders

        [DisplayName("Proposed Order Public Noticed")]
        public bool IsProposedOrder { get; }

        [DisplayName("Publication Date For Proposed Order")]
        [DisplayFormat(DataFormatString = DisplayFormats.ShortDateComposite)]
        public DateTime? ProposedOrderPostedDate { get; }

        // Executed orders

        [DisplayName("Enforcement Order Executed")]
        public bool IsExecutedOrder { get; }

        [DisplayName("Date Executed")]
        [DisplayFormat(DataFormatString = DisplayFormats.ShortDateComposite)]
        public DateTime? ExecutedDate { get; }
    }
}
