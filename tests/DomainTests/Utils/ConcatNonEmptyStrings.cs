using Enfo.Domain.Utils;

namespace DomainTests.Utils;

[TestFixture]
public class ConcatNonEmptyStrings
{
    [Test]
    public void ReturnsCorrectlyGivenNonEmptyStrings()
    {
        var stringArray = new[] { "abc", "def" };
        stringArray.ConcatNonEmptyStrings(",").Should().Be("abc,def");
    }

    [Test]
    public void ReturnsNullGivenEmptyArray()
    {
        var emptyStringArray = Array.Empty<string>();
        emptyStringArray.ConcatNonEmptyStrings(",").Should().BeEmpty();
    }

    [Test]
    public void ReturnsCorrectlyGivenNullItem()
    {
        var stringArray = new[] { "abc", null, "def" };
        stringArray.ConcatNonEmptyStrings(",").Should().Be("abc,def");
    }

    [Test]
    public void ReturnsCorrectlyGivenEmptyString()
    {
        var stringArray = new[] { "abc", "", "def" };
        stringArray.ConcatNonEmptyStrings(",").Should().Be("abc,def");
    }
}
