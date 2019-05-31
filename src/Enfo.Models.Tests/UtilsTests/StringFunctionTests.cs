using System;
using Xunit;
using static Enfo.Models.Utils.StringFunctions;

namespace Enfo.Models.Tests.UtilsTests
{
    public class StringFunctionTests
    {
        [Fact]
        public void ConcatEmptyStringsReturnsNull()
        {
            string[] emptyStringArray = Array.Empty<string>();

            string actual = ConcatNonEmptyStrings(emptyStringArray, ",");

            Assert.Equal("", actual);
        }

        [Fact]
        public void ConcatNonEmptyStringArrayReturnsCorrectString()
        {
            var stringArray = new string[] { "abc", "def" };

            string actual = ConcatNonEmptyStrings(stringArray, ",");

            Assert.Equal("abc,def", actual);
        }

        [Fact]
        public void ConcatMixedArrayReturnsCorrectString()
        {
            var stringArray = new string[] { "abc", null, "def" };

            string actual = ConcatNonEmptyStrings(stringArray, ",");

            Assert.Equal("abc,def", actual);
        }
    }
}
