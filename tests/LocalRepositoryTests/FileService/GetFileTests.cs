using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LocalRepositoryTests.FileService;

[TestFixture]
public class GetFileTests
{
    private Enfo.LocalRepository.FileService? _fileService;

    [SetUp]
    public void SetUp() => _fileService = new Enfo.LocalRepository.FileService();

    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        var attachmentFile = _fileService!.Files.First();
        var result = await _fileService.GetFileAsync(attachmentFile.FileName);
        Convert.ToBase64String(result).Should().BeEquivalentTo(attachmentFile.Base64EncodedFile);
    }

    [Test]
    public async Task WhenNotExists_ReturnsEmpty()
    {
        var result = await _fileService!.GetFileAsync("none");
        result.Should().BeEmpty();
    }
}
