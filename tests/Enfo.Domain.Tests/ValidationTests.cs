using Enfo.Domain.Validation;
using FluentAssertions;
using Xunit;

namespace Enfo.Domain.Tests
{
    public class ValidationTests
    {
        [Fact]
        public void NegativeValueFailsValidation()
        {
            var value = -20M;

            var validation = new NonNegativeAttribute();
            var isValid = validation.IsValid(value);

            isValid.Should().BeFalse();
        }
    }
}
