using Enfo.Infrastructure.Contexts;

namespace Enfo.Infrastructure.SeedData
{
    public static partial class DevSeedData
    {
        public static void SeedTestData(this EnfoDbContext context)
        {
            context.SaveChanges();
        }
    }
}
