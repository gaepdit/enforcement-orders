using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Enfo.Infrastructure.Contexts;

/// <summary>
/// Facilitates some EF Core Tools commands. See "Design-time DbContext Creation":
/// https://docs.microsoft.com/en-us/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli#from-a-design-time-factory
/// </summary>
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
