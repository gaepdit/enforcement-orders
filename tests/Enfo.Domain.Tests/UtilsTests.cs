using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;
using static Enfo.Domain.Utils.DateUtils;
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

        public static IEnumerable<object[]> WeekdayTestData
        {
            get
            {
                return new List<object[]>
                {
                    new object[] { new DateTime(2019, 11, 18),
                        DayOfWeek.Monday, new DateTime(2019, 11, 18) },
                    new object[] { new DateTime(2019, 11, 18),
                        DayOfWeek.Tuesday, new DateTime(2019, 11, 19) },
                    new object[] { new DateTime(2019, 11, 18),
                        DayOfWeek.Sunday, new DateTime(2019, 11, 24) }
                };
            }
        }

        [Theory, MemberData(nameof(WeekdayTestData))]
        public void GetNextWeekdayWorks(DateTime start, DayOfWeek day, DateTime expected)
        {
            GetNextWeekday(start, day).Should().Be(expected);
        }
    }
}
