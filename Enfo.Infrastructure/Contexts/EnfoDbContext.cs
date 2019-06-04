using Enfo.Infrastructure.SeedData;
using Enfo.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Enfo.Infrastructure.Contexts
{
    public class EnfoDbContext : DbContext
    {
        public EnfoDbContext(DbContextOptions<EnfoDbContext> options) : base(options) { }

        public DbSet<County> Counties { get; set; }
        public DbSet<LegalAuthority> LegalAuthorities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<County>().HasData(CountySeedData.GetCounties());
        }
    }
}
