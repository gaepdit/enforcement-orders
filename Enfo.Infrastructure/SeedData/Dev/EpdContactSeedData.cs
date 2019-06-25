using Enfo.Domain.Entities;
using System.Collections.Generic;

namespace Enfo.Infrastructure.SeedData
{
    public static partial class DevSeedData
    {
        public static EpdContact[] GetEpdContacts()
        {
            return new List<EpdContact>
            {
                new EpdContact { Id = 2000, Active = false, AddressId = 2000, ContactName = "Mr. Keith M. Bentley", Email = "null", Organization = "Environmental Protection Division", Title = "Chief, Air Protection Branch" },
                new EpdContact { Id = 2001, Active = false, AddressId = 2001, ContactName = "Mr. Jeff Cown", Email = "null", Organization = "Environmental Protection Division", Title = "Chief, Land Protection Branch" },
                new EpdContact { Id = 2002, Active = true, AddressId = 2002, ContactName = "Mr. Chuck Mueller", Email = "Chuck.mueller@dnr.ga.gov", Organization = "Environmental Protection Division", Title = "Chief, Land Protection Branch" }
            }.ToArray();
        }
    }
}
