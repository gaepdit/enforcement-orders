using Enfo.WebApp.Platform.Utilities;

namespace WebAppTests.Platform;

[TestFixture]
public class FileSizeTests
{
    [Test]
    [TestCase(0, "0 bytes")]
    [TestCase(1, "1 bytes")]
    [TestCase(10, "10 bytes")]
    [TestCase(1024, "1.00 KB")]
    [TestCase(2048, "2.0 KB")]
    [TestCase(99999999, "95.3674 MB")]
    [TestCase(-1, "-1 bytes")]
    [TestCase(-10, "-10 bytes")]
    [TestCase(-1024, "-1.00 KB")]
    [TestCase(-2048, "-2.0 KB")]
    [TestCase(-99999999, "-95.3674 MB")]
    public void FileSize_ReturnsCorrectly(long input, string expected)
    {
        var result = input.ToFileSizeString();
        result.Should().Be(expected);
    }
}
