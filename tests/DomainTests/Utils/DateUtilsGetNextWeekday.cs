using Enfo.Domain.Utils;

namespace DomainTests.Utils;

[TestFixture]
public class DateUtilsGetNextWeekday
{
    public static IEnumerable<object[]> WeekdayTestData => new List<object[]>
    {
        new object[]
        {
            new DateTime(2019, 11, 18, 0, 0, 0, DateTimeKind.Local),
            DayOfWeek.Monday,
            new DateTime(2019, 11, 18, 0, 0, 0, DateTimeKind.Local),
        },
        new object[]
        {
            new DateTime(2019, 11, 18, 0, 0, 0, DateTimeKind.Local),
            DayOfWeek.Tuesday,
            new DateTime(2019, 11, 19, 0, 0, 0, DateTimeKind.Local),
        },
        new object[]
        {
            new DateTime(2019, 11, 18, 0, 0, 0, DateTimeKind.Local),
            DayOfWeek.Sunday,
            new DateTime(2019, 11, 24, 0, 0, 0, DateTimeKind.Local),
        },
    };

    [TestCaseSource(nameof(WeekdayTestData))]
    public void ReturnsCorrectDay(DateTime start, DayOfWeek day, DateTime expected)
    {
        DateUtils.GetNextWeekday(start, day).Should().Be(expected);
    }
}
