using Enfo.Domain.Entities;
using Enfo.Infrastructure.Contexts;
using Enfo.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Enfo.Infrastructure.Tests.RepositoryTests
{
    class FakeRepository
    {
        public class Entity : BaseEntity
        {
            [Required]
            public string Name { get; set; }
        }

        public class ReadableRepository : ReadableRepository<Entity>
        {
            public ReadableRepository(EntityDbContext context) : base(context) { }
        }

        public class WritableRepository : WritableRepository<Entity>
        {
            public WritableRepository(EntityDbContext context) : base(context) { }
        }

        public class EntityDbContext : EnfoDbContext
        {
            public EntityDbContext(DbContextOptions<EnfoDbContext> options) : base(options) { }
            public DbSet<Entity> Entities { get; set; }
            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);

                builder.Entity<Entity>().HasData(GetSeedData());
            }

            private Entity[] GetSeedData()
            {
                return new List<Entity> {
                    new Entity { Id = 1, Name = "Apple" },
                    new Entity { Id = 2, Name = "Banana" , Active = false }
                }.ToArray();
            }
        }
    }
}
