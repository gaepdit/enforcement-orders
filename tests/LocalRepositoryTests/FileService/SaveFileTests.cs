using EnfoTests.TestData;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.LocalRepositoryTests.FileService;

[TestFixture]
public class SaveFileTests
{
    [Test]
    public async Task WhenValid_AddsFileToList()
    {
        var formFile = new Mock<IFormFile>();
        formFile.Setup(l => l.Length).Returns(1);
        formFile.Setup(l => l.FileName).Returns("test.pdf");

        var fileCount = AttachmentData.AttachmentFiles.Count;
        var id = Guid.NewGuid();

        var fileService = new Enfo.LocalRepository.FileService();
        await fileService.SaveFileAsync(formFile.Object, id);

        using (new AssertionScope())
        {
            AttachmentData.AttachmentFiles.Count.Should().Be(fileCount + 1);
            AttachmentData.AttachmentFiles.Any(a => a.FileName == $"{id.ToString()}.pdf").Should().BeTrue();
        }
    }

    [Test]
    public async Task WhenFileIsEmpty_ListIsUnchanged()
    {
        var formFile = new Mock<IFormFile>();
        formFile.Setup(l => l.Length).Returns(0);
        formFile.Setup(l => l.FileName).Returns("test.pdf");

        var fileCount = AttachmentData.AttachmentFiles.Count;
        var id = Guid.NewGuid();
        var expectedFilename = $"{id.ToString()}.pdf";

        var fileService = new Enfo.LocalRepository.FileService();
        await fileService.SaveFileAsync(formFile.Object, id);

        using (new AssertionScope())
        {
            AttachmentData.AttachmentFiles.Count.Should().Be(fileCount);
            AttachmentData.AttachmentFiles.Any(a => a.FileName == expectedFilename).Should().BeFalse();
        }
    }

    [Test]
    public async Task WhenFileNameIsMissing_ListIsUnchanged()
    {
        var fileCount = AttachmentData.AttachmentFiles.Count;

        var formFile = new Mock<IFormFile>();
        formFile.Setup(l => l.Length).Returns(1);
        formFile.Setup(l => l.FileName).Returns("");

        var id = Guid.NewGuid();
        var expectedFilename = $"{id.ToString()}.pdf";

        var fileService = new Enfo.LocalRepository.FileService();
        await fileService.SaveFileAsync(formFile.Object, id);

        using (new AssertionScope())
        {
            AttachmentData.AttachmentFiles.Count.Should().Be(fileCount);
            AttachmentData.AttachmentFiles.Any(a => a.FileName == expectedFilename).Should().BeFalse();
        }
    }

    [Test]
    public async Task WhenFileIsTooLarge_ThrowsException()
    {
        var fileCount = AttachmentData.AttachmentFiles.Count;
        var fileService = new Enfo.LocalRepository.FileService();

        var formFile = new Mock<IFormFile>();
        formFile.Setup(l => l.Length).Returns((long)int.MaxValue + 1);
        formFile.Setup(l => l.FileName).Returns("test.pdf");

        var action = async () => await fileService.SaveFileAsync(formFile.Object, Guid.Empty);

        using (new AssertionScope())
        {
            await action.Should().ThrowAsync<OverflowException>();
            AttachmentData.AttachmentFiles.Count.Should().Be(fileCount);
        }
    }
}
