using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using TestData;

namespace LocalRepositoryTests.FileService;

[TestFixture]
public class GetFileTests
{
    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        var attachmentFile = AttachmentData.AttachmentFiles.First(a => !string.IsNullOrEmpty(a.Base64EncodedFile));
        var fileService = new Enfo.LocalRepository.FileService();
        var result = await fileService.GetFileAsync(attachmentFile.FileName);
        Convert.ToBase64String(result).Should().BeEquivalentTo(attachmentFile.Base64EncodedFile);
    }

    [Test]
    public async Task WhenNotExists_ReturnsEmpty()
    {
        var fileService = new Enfo.LocalRepository.FileService();
        var result = await fileService.GetFileAsync("none");
        result.Should().BeEmpty();
    }
}
