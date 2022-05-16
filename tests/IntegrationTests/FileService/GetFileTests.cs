using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace EnfoTests.Infrastructure.FileService;

public class GetFileTests
{
    [Fact]
    public async Task WhenNotExists_ReturnsEmpty()
    {
        var service = new Enfo.Infrastructure.Services.FileService("./");
        var result = await service.GetFileAsync("none");
        result.Should().BeEmpty();
    }
}
