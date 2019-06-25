﻿using Enfo.Infrastructure.Contexts;

namespace Enfo.Infrastructure.SeedData
{
    public static partial class DevSeedData
    {
        public static void SeedTestData(this EnfoDbContext context)
        {
            context.LegalAuthorities.AddRange(GetLegalAuthorities());
            context.Addresses.AddRange(GetAddresses());
            context.EpdContacts.AddRange(GetEpdContacts());
            context.SaveChanges();
        }
    }
}
