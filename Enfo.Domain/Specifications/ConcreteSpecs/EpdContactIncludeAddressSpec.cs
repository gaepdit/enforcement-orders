using Enfo.Domain.Entities;

namespace Enfo.Domain.Specifications
{
    public class EpdContactIncludeAddressSpec : BaseSpecification<EpdContact>
    {
        public EpdContactIncludeAddressSpec(bool includeInactive = false)
            : base(e => e.Active || includeInactive)
        {
            AddInclude(e => e.Address);
        }
    }
}
