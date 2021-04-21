using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Enfo.Infrastructure.Contexts
{
    [UsedImplicitly]
    public class EnfoDbContextFactory : IDesignTimeDbContextFactory<EnfoDbContext>
    {
        public EnfoDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EnfoDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=enfo-temp;");
            return new EnfoDbContext(optionsBuilder.Options, null);
        }
    }
}
