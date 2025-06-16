using Enfo.Domain.Attachments;
using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.Domain.EpdContacts.Entities;
using Enfo.Domain.LegalAuthorities.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnfoTests.TestData;

public static class SeedDataHelper
{
    public static async Task SeedAllDataAsync(DbContext context, CancellationToken cancellationToken = default)
    {
        context.Set<LegalAuthority>().AddRange(LegalAuthorityData.LegalAuthorities);
        context.Set<EpdContact>().AddRange(EpdContactData.EpdContacts);
        context.Set<Attachment>().AddRange(AttachmentData.Attachments);
        context.Set<EnforcementOrder>().AddRange(EnforcementOrderData.EnforcementOrders);
        await context.SaveChangesAsync(cancellationToken);
    }
}
