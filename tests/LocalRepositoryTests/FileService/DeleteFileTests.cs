using FluentAssertions;
using NUnit.Framework;
using System.Linq;

namespace LocalRepositoryTests.FileService;

[TestFixture]
public class DeleteFileTests
{
    private Enfo.LocalRepository.Attachments.FileService? _fileService;

    [SetUp]
    public void SetUp() => _fileService = new Enfo.LocalRepository.Attachments.FileService();

    [Test]
    public void WhenItemExists_RemovesItem()
    {
        var initialFileCount = _fileService!.Files.Count;
        var fileName = _fileService.Files.First().FileName;

        _fileService.TryDeleteFile(fileName);

        Assert.Multiple(() =>
        {
            _fileService.Files.Count.Should().Be(initialFileCount - 1);
            _fileService.Files.Any(a => a.FileName == fileName).Should().BeFalse();
        });
    }

    [Test]
    public void WhenNotExists_ListIsUnchanged()
    {
        var fileCount = _fileService!.Files.Count;
        _fileService!.TryDeleteFile("none");
        _fileService.Files.Count.Should().Be(fileCount);
    }
}
