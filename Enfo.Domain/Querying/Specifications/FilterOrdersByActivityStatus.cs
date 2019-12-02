using Enfo.Domain.Entities;
using static Enfo.Domain.Entities.Enums;

namespace Enfo.Domain.Querying
{
    public class FilterOrdersByActivityStatus : Specification<EnforcementOrder>
    {
        public FilterOrdersByActivityStatus(ActivityStatus status)
        {
            switch (status)
            {
                case ActivityStatus.Proposed:
                    ApplyCriteria(e => e.IsProposedOrder && !e.IsExecutedOrder);
                    break;

                case ActivityStatus.Executed:
                    ApplyCriteria(e => e.IsExecutedOrder);
                    break;

                default:
                    ApplyCriteria(e => true);
                    break;
            }
        }
    }
}
