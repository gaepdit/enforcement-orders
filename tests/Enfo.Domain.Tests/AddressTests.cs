using Enfo.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Enfo.Domain.Tests
{
    public class AddressTests
    {
        [Fact]
        public void ComplexAddressToStringReturnsCorrectString()
        {
            var address = new Address()
            {
                Street = "123 Any Street",
                Street2 = "Suite 404",
                City = "Atlanta",
                PostalCode = "10101"
            };

            var actual = address.ToString();

            actual.Should().Be("123 Any Street, Suite 404, Atlanta, GA 10101");
        }

        [Fact]
        public void SimpleAddressToStringReturnsCorrectString()
        {
            var address = new Address()
            {
                Street = "123 Any Street",
                City = "New York",
                State = "NY",
                PostalCode = "10101"
            };

            var actual = address.ToString();
            actual.Should().Be("123 Any Street, New York, NY 10101");
        }

        [Fact]
        public void AddressWithNullParts()
        {
            var address = new Address()
            {
                City = null,
                PostalCode = "ABC"
            };

            var actual = address.ToString();
            actual.Should().Be("GA ABC");
        }
    }
}
