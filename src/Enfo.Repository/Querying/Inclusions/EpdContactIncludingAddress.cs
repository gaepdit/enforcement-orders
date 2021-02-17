using Enfo.Domain.Entities;

namespace Enfo.Repository.Querying
{
    public class EpdContactIncludingAddress : Inclusion<EpdContact>
    {
        public EpdContactIncludingAddress()
        {
            AddInclude(e => e.Address);
        }
    }
}
