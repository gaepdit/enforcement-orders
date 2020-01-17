using Enfo.Domain.Entities;

namespace Enfo.Domain.Querying
{
    public class EpdContactIncludingAddress : Inclusion<EpdContact>
    {
        public EpdContactIncludingAddress()
        {
            AddInclude(e => e.Address);
        }
    }
}
