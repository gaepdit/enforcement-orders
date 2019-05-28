using System;
using Xunit;
using static Enfo.Models.Utils.StringFunctions;

namespace Enfo.Models.Tests.Utils
{
    public class StringFunctionTests
    {
        [Fact]
        public void ConcatNonEmptyStrings_Empty_ReturnsNull()
        {
            string[] emptyStringArray = Array.Empty<string>();

            string actual = ConcatNonEmptyStrings(emptyStringArray, ",");

            Assert.Equal("", actual);
        }

        [Fact]
        public void ConcatNonEmptyString_NonemptyArray_ReturnsCorrectString()
        {
            var stringArray = new string[] { "abc", "def" };

            string actual = ConcatNonEmptyStrings(stringArray, ",");

            Assert.Equal("abc,def", actual);
        }

        [Fact]
        public void ConcatNonEmptyString_MixedArray_ReturnsCorrectString()
        {
            var stringArray = new string[] { "abc", null, "def" };

            string actual = ConcatNonEmptyStrings(stringArray, ",");

            Assert.Equal("abc,def", actual);
        }
    }
}
