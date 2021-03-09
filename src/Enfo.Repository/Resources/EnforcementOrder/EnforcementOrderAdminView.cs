using System;
using System.ComponentModel;
using Enfo.Repository.Utils;
using JetBrains.Annotations;

namespace Enfo.Repository.Resources.EnforcementOrder
{
    public class EnforcementOrderAdminView : EnforcementOrderDetailedView
    {
        public EnforcementOrderAdminView([NotNull] Domain.Entities.EnforcementOrder item) : base(item)
        {
            Guard.NotNull(item, nameof(item));

            Deleted = item.Deleted;
            PublicationStatus = GetResourcePublicationState(item.PublicationStatus);
            LastPostedDate = item.GetLastPostedDate();
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

        [DisplayName("Last Posted Date")]
        public DateTime? LastPostedDate { get; }
    }
}