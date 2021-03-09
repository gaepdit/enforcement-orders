using Enfo.Repository.Validation;
using FluentAssertions;
using Xunit;

namespace Repository.Tests.ValidationTests
{
    public class UsingNonNegative
    {
        [Fact]
        public void PassesValidationGivenPositiveValue()
        {
            const decimal value = 20M;

            var validation = new NonNegativeAttribute();
            validation.IsValid(value).Should().BeTrue();
        }

        [Fact]
        public void PassesValidationGivenZeroValue()
        {
            const decimal value = 0M;

            var validation = new NonNegativeAttribute();
            validation.IsValid(value).Should().BeTrue();
        }

        [Fact]
        public void FailsValidationGivenNegativeValue()
        {
            const decimal value = -20M;

            var validation = new NonNegativeAttribute();
            validation.IsValid(value).Should().BeFalse();
        }
    }
}