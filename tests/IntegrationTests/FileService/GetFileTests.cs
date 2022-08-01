using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace EnfoTests.Infrastructure.FileService;

[TestFixture]
public class GetFileTests
{
    [Test]
    public async Task WhenNotExists_ReturnsEmpty()
    {
        var service = new Enfo.Infrastructure.Services.FileService("./");
        var result = await service.GetFileAsync("none");
        result.Should().BeEmpty();
    }
}
