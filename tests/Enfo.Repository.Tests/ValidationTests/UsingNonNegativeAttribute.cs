using FluentAssertions;
using Xunit;

namespace Enfo.Repository.Tests.ValidationTests
{
    public class UsingNonNegativeAttribute
    {
        [Fact]
        public void PassesValidationGivenPositiveValue()
        {
            const decimal value = 20M;

            var validation = new Repository.Validation.NonNegativeAttribute();
            validation.IsValid(value).Should().BeTrue();
        }

        [Fact]
        public void PassesValidationGivenZeroValue()
        {
            const decimal value = 0M;

            var validation = new Repository.Validation.NonNegativeAttribute();
            validation.IsValid(value).Should().BeTrue();
        }

        [Fact]
        public void FailsValidationGivenNegativeValue()
        {
            const decimal value = -20M;

            var validation = new Repository.Validation.NonNegativeAttribute();
            validation.IsValid(value).Should().BeFalse();
        }
    }
}