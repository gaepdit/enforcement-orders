using FluentAssertions;
using System;
using Xunit;
using static Enfo.Domain.Utils.StringFunctions;

namespace Enfo.Domain.Tests.UtilsTests
{
    public class StringFunctionTests
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
