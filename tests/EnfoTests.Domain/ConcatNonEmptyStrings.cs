using System;
using Enfo.Domain.Utils;
using FluentAssertions;
using Xunit;

namespace EnfoTests.Domain
{
    public class ConcatNonEmptyStrings
    {
        [Fact]
        public void ReturnsCorrectlyGivenNonEmptyStrings()
        {
            var stringArray = new[] {"abc", "def"};
            stringArray.ConcatNonEmptyStrings(",").Should().Be("abc,def");
        }

        [Fact]
        public void ReturnsNullGivenEmptyArray()
        {
            var emptyStringArray = Array.Empty<string>();
            emptyStringArray.ConcatNonEmptyStrings(",").Should().BeEmpty();
        }

        [Fact]
        public void ReturnsCorrectlyGivenNullItem()
        {
            var stringArray = new[] {"abc", null, "def"};
            stringArray.ConcatNonEmptyStrings(",").Should().Be("abc,def");
        }

        [Fact]
        public void ReturnsCorrectlyGivenEmptyString()
        {
            var stringArray = new[] {"abc", "", "def"};
            stringArray.ConcatNonEmptyStrings(",").Should().Be("abc,def");
        }
    }
}