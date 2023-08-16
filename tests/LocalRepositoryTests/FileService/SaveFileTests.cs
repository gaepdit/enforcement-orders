using EnfoTests.TestData;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace EnfoTests.LocalRepositoryTests.FileService;

[TestFixture]
public class SaveFileTests
{
    [Test]
    public async Task WhenValid_AddsFileToList()
    {
        var formFile = Substitute.For<IFormFile>();
        formFile.Length.Returns(1);
        formFile.FileName.Returns("test.pdf");

        var fileCount = AttachmentData.AttachmentFiles.Count;
        var id = Guid.NewGuid();

        var fileService = new Enfo.LocalRepository.FileService();
        await fileService.SaveFileAsync(formFile, id);

        using (new AssertionScope())
        {
            AttachmentData.AttachmentFiles.Count.Should().Be(fileCount + 1);
            AttachmentData.AttachmentFiles.Exists(a => a.FileName == $"{id.ToString()}.pdf").Should().BeTrue();
        }
    }

    [Test]
    public async Task WhenFileIsEmpty_ListIsUnchanged()
    {
        var formFile = Substitute.For<IFormFile>();
        formFile.Length.Returns(0);
        formFile.FileName.Returns("test.pdf");

        var fileCount = AttachmentData.AttachmentFiles.Count;
        var id = Guid.NewGuid();
        var expectedFilename = $"{id.ToString()}.pdf";

        var fileService = new Enfo.LocalRepository.FileService();
        await fileService.SaveFileAsync(formFile, id);

        using (new AssertionScope())
        {
            AttachmentData.AttachmentFiles.Count.Should().Be(fileCount);
            AttachmentData.AttachmentFiles.Exists(a => a.FileName == expectedFilename).Should().BeFalse();
        }
    }

    [Test]
    public async Task WhenFileNameIsMissing_ListIsUnchanged()
    {
        var fileCount = AttachmentData.AttachmentFiles.Count;

        var formFile = Substitute.For<IFormFile>();
        formFile.Length.Returns(1);
        formFile.FileName.Returns("");

        var id = Guid.NewGuid();
        var expectedFilename = $"{id.ToString()}.pdf";

        var fileService = new Enfo.LocalRepository.FileService();
        await fileService.SaveFileAsync(formFile, id);

        using (new AssertionScope())
        {
            AttachmentData.AttachmentFiles.Count.Should().Be(fileCount);
            AttachmentData.AttachmentFiles.Exists(a => a.FileName == expectedFilename).Should().BeFalse();
        }
    }

    [Test]
    public async Task WhenFileIsTooLarge_ThrowsException()
    {
        var fileCount = AttachmentData.AttachmentFiles.Count;
        var fileService = new Enfo.LocalRepository.FileService();

        var formFile = Substitute.For<IFormFile>();
        formFile.Length.Returns((long)int.MaxValue + 1);
        formFile.FileName.Returns("test.pdf");

        var action = async () => await fileService.SaveFileAsync(formFile, Guid.Empty);

        using (new AssertionScope())
        {
            await action.Should().ThrowAsync<OverflowException>();
            AttachmentData.AttachmentFiles.Count.Should().Be(fileCount);
        }
    }
}
