using Enfo.Repository.Validation;
using FluentAssertions;
using Xunit;

namespace Enfo.Repository.Tests.ValidationTests
{
    public class ConstructingValidateResourceResult
    {
        [Fact]
        public void ReturnsSuccessByDefault()
        {
            var result = new ValidateResourceResult();

            result.Success.Should().BeTrue();
            result.ErrorMessages.Should().BeEmpty();
        }

        [Fact]
        public void ReturnsFailedGivenErrorMessage()
        {
            const string key = "Name";
            const string message = "Name is missing";

            var result = new ValidateResourceResult();
            result.AddErrorMessage(key, message);

            result.Success.Should().BeFalse();
            result.ErrorMessages.Should().HaveCount(1)
                .And.ContainKey(key);
        }
    }
}