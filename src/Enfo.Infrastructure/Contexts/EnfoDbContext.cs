using Enfo.Domain.Entities;
using Enfo.Infrastructure.SeedData;
using Microsoft.EntityFrameworkCore;
using System;

namespace Enfo.Infrastructure.Contexts
{
    public class EnfoDbContext : DbContext
    {
        public EnfoDbContext(DbContextOptions<EnfoDbContext> options) : base(options) { }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<County> Counties { get; set; }
        public DbSet<EnforcementOrder> EnforcementOrders { get; set; }
        public DbSet<EpdContact> EpdContacts { get; set; }
        public DbSet<LegalAuthority> LegalAuthorities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder ?? throw new ArgumentNullException(nameof(builder)));

            builder.Entity<County>().HasData(ProdSeedData.GetCounties());
            builder.Entity<LegalAuthority>().HasData(ProdSeedData.GetLegalAuthorities());
            builder.Entity<Address>().HasData(ProdSeedData.GetAddresses());
            builder.Entity<EpdContact>().HasData(ProdSeedData.GetEpdContacts());

            builder.Entity<EnforcementOrder>()
                .HasIndex(b => b.OrderNumber).IsUnique();
        }
    }
}
