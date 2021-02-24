using System.Linq;
using Enfo.Infrastructure.Contexts;
using Enfo.Repository.Utils;

namespace Enfo.Infrastructure.TestData
{
    public static partial class TestData
    {
        public static void SeedTestData(this EnfoDbContext context)
        {
            Guard.NotNull(context, nameof(context));

            if (!context.EnforcementOrders.Any()) context.EnforcementOrders.AddRange(GetEnforcementOrders());
            context.SaveChanges();
        }
    }
}
