﻿using Enfo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using static Enfo.Infrastructure.SeedData.DevSeedData;
using static Enfo.Infrastructure.SeedData.SeedData;

namespace Enfo.Infrastructure.Contexts
{
    public class EnfoDbContext : DbContext
    {
        public EnfoDbContext(DbContextOptions<EnfoDbContext> options) : base(options) { }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<County> Counties { get; set; }
        public DbSet<Order> EnforcementOrders { get; set; }
        public DbSet<EpdContact> EpdContacts { get; set; }
        public DbSet<LegalAuthority> LegalAuthorities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<County>().HasData(GetCounties());
        }
    }
}
