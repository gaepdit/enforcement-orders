using Enfo.Domain.Entities;

namespace Enfo.Domain.Querying
{
    public class EnforcementOrderIncludeAll : Inclusion<EnforcementOrder>
    {
        public EnforcementOrderIncludeAll()
        {
            AddInclude(e => e.LegalAuthority);
            AddInclude(e => e.CommentContact);
            AddInclude(e => e.HearingContact);
        }
    }
}
