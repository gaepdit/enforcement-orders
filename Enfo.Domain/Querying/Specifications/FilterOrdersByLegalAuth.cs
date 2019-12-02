using Enfo.Domain.Entities;
using Enfo.Domain.Utils;

namespace Enfo.Domain.Querying
{
    public class FilterOrdersByLegalAuth : Specification<EnforcementOrder>
    {
        public FilterOrdersByLegalAuth(int legalAuth)
        {
            Check.Positive(legalAuth, nameof(legalAuth));
            ApplyCriteria(e => e.LegalAuthorityId.Equals(legalAuth));
        }
    }
}
