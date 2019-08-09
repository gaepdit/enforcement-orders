using Enfo.Domain.Entities;

namespace Enfo.Domain.Specifications
{
    public class EpdContactIncludeAddressSpec : Specification<EpdContact>
    {
        public EpdContactIncludeAddressSpec() => AddInclude(e => e.Address);
    }
}
