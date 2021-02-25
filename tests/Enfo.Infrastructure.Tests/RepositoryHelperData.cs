using System.Collections.Generic;
using Enfo.Domain.Entities;

namespace Enfo.Infrastructure.Tests
{
    public static class RepositoryHelperData
    {
        public static IEnumerable<Address> GetAddresses() => new List<Address>
        {
            new Address
            {
                Id = 2000,
                Active = true,
                City = "Atlanta",
                PostalCode = "30354",
                State = "GA",
                Street = "4244 International Parkway",
                Street2 = "Suite 120",
            },
            new Address
            {
                Id = 2001,
                Active = false,
                City = "Atlanta",
                PostalCode = "30354",
                State = "GA",
                Street = "4244 International Parkway",
                Street2 = "Suite 104",
            },
        };

        public static IEnumerable<LegalAuthority> GetLegalAuthorities() => new List<LegalAuthority>
        {
            new LegalAuthority {Id = 1, Active = true, AuthorityName = "Air Quality Act",},
            new LegalAuthority {Id = 2, Active = true, AuthorityName = "Asbestos Safety Act",},
        };

        public static IEnumerable<EpdContact> GetEpdContacts() => new List<EpdContact>
        {
            new EpdContact
            {
                Id = 2000,
                Active = true,
                AddressId = 2000,
                ContactName = "A. Jones",
                Email = null,
                Organization = "Environmental Protection Division",
                Telephone = null,
                Title = "Chief, Air Protection Branch",
            },
            new EpdContact
            {
                Id = 2001,
                Active = false,
                AddressId = 2001,
                ContactName = "B. Smith",
                Email = null,
                Organization = "Environmental Protection Division",
                Telephone = null,
                Title = "Chief, Land Protection Branch",
            },
        };
    }
}