using Enfo.WebApp.Platform.Attachments;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace EnfoTests.WebApp.Platform;

public class FileSizeTests
{
    [Theory]
    [InlineData(0, 2, "0 bytes")]
    [InlineData(1, 2, "1 bytes")]
    [InlineData(10, 2, "10 bytes")]
    [InlineData(1024, 2, "1.00 KB")]
    [InlineData(2048, 1, "2.0 KB")]
    [InlineData(99999999, 4, "95.3674 MB")]
    public void FileSize_ReturnsCorrectly(long value, int precision, string expected)
    {
        var result = value.ToFileSizeString(precision);
        result.ShouldEqual(expected);
    }
}
