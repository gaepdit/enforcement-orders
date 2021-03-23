using Enfo.Domain.Entities;
using Enfo.Repository.Resources.Address;
using FluentAssertions;
using Xunit;

namespace Repository.Tests
{
    public class AddressToString
    {
        [Fact]
        public void ReturnsCorrectlyGivenSimpleAddress()
        {
            var address = new Address()
            {
                Street = "123 Any Street",
                City = "New York",
                State = "NY",
                PostalCode = "10101"
            };

            var addressView = new AddressView(address);

            addressView.ToString().Should().Be("123 Any Street, New York, NY 10101");
        }

        [Fact]
        public void ReturnsCorrectlyGivenComplexAddress()
        {
            var address = new Address()
            {
                Street = "123 Any Street",
                Street2 = "Suite 404",
                City = "Atlanta",
                PostalCode = "10101"
            };

            var addressView = new AddressView(address);

            addressView.ToString().Should().Be("123 Any Street, Suite 404, Atlanta, GA 10101");
        }

        [Fact]
        public void ReturnsCorrectlyGivenNullParts()
        {
            var address = new Address()
            {
                City = null,
                PostalCode = "ABC"
            };

            var addressView = new AddressView(address);

            addressView.ToString().Should().Be("GA ABC");
        }
    }
}