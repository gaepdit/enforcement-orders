﻿using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using Enfo.Infrastructure.Contexts;
using Enfo.Infrastructure.Repositories;
using Enfo.Infrastructure.SeedData;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Enfo.Infrastructure.Tests.Helpers
{
    internal static class RepositoryHelpers
    {
        public static IAsyncWritableRepository<TEntity> GetRepository<TEntity>(this object callingClass, int appendToName = 0, [CallerMemberName] string dbName = null)
            where TEntity : BaseEntity
        {
            var options = new DbContextOptionsBuilder<EnfoDbContext>()
                .UseSqlite($"Data Source={callingClass}_{dbName}_{appendToName}_Test.db")
                .Options;

            var context = new EnfoDbContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.SeedTestData();

            return new WritableRepository<TEntity>(context);
        }
    }
}
