using Enfo.WebApp.Platform.Utilities;

namespace WebAppTests.Platform;

[TestFixture]
public class FileSizeTests
{
    [Test]
    [TestCase(0, 2, "0 bytes")]
    [TestCase(1, 2, "1 bytes")]
    [TestCase(10, 2, "10 bytes")]
    [TestCase(1024, 2, "1.00 KB")]
    [TestCase(2048, 1, "2.0 KB")]
    [TestCase(99999999, 4, "95.3674 MB")]
    public void FileSize_ReturnsCorrectly(long value, int precision, string expected)
    {
        var result = value.ToFileSizeString(precision);
        result.Should().Be(expected);
    }
}
