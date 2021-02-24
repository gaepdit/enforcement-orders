using System.Collections.Generic;
using Enfo.Domain.Entities;

namespace Enfo.Domain.Data
{
    public static partial class DomainData
    {
        public static IEnumerable<EpdContact> GetEpdContacts() => new List<EpdContact>
        {
            new EpdContact
            {
                Id = 2000,
                Active = false,
                AddressId = 2000,
                ContactName = "Mr. Keith M. Bentley",
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
                ContactName = "Mr. Jeff Cown",
                Email = null,
                Organization = "Environmental Protection Division",
                Telephone = null,
                Title = "Chief, Land Protection Branch",
            },
            new EpdContact
            {
                Id = 2002,
                Active = true,
                AddressId = 2002,
                ContactName = "Mr. Chuck Mueller",
                Email = "Chuck.mueller@dnr.ga.gov",
                Organization = "Environmental Protection Division",
                Telephone = null,
                Title = "Chief, Land Protection Branch",
            },
            new EpdContact
            {
                Id = 2003,
                Active = false,
                AddressId = 2003,
                ContactName = "Unknown",
                Email = null,
                Organization = "None",
                Telephone = null,
                Title = "None",
            },
            new EpdContact
            {
                Id = 2004,
                Active = true,
                AddressId = 2004,
                ContactName = "Mr. James A. Capp",
                Email = "James.Capp@dnr.ga.gov",
                Organization = "Environmental Protection Division",
                Telephone = null,
                Title = "Chief, Watershed Protection Branch",
            },
            new EpdContact
            {
                Id = 2007,
                Active = false,
                AddressId = 2007,
                ContactName = "Ms. Mary Sheffield",
                Email = null,
                Organization = "Environmental Protection Division",
                Telephone = null,
                Title = "Manager, Southwest District",
            },
            new EpdContact
            {
                Id = 2008,
                Active = false,
                AddressId = 2008,
                ContactName = "Mr. Bruce Foisy",
                Email = null,
                Organization = "Environmental Protection Division",
                Telephone = null,
                Title = "Manager, Coastal District Office",
            },
            new EpdContact
            {
                Id = 2009,
                Active = false,
                AddressId = 2009,
                ContactName = "Mr. Todd Bethune",
                Email = null,
                Organization = "Environmental Protection Division",
                Telephone = null,
                Title = "Manager, West Central District",
            },
            new EpdContact
            {
                Id = 2010,
                Active = false,
                AddressId = 2016,
                ContactName = "Dr. Bert Langley",
                Email = "Bert.Langley@dnr.ga.gov",
                Organization = "Environmental Protection Division",
                Telephone = null,
                Title = "Director Of Compliance",
            },
            new EpdContact
            {
                Id = 2011,
                Active = false,
                AddressId = 2011,
                ContactName = "Mr. Mike Rodock",
                Email = null,
                Organization = "Environmental Protection Division",
                Telephone = null,
                Title = "Manager, Northeast District",
            },
            new EpdContact
            {
                Id = 2012,
                Active = false,
                AddressId = 2012,
                ContactName = "Mr. Don McCarty",
                Email = null,
                Organization = "Environmental Protection Division",
                Telephone = null,
                Title = "Manager, East Central District",
            },
            new EpdContact
            {
                Id = 2013,
                Active = true,
                AddressId = 2016,
                ContactName = "Mr. James Cooley",
                Email = "james.cooley@dnr.ga.gov",
                Organization = "Environmental Protection Division",
                Telephone = "(770) 387-4929",
                Title = "Director of District Operations",
            },
            new EpdContact
            {
                Id = 2014,
                Active = false,
                AddressId = 2014,
                ContactName = "Mr. Jeff Darley",
                Email = null,
                Organization = "Environmental Protection Division",
                Telephone = null,
                Title = "Manager, East Central District",
            },
            new EpdContact
            {
                Id = 2015,
                Active = true,
                AddressId = 2015,
                ContactName = "Ms. Karen Hays",
                Email = "Karen.Hays@dnr.ga.gov",
                Organization = "Environmental Protection Division",
                Telephone = null,
                Title = "Chief, Air Protection Branch",
            }
        };
    }
}
