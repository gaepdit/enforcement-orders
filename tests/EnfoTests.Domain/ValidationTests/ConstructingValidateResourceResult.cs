using Enfo.Domain.Validation;
using FluentAssertions;
using Xunit;

namespace EnfoTests.Domain.ValidationTests
{
    public class ConstructingValidateResourceResult
    {
        [Fact]
        public void ReturnsSuccessByDefault()
        {
            var result = new ResourceValidationResult();

            result.IsValid.Should().BeTrue();
            result.ErrorMessages.Should().BeEmpty();
        }

        [Fact]
        public void ReturnsFailedGivenErrorMessage()
        {
            const string key = "Name";
            const string message = "Name is missing";

            var result = new ResourceValidationResult();
            result.AddErrorMessage(key, message);

            result.IsValid.Should().BeFalse();
            result.ErrorMessages.Should().HaveCount(1)
                .And.ContainKey(key);
        }
    }
}