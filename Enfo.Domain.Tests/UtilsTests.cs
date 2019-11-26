using FluentAssertions;
using System;
using Xunit;
using static Enfo.Domain.Utils.StringUtils;

namespace Enfo.Domain.Tests
{
    public class UtilsTests
    {
        [Fact]
        public void ConcatEmptyStringsReturnsNull()
        {
            string[] emptyStringArray = Array.Empty<string>();

            string actual = ConcatNonEmptyStrings(emptyStringArray, ",");

            actual.Should().BeEmpty();
        }

        [Fact]
        public void ConcatNonEmptyStringArrayReturnsCorrectString()
        {
            var stringArray = new string[] { "abc", "def" };

            string actual = ConcatNonEmptyStrings(stringArray, ",");

            actual.Should().Be("abc,def");
        }

        [Fact]
        public void ConcatMixedArrayReturnsCorrectString()
        {
            var stringArray = new string[] { "abc", null, "def" };

            string actual = ConcatNonEmptyStrings(stringArray, ",");

            actual.Should().Be("abc,def");
        }
    }
}
