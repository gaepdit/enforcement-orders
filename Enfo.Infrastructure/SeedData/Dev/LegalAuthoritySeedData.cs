using Enfo.Domain.Entities;
using System.Collections.Generic;

namespace Enfo.Infrastructure.SeedData
{
    internal static partial class DevSeedData
    {
        public static LegalAuthority[] GetLegalAuthorities()
        {
            return new List<LegalAuthority>
            {
                new LegalAuthority { Id = 1, Active =  true, AuthorityName = "Air Quality Act", OrderNumberTemplate = "EPD-AQC-" },
                new LegalAuthority { Id = 2, Active =  true, AuthorityName = "Asbestos Safety Act" },
                new LegalAuthority { Id = 21, Active = false,AuthorityName = "Year 2000 Readiness Act" }
            }.ToArray();
        }
    }
}
