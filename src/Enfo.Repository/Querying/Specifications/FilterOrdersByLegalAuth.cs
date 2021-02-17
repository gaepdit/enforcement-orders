using Enfo.Domain.Entities;
using Enfo.Repository.Utils;

namespace Enfo.Repository.Querying
{
    public class FilterOrdersByLegalAuth : Specification<EnforcementOrder>
    {
        public FilterOrdersByLegalAuth(int legalAuth)
        {
            Guard.Positive(legalAuth, nameof(legalAuth));
            ApplyCriteria(e => e.LegalAuthorityId.Equals(legalAuth));
        }
    }
}
