using Enfo.Domain.Attachments;
using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.Domain.EpdContacts.Entities;
using Enfo.Domain.LegalAuthorities.Entities;
using Enfo.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EnfoTests.TestData;

public static class SeedDataHelper
{
    private const string SqlServerProvider = "Microsoft.EntityFrameworkCore.SqlServer";

    public static void SeedAllData(DbContext context)
    {
        context.Set<ApplicationUser>().AddRange(UserData.GetUsers);
        context.Set<IdentityRole<Guid>>().AddRange(UserData.GetRoles);
        context.SaveChanges();

        if (context.Database.ProviderName == SqlServerProvider)
        {
            context.Database.BeginTransaction();
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT LegalAuthorities ON");
        }

        context.Set<LegalAuthority>().AddRange(LegalAuthorityData.LegalAuthorities);
        context.SaveChanges();

        if (context.Database.ProviderName == SqlServerProvider)
        {
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT LegalAuthorities OFF");
            context.Database.CommitTransaction();
        }

        if (context.Database.ProviderName == SqlServerProvider)
        {
            context.Database.BeginTransaction();
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT EpdContacts ON");
        }

        context.Set<EpdContact>().AddRange(EpdContactData.EpdContacts);
        context.SaveChanges();

        if (context.Database.ProviderName == SqlServerProvider)
        {
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT EpdContacts OFF");
            context.Database.CommitTransaction();
        }

        if (context.Database.ProviderName == SqlServerProvider)
        {
            context.Database.BeginTransaction();
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT EnforcementOrders ON");
        }

        context.Set<EnforcementOrder>().AddRange(EnforcementOrderData.EnforcementOrders);
        context.SaveChanges();

        if (context.Database.ProviderName == SqlServerProvider)
        {
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT EnforcementOrders OFF");
            context.Database.CommitTransaction();
        }

        context.Set<Attachment>().AddRange(AttachmentData.Attachments);
        context.SaveChanges();
    }

    public static async Task SeedAllDataAsync(DbContext context, CancellationToken token = default)
    {
        context.Set<ApplicationUser>().AddRange(UserData.GetUsers);
        context.Set<IdentityRole<Guid>>().AddRange(UserData.GetRoles);
        context.Set<LegalAuthority>().AddRange(LegalAuthorityData.LegalAuthorities);
        context.Set<EpdContact>().AddRange(EpdContactData.EpdContacts);
        context.Set<EnforcementOrder>().AddRange(EnforcementOrderData.EnforcementOrders);
        context.Set<Attachment>().AddRange(AttachmentData.Attachments);
        await context.SaveChangesAsync(token);
    }
}
