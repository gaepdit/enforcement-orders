using Enfo.Domain.Entities;

namespace Enfo.Domain.Querying
{
    public class EnforcementOrderIncludeLegalAuth : Inclusion<EnforcementOrder>
    {
        public EnforcementOrderIncludeLegalAuth()
        {
            AddInclude(e => e.LegalAuthority);
        }
    }
}
