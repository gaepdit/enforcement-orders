using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Enfo.Repository.Utils;
using JetBrains.Annotations;

namespace Enfo.Repository.Resources.EnforcementOrder
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
            PendingPublicationDate = item.ExecutedDate ?? item.ProposedOrderPostedDate;
        }

        public int Id { get; }
        public bool Deleted { get; }

        [DisplayName("Facility")]
        public string FacilityName { get; }

        public string County { get; }

        [DisplayFormat(DataFormatString = DisplayFormats.FormatDateShortComposite)]
        public DateTime? PendingPublicationDate { get; }
    }
}