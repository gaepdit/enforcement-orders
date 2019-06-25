using Enfo.Domain.Entities;
using System.Collections.Generic;

namespace Enfo.Infrastructure.SeedData
{
    public static partial class DevSeedData
    {
        public static Address[] GetAddresses()
        {
            return new List<Address>
            {
                new Address { Id = 2000, Active = true, City = "Atlanta", PostalCode = "30354", State = "GA", Street = "4244 International Parkway" , Street2 = "Suite 120" },
                new Address { Id = 2001, Active = false, City = "Atlanta", PostalCode = "30354", State = "GA", Street = "4244 International Parkway" , Street2 = "Suite 104" },
                new Address { Id = 2002, Active = true, City = "Atlanta", PostalCode = "30334", State = "GA", Street = "2 Martin Luther King Jr. Drive SE" , Street2 = "Suite 1054 East" }
            }.ToArray();
        }
    }
}
