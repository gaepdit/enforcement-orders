﻿using System;
using Enfo.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Enfo.Infrastructure.Contexts
{
    public class EnfoDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public EnfoDbContext(DbContextOptions<EnfoDbContext> options) : base(options) { }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<EnforcementOrder> EnforcementOrders { get; set; }
        public DbSet<EpdContact> EpdContacts { get; set; }
        public DbSet<LegalAuthority> LegalAuthorities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder ?? throw new ArgumentNullException(nameof(builder)));

            // Unique indexes
            // builder.Entity<EnforcementOrder>().HasIndex(b => b.OrderNumber).IsUnique();

            // ASP.NET Identity Tables
            builder.Entity<ApplicationUser>().ToTable("AppUsers");
            builder.Entity<IdentityRole<Guid>>().ToTable("AppRoles");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens");
        }
    }
}
