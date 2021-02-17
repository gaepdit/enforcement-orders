using Enfo.Domain.Entities;

namespace Enfo.Repository.Querying
{
    public class EnforcementOrderIncludeLegalAuth : Inclusion<EnforcementOrder>
    {
        public EnforcementOrderIncludeLegalAuth()
        {
            AddInclude(e => e.LegalAuthority);
        }
    }
}
