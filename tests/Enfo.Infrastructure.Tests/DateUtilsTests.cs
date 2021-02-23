using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;
using static Enfo.Repository.Utils.DateUtils;
using static Enfo.Domain.Utils.StringUtils;

namespace Enfo.Infrastructure.Tests
{
    public class DateUtilsTests
    {
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
