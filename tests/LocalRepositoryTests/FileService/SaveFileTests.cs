using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LocalRepositoryTests.FileService;

[TestFixture]
public class SaveFileTests
{
    private Enfo.LocalRepository.Attachments.FileService _fileService;

    [SetUp]
    public void SetUp() => _fileService = new Enfo.LocalRepository.Attachments.FileService();

    [Test]
    public async Task WhenValid_AddsFileToList()
    {
        var fileCount = _fileService.Files.Count;
        var formFile = new Mock<IFormFile>();
        formFile.Setup(l => l.Length).Returns(1);
        formFile.Setup(l => l.FileName).Returns("test.pdf");

        var id = Guid.NewGuid();

        await _fileService.SaveFileAsync(formFile.Object, id);

        Assert.Multiple(() =>
        {
            _fileService.Files.Count.Should().Be(fileCount + 1);
            _fileService.Files.Any(a => a.FileName == $"{id.ToString()}.pdf").Should().BeTrue();
        });
    }

    [Test]
    public async Task WhenFileIsEmpty_ListIsUnchanged()
    {
        var fileCount = _fileService.Files.Count;

        var formFile = new Mock<IFormFile>();
        formFile.Setup(l => l.Length).Returns(0);
        formFile.Setup(l => l.FileName).Returns("test.pdf");

        var id = Guid.NewGuid();
        var expectedFilename = $"{id.ToString()}.pdf";

        await _fileService.SaveFileAsync(formFile.Object, id);

        Assert.Multiple(() =>
        {
            _fileService.Files.Count.Should().Be(fileCount);
            _fileService.Files.Any(a => a.FileName == expectedFilename).Should().BeFalse();
        });
    }

    [Test]
    public async Task WhenFileNameIsMissing_ListIsUnchanged()
    {
        var fileCount = _fileService.Files.Count;

        var formFile = new Mock<IFormFile>();
        formFile.Setup(l => l.Length).Returns(1);
        formFile.Setup(l => l.FileName).Returns("");

        var id = Guid.NewGuid();
        var expectedFilename = $"{id.ToString()}.pdf";

        await _fileService.SaveFileAsync(formFile.Object, id);

        Assert.Multiple(() =>
        {
            _fileService.Files.Count.Should().Be(fileCount);
            _fileService.Files.Any(a => a.FileName == expectedFilename).Should().BeFalse();
        });
    }

    [Test]
    public async Task WhenFileIsTooLarge_ThrowsException()
    {
        var fileCount = _fileService.Files.Count;

        var formFile = new Mock<IFormFile>();
        formFile.Setup(l => l.Length).Returns((long)int.MaxValue + 1);
        formFile.Setup(l => l.FileName).Returns("test.pdf");

        var act = () => _fileService.SaveFileAsync(formFile.Object, Guid.Empty);

        await act.Should().ThrowAsync<OverflowException>();
        _fileService.Files.Count.Should().Be(fileCount);
    }
}
