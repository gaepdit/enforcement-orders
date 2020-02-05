using System;
using System.Collections.Generic;
using Enfo.Domain.Entities;

namespace Enfo.Infrastructure.SeedData
{
    public static partial class ProdSeedData
    {
        public static EpdContact[] GetEpdContacts()
        {
            return new List<EpdContact>
            {
                new EpdContact {
                    Id = 2000,
                    Active = false,
                    AddressId = 2000,
                    ContactName = "Mr. Keith M. Bentley",
                    CreatedById = new Guid("CECDB2C3-101C-45EF-2F05-08D4881DF634"),
                    CreatedDate = new DateTime(2017,04,27,21,13,07),
                    Email = null,
                    Organization = "Environmental Protection Division",
                    Telephone = null,
                    Title = "Chief, Air Protection Branch",
                    UpdatedById = new Guid("C076CDA6-8344-4BDE-8A3A-2C96DC4DE932"),
                    UpdatedDate = new DateTime(2017,05,03,14,33,18)
                },
                new EpdContact {
                    Id = 2001,
                    Active = false,
                    AddressId = 2001,
                    ContactName = "Mr. Jeff Cown",
                    CreatedById = new Guid("CECDB2C3-101C-45EF-2F05-08D4881DF634"),
                    CreatedDate = new DateTime(2017,04,27,21,13,07),
                    Email = null,
                    Organization = "Environmental Protection Division",
                    Telephone = null,
                    Title = "Chief, Land Protection Branch",
                    UpdatedById = new Guid("C076CDA6-8344-4BDE-8A3A-2C96DC4DE932"),
                    UpdatedDate = new DateTime(2017,05,03,14,34,01)
                },
                new EpdContact {
                    Id = 2002,
                    Active = true,
                    AddressId = 2002,
                    ContactName = "Mr. Chuck Mueller",
                    CreatedById = new Guid("CECDB2C3-101C-45EF-2F05-08D4881DF634"),
                    CreatedDate = new DateTime(2017,04,27,21,13,07),
                    Email = "Chuck.mueller@dnr.ga.gov",
                    Organization = "Environmental Protection Division",
                    Telephone = null,
                    Title = "Chief, Land Protection Branch",
                    UpdatedById = new Guid("C076CDA6-8344-4BDE-8A3A-2C96DC4DE932"),
                    UpdatedDate = new DateTime(2018,09,28,13,02,35)
                },
                new EpdContact {
                    Id = 2003,
                    Active = false,
                    AddressId = 2003,
                    ContactName = "Unknown",
                    CreatedById = new Guid("CECDB2C3-101C-45EF-2F05-08D4881DF634"),
                    CreatedDate = new DateTime(2017,04,27,21,13,07),
                    Email = null,
                    Organization = "None",
                    Telephone = null,
                    Title = "None",
                    UpdatedById = new Guid("CECDB2C3-101C-45EF-2F05-08D4881DF634"),
                    UpdatedDate = new DateTime(2017,04,28,01,25,05)
                },
                new EpdContact {
                    Id = 2004,
                    Active = true,
                    AddressId = 2004,
                    ContactName = "Mr. James A. Capp",
                    CreatedById = new Guid("CECDB2C3-101C-45EF-2F05-08D4881DF634"),
                    CreatedDate = new DateTime(2017,04,27,21,13,07),
                    Email = "James.Capp@dnr.ga.gov",
                    Organization = "Environmental Protection Division",
                    Telephone = null,
                    Title = "Chief, Watershed Protection Branch",
                    UpdatedById = new Guid("C076CDA6-8344-4BDE-8A3A-2C96DC4DE932"),
                    UpdatedDate = new DateTime(2017,05,03,14,50,40)
                },
                new EpdContact {
                    Id = 2007,
                    Active = false,
                    AddressId = 2007,
                    ContactName = "Ms. Mary Sheffield",
                    CreatedById = new Guid("CECDB2C3-101C-45EF-2F05-08D4881DF634"),
                    CreatedDate = new DateTime(2017,04,27,21,13,07),
                    Email = null,
                    Organization = "Environmental Protection Division",
                    Telephone = null,
                    Title = "Manager, Southwest District",
                    UpdatedById = new Guid("C076CDA6-8344-4BDE-8A3A-2C96DC4DE932"),
                    UpdatedDate = new DateTime(2017,05,03,14,34,15)
                },
                new EpdContact {
                    Id = 2008,
                    Active = false,
                    AddressId = 2008,
                    ContactName = "Mr. Bruce Foisy",
                    CreatedById = new Guid("CECDB2C3-101C-45EF-2F05-08D4881DF634"),
                    CreatedDate = new DateTime(2017,04,27,21,13,07),
                    Email = null,
                    Organization = "Environmental Protection Division",
                    Telephone = null,
                    Title = "Manager, Coastal District Office",
                    UpdatedById = new Guid("C076CDA6-8344-4BDE-8A3A-2C96DC4DE932"),
                    UpdatedDate = new DateTime(2017,05,03,14,34,22)
                },
                new EpdContact {
                    Id = 2009,
                    Active = false,
                    AddressId = 2009,
                    ContactName = "Mr. Todd Bethune",
                    CreatedById = new Guid("CECDB2C3-101C-45EF-2F05-08D4881DF634"),
                    CreatedDate = new DateTime(2017,04,27,21,13,07),
                    Email = null,
                    Organization = "Environmental Protection Division",
                    Telephone = null,
                    Title = "Manager, West Central District",
                    UpdatedById = new Guid("C076CDA6-8344-4BDE-8A3A-2C96DC4DE932"),
                    UpdatedDate = new DateTime(2017,05,03,14,34,29)
                },
                new EpdContact {
                    Id = 2010,
                    Active = false,
                    AddressId = 2016,
                    ContactName = "Dr. Bert Langley",
                    CreatedById = new Guid("CECDB2C3-101C-45EF-2F05-08D4881DF634"),
                    CreatedDate = new DateTime(2017,04,27,21,13,07),
                    Email = "Bert.Langley@dnr.ga.gov",
                    Organization = "Environmental Protection Division",
                    Telephone = null,
                    Title = "Director Of Compliance",
                    UpdatedById = new Guid("CECDB2C3-101C-45EF-2F05-08D4881DF634"),
                    UpdatedDate = new DateTime(2018,03,06,08,55,01)
                },
                new EpdContact {
                    Id = 2011,
                    Active = false,
                    AddressId = 2011,
                    ContactName = "Mr. Mike Rodock",
                    CreatedById = new Guid("CECDB2C3-101C-45EF-2F05-08D4881DF634"),
                    CreatedDate = new DateTime(2017,04,27,21,13,07),
                    Email = null,
                    Organization = "Environmental Protection Division",
                    Telephone = null,
                    Title = "Manager, Northeast District",
                    UpdatedById = new Guid("C076CDA6-8344-4BDE-8A3A-2C96DC4DE932"),
                    UpdatedDate = new DateTime(2017,05,03,14,34,36)
                },
                new EpdContact {
                    Id = 2012,
                    Active = false,
                    AddressId = 2012,
                    ContactName = "Mr. Don McCarty",
                    CreatedById = new Guid("CECDB2C3-101C-45EF-2F05-08D4881DF634"),
                    CreatedDate = new DateTime(2017,04,27,21,13,07),
                    Email = null,
                    Organization = "Environmental Protection Division",
                    Telephone = null,
                    Title = "Manager, East Central District",
                    UpdatedById = new Guid("C076CDA6-8344-4BDE-8A3A-2C96DC4DE932"),
                    UpdatedDate = new DateTime(2017,05,03,14,34,47)
                },
                new EpdContact {
                    Id = 2013,
                    Active = true,
                    AddressId = 2016,
                    ContactName = "Mr. James Cooley",
                    CreatedById = new Guid("CECDB2C3-101C-45EF-2F05-08D4881DF634"),
                    CreatedDate = new DateTime(2017,04,27,21,13,07),
                    Email = "james.cooley@dnr.ga.gov",
                    Organization = "Environmental Protection Division",
                    Telephone = "(770) 387-4929",
                    Title = "Director of District Operations",
                    UpdatedById = new Guid("CECDB2C3-101C-45EF-2F05-08D4881DF634"),
                    UpdatedDate = new DateTime(2018,03,06,08,57,35)
                },
                new EpdContact {
                    Id = 2014,
                    Active = false,
                    AddressId = 2014,
                    ContactName = "Mr. Jeff Darley",
                    CreatedById = new Guid("CECDB2C3-101C-45EF-2F05-08D4881DF634"),
                    CreatedDate = new DateTime(2017,04,27,21,13,07),
                    Email = null,
                    Organization = "Environmental Protection Division",
                    Telephone = null,
                    Title = "Manager, East Central District",
                    UpdatedById = new Guid("C076CDA6-8344-4BDE-8A3A-2C96DC4DE932"),
                    UpdatedDate = new DateTime(2017,05,03,14,34,57)
                },
                new EpdContact {
                    Id = 2015,
                    Active = true,
                    AddressId = 2015,
                    ContactName = "Ms. Karen Hays",
                    CreatedById = new Guid("CECDB2C3-101C-45EF-2F05-08D4881DF634"),
                    CreatedDate = new DateTime(2017,04,27,21,13,07),
                    Email = "Karen.Hays@dnr.ga.gov",
                    Organization = "Environmental Protection Division",
                    Telephone = null,
                    Title = "Chief, Air Protection Branch",
                    UpdatedById = new Guid("C076CDA6-8344-4BDE-8A3A-2C96DC4DE932"),
                    UpdatedDate = new DateTime(2017,05,03,14,49,34)
                }
            }.ToArray();
        }
    }
}
