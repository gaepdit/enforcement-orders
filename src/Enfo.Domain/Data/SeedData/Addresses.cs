using System.Collections.Generic;
using Enfo.Domain.Entities;

namespace Enfo.Domain.Data
{
    public static partial class DomainData
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
            new Address
            {
                Id = 2002,
                Active = true,
                City = "Atlanta",
                PostalCode = "30334",
                State = "GA",
                Street = "2 Martin Luther King Jr. Drive SE",
                Street2 = "Suite 1054 East",
            },
            new Address
            {
                Id = 2003,
                Active = false,
                City = "Unknown",
                PostalCode = "00000",
                State = "GA",
                Street = "Unknown",
                Street2 = "",
            },
            new Address
            {
                Id = 2004,
                Active = true,
                City = "Atlanta",
                PostalCode = "30334",
                State = "GA",
                Street = "2 Martin Luther King Jr. Drive SE",
                Street2 = "Suite 1152 East",
            },
            new Address
            {
                Id = 2007,
                Active = false,
                City = "Albany",
                PostalCode = "31701-3576",
                State = "GA",
                Street = "2024 Newton Road",
                Street2 = "",
            },
            new Address
            {
                Id = 2008,
                Active = false,
                City = "Brunswick",
                PostalCode = "31523",
                State = "GA",
                Street = "400 Commerce Center Drive",
                Street2 = "",
            },
            new Address
            {
                Id = 2009,
                Active = false,
                City = "Macon",
                PostalCode = "31211",
                State = "GA",
                Street = "2640 Shurling Drive",
                Street2 = "",
            },
            new Address
            {
                Id = 2010,
                Active = false,
                City = "Cartersville",
                PostalCode = "30120",
                State = "GA",
                Street = "Post Office Box 3250",
                Street2 = "",
            },
            new Address
            {
                Id = 2011,
                Active = false,
                City = "Athens",
                PostalCode = "30605",
                State = "GA",
                Street = "745 Gaines School Road",
                Street2 = "",
            },
            new Address
            {
                Id = 2012,
                Active = false,
                City = "Augusta",
                PostalCode = "30906",
                State = "GA",
                Street = "1885 Tobacco Road",
                Street2 = "",
            },
            new Address
            {
                Id = 2013,
                Active = false,
                City = "Cartersville",
                PostalCode = "30120",
                State = "GA",
                Street = "Post Office Box 3250",
                Street2 = "",
            },
            new Address
            {
                Id = 2014,
                Active = false,
                City = "Augusta",
                PostalCode = "30909",
                State = "GA",
                Street = "3525 Walton Way Ext.",
                Street2 = "",
            },
            new Address
            {
                Id = 2015,
                Active = true,
                City = "Atlanta",
                PostalCode = "30354-3906",
                State = "GA",
                Street = "4244 International Pkwy",
                Street2 = "Suite 120",
            },
            new Address
            {
                Id = 2016,
                Active = true,
                City = "Atlanta",
                PostalCode = "30334",
                State = "GA",
                Street = "2 Martin Luther King Jr. Drive SE",
                Street2 = "Suite 1456 East",
            }
        };
    }
}
