using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using FluentAssertions;
using Xunit;

namespace Enfo.Domain.Tests
{
    public class CreateEntityResultTests
    {
        [Fact]
        public void CreateEntityResultEmpty()
        {
            var result = new CreateEntityResult<Address>();

            result.Success.Should().BeTrue();
            result.ErrorMessages.Should().BeEmpty();
        }

        [Fact]
        public void CreateEntityResultWithItem()
        {
            var item = new Address()
            {
                Street = "123 Any Street",
                Street2 = "Suite 404",
                City = "Atlanta",
                PostalCode = "10101"
            };

            var result = new CreateEntityResult<Address>(item);

            result.Success.Should().BeTrue();
            result.ErrorMessages.Should().BeEmpty();
            result.NewItem.Should().BeEquivalentTo(item);
        }

        [Fact]
        public void CreateEntityResultWithErrorMessage()
        {
            var key = "Name";
            var message = "Name is missing";
            var result = new CreateEntityResult<Address>(key, message);

            result.Success.Should().BeFalse();
            result.ErrorMessages.Should().HaveCount(1)
                .And.ContainKey(key);
            result.NewItem.Should().BeNull();
        }

        [Fact]
        public void AddErrorMessageToCreateEntityResult()
        {
            var item = new Address()
            {
                Street = "123 Any Street",
                Street2 = "Suite 404",
                City = "Atlanta",
                PostalCode = "10101"
            };

            var key = "Name";
            var message = "Name is missing";

            var result = new CreateEntityResult<Address>(item);
            result.AddErrorMessage(key, message);

            result.Success.Should().BeFalse();
            result.ErrorMessages.Should().HaveCount(1)
                .And.ContainKey(key);
            result.NewItem.Should().BeNull();
        }

        [Fact]
        public void AddItemToCreateEntityResult()
        {
            var item = new Address()
            {
                Street = "123 Any Street",
                Street2 = "Suite 404",
                City = "Atlanta",
                PostalCode = "10101"
            };

            var key = "Name";
            var message = "Name is missing";

            var result = new CreateEntityResult<Address>(key, message);
            result.AddItem(item);

            result.Success.Should().BeTrue();
            result.ErrorMessages.Should().HaveCount(0);
            result.NewItem.Should().BeEquivalentTo(item);
        }
    }
}
