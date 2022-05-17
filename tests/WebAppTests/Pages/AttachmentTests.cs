using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.Services;
using Enfo.Domain.Utils;
using Enfo.LocalRepository.Attachments;
using Enfo.WebApp.Pages;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnfoTests.WebApp.Pages;

[TestFixture]
public class AttachmentTests
{
    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        // Arrange
        var item = new AttachmentView(AttachmentData.Attachments.First());
        var expectedContentType = FileTypes.GetContentType(item.FileExtension);

        var repo = new Mock<IEnforcementOrderRepository>();
        repo.Setup(l => l.GetAttachmentAsync(It.IsAny<Guid>()))
            .ReturnsAsync(item);

        const string expectedFileContents = "Hello world!";
        var encoder = new UTF8Encoding();
        var expectedFile = encoder.GetBytes(expectedFileContents);
        var fileService = new Mock<IFileService>();
        fileService.Setup(l => l.GetFileAsync(It.IsAny<string>()))
            .ReturnsAsync(expectedFile);

        // Act
        var page = new Attachment(repo.Object, fileService.Object);
        var response = await page.OnGetAsync(item.Id, item.FileName);

        // Assert
        Assert.Multiple(() =>
        {
            response.Should().BeOfType<FileContentResult>();
            var result = response as FileContentResult;
            result!.ContentType.Should().Be(expectedContentType);
            result.FileDownloadName.Should().BeEmpty();
            encoder.GetString(result.FileContents).Should().Be(expectedFileContents);
        });
    }

    [Test]
    public async Task NullId_ReturnsNotFound()
    {
        var page = new Attachment(default, default);
        var result = await page.OnGetAsync(null, null);
        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task NullAttachment_ReturnsNotFound()
    {
        var repo = new Mock<IEnforcementOrderRepository>();
        repo.Setup(l => l.GetAttachmentAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as AttachmentView);

        var page = new Attachment(repo.Object, default);
        var response = await page.OnGetAsync(Guid.Empty, null);

        Assert.Multiple(() =>
        {
            response.Should().BeOfType<NotFoundObjectResult>();
            ((NotFoundObjectResult)response).Value.Should().Be($"File ID not found: {Guid.Empty}");
        });
    }

    [Test]
    public async Task EmptyFileName_ReturnsNotFound()
    {
        var view = new AttachmentView(new Enfo.Domain.EnforcementOrders.Entities.Attachment
        {
            Id = Guid.Empty,
            FileName = null,
        });

        var repo = new Mock<IEnforcementOrderRepository>();
        repo.Setup(l => l.GetAttachmentAsync(It.IsAny<Guid>()))
            .ReturnsAsync(view);

        var page = new Attachment(repo.Object, default);
        var response = await page.OnGetAsync(Guid.Empty, null);

        Assert.Multiple(() =>
        {
            response.Should().BeOfType<NotFoundObjectResult>();
            ((NotFoundObjectResult)response).Value.Should().Be($"File ID not found: {Guid.Empty}");
        });
    }

    [Test]
    public async Task IncorrectFileName_ReturnsRedirectToCorrectFileName()
    {
        var view = new AttachmentView(new Enfo.Domain.EnforcementOrders.Entities.Attachment
        {
            Id = Guid.Empty,
            FileName = "right",
        });

        var repo = new Mock<IEnforcementOrderRepository>();
        repo.Setup(l => l.GetAttachmentAsync(It.IsAny<Guid>()))
            .ReturnsAsync(view);

        var page = new Attachment(repo.Object, default);
        var response = await page.OnGetAsync(Guid.Empty, "wrong");

        Assert.Multiple(() =>
        {
            response.Should().BeOfType<RedirectToPageResult>();
            var result = response as RedirectToPageResult;
            result!.PageName.Should().Be("Attachment");
            result.RouteValues!["id"].Should().Be(Guid.Empty);
            result.RouteValues["fileName"].Should().Be(view.FileName);
        });
    }

    [Test]
    public async Task EmptyFile_ReturnsNotFound()
    {
        var view = new AttachmentView(new Enfo.Domain.EnforcementOrders.Entities.Attachment
        {
            Id = Guid.Empty,
            FileName = "abc",
        });

        var repo = new Mock<IEnforcementOrderRepository>();
        repo.Setup(l => l.GetAttachmentAsync(It.IsAny<Guid>()))
            .ReturnsAsync(view);

        var fileService = new Mock<IFileService>();
        fileService.Setup(l => l.GetFileAsync(It.IsAny<string>()))
            .ReturnsAsync(Array.Empty<byte>());

        var page = new Attachment(repo.Object, fileService.Object);
        var response = await page.OnGetAsync(Guid.Empty, view.FileName);

        Assert.Multiple(() =>
        {
            response.Should().BeOfType<NotFoundObjectResult>();
            ((NotFoundObjectResult)response).Value.Should().Be($"File not available: {view.FileName}");
        });
    }
}
