using Enfo.Repository.Repositories;
using FluentAssertions;
using Xunit;

namespace Enfo.Domain.Tests
{
    public class UpdateEntityResultTests
    {
        [Fact]
        public void UpdateEntityResultEmpty()
        {
            var result = new UpdateEntityResult();

            result.Success.Should().BeTrue();
            result.ErrorMessages.Should().BeEmpty();
        }

        [Fact]
        public void UpdateEntityResultWithErrorMessage()
        {
            var key = "Name";
            var message = "Name is missing";

            var result = new UpdateEntityResult(key, message);

            result.Success.Should().BeFalse();
            result.ErrorMessages.Should().HaveCount(1)
                .And.ContainKey(key);
        }

        [Fact]
        public void AddErrorMessageToUpdateEntityResult()
        {
            var key = "Name";
            var message = "Name is missing";

            var result = new UpdateEntityResult();
            result.AddErrorMessage(key, message);

            result.Success.Should().BeFalse();
            result.ErrorMessages.Should().HaveCount(1)
                .And.ContainKey(key);
        }
    }
}
