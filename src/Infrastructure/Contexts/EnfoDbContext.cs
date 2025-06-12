using Enfo.Domain.BaseEntities;
using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.Domain.EpdContacts.Entities;
using Enfo.Domain.LegalAuthorities.Entities;
using Enfo.Domain.Users;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Enfo.Infrastructure.Contexts;

public class EnfoDbContext(DbContextOptions<EnfoDbContext> options, IHttpContextAccessor httpContextAccessor)
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<EnforcementOrder> EnforcementOrders { get; [UsedImplicitly] set; }
    public DbSet<Attachment> Attachments { get; [UsedImplicitly] set; }
    public DbSet<EpdContact> EpdContacts { get; [UsedImplicitly] set; }
    public DbSet<LegalAuthority> LegalAuthorities { get; [UsedImplicitly] set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder ?? throw new ArgumentNullException(nameof(builder)));

        // ASP.NET Identity Tables
        builder.Entity<ApplicationUser>().ToTable("AppUsers");
        builder.Entity<IdentityRole<Guid>>().ToTable("AppRoles");
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins");
        builder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens");

        // Add audit properties to auditable entities
        foreach (var entityType in builder.Model.GetEntityTypes()
                     .Where(e => typeof(IAuditable).IsAssignableFrom(e.ClrType)).Select(e => e.ClrType))
        {
            builder.Entity(entityType).Property<DateTimeOffset?>(AuditProperties.CreatedAt);
            builder.Entity(entityType).Property<DateTimeOffset?>(AuditProperties.UpdatedAt);
            builder.Entity(entityType).Property<string>(AuditProperties.CreatedBy);
            builder.Entity(entityType).Property<string>(AuditProperties.UpdatedBy);
        }

        // Performance-related indexes
        // See https://learn.microsoft.com/en-us/ef/core/modeling/indexes?tabs=fluent-api#included-columns
        // and https://github.com/gaepdit/EPDDatabases/blob/16e12d19ae063a8df96226f55ba78107170bd86e/Troubleshooting_Scripts/ImprovePerformance/Microsoft/EnforcementOrders.sql 
        var orders = builder.Entity<EnforcementOrder>();
        orders.HasIndex(e => new { e.Deleted, e.OrderNumber }, "missing_index_345_344");
        orders.HasIndex(e => new { e.Deleted }, "missing_index_561_560")
            .IncludeProperties(e => new { e.OrderNumber });
        orders.HasIndex(e => new { e.Deleted, e.OrderNumber, e.Id }, "missing_index_617_616");
        orders.HasIndex(e => new { e.Deleted, e.PublicationStatus }, "missing_index_699_698")
            .IncludeProperties(e => new
            {
                e.ExecutedOrderPostedDate,
                e.IsExecutedOrder,
                e.IsProposedOrder,
                e.OrderNumber,
                e.ProposedOrderPostedDate,
            });
        orders.HasIndex(e => new { e.Deleted, e.LegalAuthorityId, e.PublicationStatus }, "missing_index_737_736")
            .IncludeProperties(e => new
            {
                e.ExecutedOrderPostedDate, e.IsExecutedOrder, e.IsProposedOrder, e.ProposedOrderPostedDate
            });
        orders.HasIndex(e => new { e.Deleted, e.PublicationStatus }, "missing_index_741_740")
            .IncludeProperties(e => new
            {
                e.ExecutedOrderPostedDate, e.IsExecutedOrder, e.IsProposedOrder, e.ProposedOrderPostedDate
            });
        orders.HasIndex(e => new { e.Deleted, e.LegalAuthorityId, e.PublicationStatus, e.County },
                "missing_index_898_897")
            .IncludeProperties(e => new
            {
                e.ExecutedOrderPostedDate, e.IsExecutedOrder, e.IsProposedOrder, e.ProposedOrderPostedDate
            });
        orders.HasIndex(e => new { e.Deleted, e.LegalAuthorityId, e.PublicationStatus }, "missing_index_1099_1098")
            .IncludeProperties(e => new
            {
                e.ExecutedDate,
                e.ExecutedOrderPostedDate,
                e.IsExecutedOrder,
                e.IsProposedOrder,
                e.ProposedOrderPostedDate,
            });
        orders.HasIndex(e => new { e.Deleted, e.LegalAuthorityId, e.PublicationStatus }, "missing_index_1102_1101")
            .IncludeProperties(e => new
            {
                e.ExecutedDate,
                e.ExecutedOrderPostedDate,
                e.FacilityName,
                e.IsExecutedOrder,
                e.IsProposedOrder,
                e.ProposedOrderPostedDate,
            });
        orders.HasIndex(e => new { e.Deleted, e.PublicationStatus, e.County }, "missing_index_1176_1175")
            .IncludeProperties(e => new
            {
                e.ExecutedDate,
                e.ExecutedOrderPostedDate,
                e.IsExecutedOrder,
                e.IsProposedOrder,
                e.ProposedOrderPostedDate,
            });
        orders.HasIndex(e => new { e.Deleted, e.PublicationStatus, e.County }, "missing_index_1271_1270")
            .IncludeProperties(e => new
            {
                e.ExecutedOrderPostedDate, e.IsExecutedOrder, e.IsProposedOrder, e.ProposedOrderPostedDate
            });
    }

    public override int SaveChanges()
    {
        SetAuditProperties();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        SetAuditProperties();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetAuditProperties()
    {
        var currentUser = httpContextAccessor?.HttpContext?.User.Identity?.Name;

        var entries = ChangeTracker.Entries()
            .Where(e => (e.State is EntityState.Added or EntityState.Modified) && e.Entity is IAuditable);

        foreach (var entry in entries)
        {
            entry.Property(AuditProperties.UpdatedAt).CurrentValue = DateTimeOffset.Now;
            entry.Property(AuditProperties.UpdatedBy).CurrentValue = currentUser;
            if (entry.State == EntityState.Modified) continue;
            entry.Property(AuditProperties.CreatedAt).CurrentValue = DateTimeOffset.Now;
            entry.Property(AuditProperties.CreatedBy).CurrentValue = currentUser;
        }
    }

    private static class AuditProperties
    {
        internal const string CreatedAt = nameof(CreatedAt);
        internal const string UpdatedAt = nameof(UpdatedAt);
        internal const string CreatedBy = nameof(CreatedBy);
        internal const string UpdatedBy = nameof(UpdatedBy);
    }
}
