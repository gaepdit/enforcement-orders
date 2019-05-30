using Enfo.Models.Models;
using Xunit;

namespace Enfo.Models.Tests.Models
{
    public class AddressTests
    {
        [Fact]
        public void ComplexAddressToString_ReturnsCorrectString()
        {
            var address = new Address()
            {
                Street = "123 Any Street",
                Street2 = "Suite 404",
                City = "Atlanta",
                PostalCode = "10101"
            };

            var actual = address.ToString();
            string expected = "123 Any Street, Suite 404, Atlanta, GA 10101";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SimpleAddressToString_ReturnsCorrectString()
        {
            var address = new Address()
            {
                Street = "123 Any Street",
                City = "New York",
                State = "NY",
                PostalCode = "10101"
            };

            var actual = address.ToString();
            string expected = "123 Any Street, New York, NY 10101";

            Assert.Equal(expected, actual);
        }
    }
}