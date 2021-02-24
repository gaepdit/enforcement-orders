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
    }
}