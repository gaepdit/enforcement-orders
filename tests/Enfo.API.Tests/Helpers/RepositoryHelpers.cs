using Enfo.Domain.Entities;
using Enfo.Repository.Repositories;
using Enfo.Infrastructure.Contexts;
using Enfo.Infrastructure.Repositories;
using Enfo.Infrastructure.SeedData;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Enfo.API.Tests.Helpers
{
    internal static class RepositoryHelpers
    {
        public static IWritableRepository<T> GetRepository<T>(
            this object callingClass,
            int appendToName = 0,
            [CallerMemberName] string dbName = null)
            where T : BaseEntity
        {
            var options = new DbContextOptionsBuilder<EnfoDbContext>()
                .UseSqlite($"Data Source={callingClass}_{dbName}_{appendToName}_Test.db")
                .Options;

            var context = new EnfoDbContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.SeedTestData();

            return new WritableRepository<T>(context);
        }

        public static IEnforcementOrderRepository GetEnforcementOrderRepository(
            this object callingClass,
            int appendToName = 0,
            [CallerMemberName] string dbName = null)
        {
            var options = new DbContextOptionsBuilder<EnfoDbContext>()
                .UseSqlite($"Data Source={callingClass}_{dbName}_{appendToName}_Test.db")
                .Options;

            var context = new EnfoDbContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.SeedTestData();

            return new EnforcementOrderRepository(context);
        }
    }
}
