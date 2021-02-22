using Enfo.Domain.Entities;
using static Enfo.Domain.Entities.EnforcementOrder;
using static Enfo.Domain.Entities.Enums;

namespace Enfo.Repository.Querying
{
    public class FilterOrdersByPublicationStatus : Specification<EnforcementOrder>
    {
        public FilterOrdersByPublicationStatus(PublicationStatus status)
        {
            switch (status)
            {
                case PublicationStatus.Published:
                    ApplyCriteria(e => e.PublicationStatus == PublicationState.Published);
                    break;

                case PublicationStatus.Draft:
                    ApplyCriteria(e => e.PublicationStatus == PublicationState.Draft);
                    break;

                case PublicationStatus.All:
                    ApplyCriteria(e => true);
                    break;
            }
        }
    }
}
