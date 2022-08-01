using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using static Enfo.Domain.Utils.DateUtils;

namespace EnfoTests.Domain.Utils;

[TestFixture]
public class DateUtilsGetNextWeekday
{
    public static IEnumerable<object[]> WeekdayTestData => new List<object[]>
    {
        new object[] { new DateTime(2019, 11, 18), DayOfWeek.Monday, new DateTime(2019, 11, 18) },
        new object[] { new DateTime(2019, 11, 18), DayOfWeek.Tuesday, new DateTime(2019, 11, 19) },
        new object[] { new DateTime(2019, 11, 18), DayOfWeek.Sunday, new DateTime(2019, 11, 24) },
    };

    [TestCaseSource(nameof(WeekdayTestData))]
    public void ReturnsCorrectDay(DateTime start, DayOfWeek day, DateTime expected)
    {
        GetNextWeekday(start, day).Should().Be(expected);
    }
}
