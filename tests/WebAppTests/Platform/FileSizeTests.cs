using Enfo.WebApp.Platform.Utilities;

namespace WebAppTests.Platform;

[TestFixture]
public class FileSizeTests
{
    [Test]
    [TestCase(0L, "0 bytes")]
    [TestCase(1L, "1 bytes")]
    [TestCase(10L, "10 bytes")]
    [TestCase(1024L, "1.0 KB")]
    [TestCase(2048L, "2.0 KB")]
    [TestCase(99_999_999L, "95.4 MB")]
    [TestCase(1_073_741_824L, "1.0 GB")]
    [TestCase(1_099_511_627_776L, "1.0 TB")]
    [TestCase(1_125_899_906_842_624L, "1.0 PB")]
    [TestCase(1_152_921_504_606_846_976L, "1.0 EB")]
    [TestCase(long.MaxValue, "8.0 EB")]
    public void FileSize_ReturnsCorrectly(long input, string expected)
    {
        var result = input.ToFileSizeString();
        result.Should().Be(expected);
    }
}
