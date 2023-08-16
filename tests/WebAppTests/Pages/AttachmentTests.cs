using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.Services;
using Enfo.Domain.Utils;
using EnfoTests.TestData;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Attachment = Enfo.WebApp.Pages.Attachment;

namespace EnfoTests.WebApp.Pages;

// TODO: Improve test coverage for Attachments page

[TestFixture]
public class AttachmentTests
{
    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        // Mock user & page context
        var httpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new GenericIdentity("Name")) };
        var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor());
        var pageContext = new PageContext(actionContext);

        // Arrange
        var item = new AttachmentView(AttachmentData.Attachments[0]);
        var expectedContentType = FileTypes.GetContentType(item.FileExtension);

        var repo = Substitute.For<IEnforcementOrderRepository>();
        repo.GetAttachmentAsync(Arg.Any<Guid>()).Returns(item);

        const string expectedFileContents = "Hello world!";
        var encoder = new UTF8Encoding();
        var expectedFile = encoder.GetBytes(expectedFileContents);
        var fileService = Substitute.For<IFileService>();
        fileService.GetFileAsync(Arg.Any<string>()).Returns(expectedFile);

        // Act
        var page = new Attachment(repo, fileService) { PageContext = pageContext };
        var response = await page.OnGetAsync(item.Id, item.FileName);

        // Assert
        using (new AssertionScope())
        {
            response.Should().BeOfType<FileContentResult>();
            var result = response as FileContentResult;
            result!.ContentType.Should().Be(expectedContentType);
            result.FileDownloadName.Should().BeEmpty();
            encoder.GetString(result.FileContents).Should().Be(expectedFileContents);
        }
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
        // Mock user & page context
        var httpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new GenericIdentity("Name")) };
        var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor());
        var pageContext = new PageContext(actionContext);

        var repo = Substitute.For<IEnforcementOrderRepository>();
        repo.GetAttachmentAsync(Arg.Any<Guid>()).Returns(null as AttachmentView);

        var page = new Attachment(repo, default) { PageContext = pageContext };
        var response = await page.OnGetAsync(Guid.Empty, null);

        using (new AssertionScope())
        {
            response.Should().BeOfType<NotFoundObjectResult>();
            ((NotFoundObjectResult)response).Value.Should().Be($"Attachment ID not found: {Guid.Empty.ToString()}");
        }
    }

    [Test]
    public async Task EmptyFileName_ReturnsNotFound()
    {
        // Mock user & page context
        var httpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new GenericIdentity("Name")) };
        var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor());
        var pageContext = new PageContext(actionContext);

        var view = new AttachmentView(new Enfo.Domain.EnforcementOrders.Entities.Attachment
        {
            Id = Guid.Empty,
            FileName = null,
            EnforcementOrder = new EnforcementOrder { Id = 1 },
        });

        var repo = Substitute.For<IEnforcementOrderRepository>();
        repo.GetAttachmentAsync(Arg.Any<Guid>()).Returns(view);

        var page = new Attachment(repo, default) { PageContext = pageContext };
        var response = await page.OnGetAsync(Guid.Empty, null);

        using (new AssertionScope())
        {
            response.Should().BeOfType<NotFoundObjectResult>();
            ((NotFoundObjectResult)response).Value.Should().Be($"Attachment ID not found: {Guid.Empty.ToString()}");
        }
    }

    [Test]
    public async Task IncorrectFileName_ReturnsRedirectToCorrectFileName()
    {
        // Mock user & page context
        var httpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new GenericIdentity("Name")) };
        var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor());
        var pageContext = new PageContext(actionContext);

        var view = new AttachmentView(new Enfo.Domain.EnforcementOrders.Entities.Attachment
        {
            Id = Guid.Empty,
            FileName = "right",
            EnforcementOrder = new EnforcementOrder { Id = 1 },
        });

        var repo = Substitute.For<IEnforcementOrderRepository>();
        repo.GetAttachmentAsync(Arg.Any<Guid>()).Returns(view);

        var page = new Attachment(repo, default) { PageContext = pageContext };
        var response = await page.OnGetAsync(Guid.Empty, "wrong");

        using (new AssertionScope())
        {
            response.Should().BeOfType<RedirectToPageResult>();
            var result = response as RedirectToPageResult;
            result!.PageName.Should().Be("Attachment");
            result.RouteValues!["id"].Should().Be(Guid.Empty);
            result.RouteValues["fileName"].Should().Be(view.FileName);
        }
    }

    [Test]
    public async Task EmptyFile_ReturnsNotFound()
    {
        // Mock user & page context
        var httpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new GenericIdentity("Name")) };
        var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor());
        var pageContext = new PageContext(actionContext);

        var view = new AttachmentView(new Enfo.Domain.EnforcementOrders.Entities.Attachment
        {
            Id = Guid.Empty,
            FileName = "abc",
            EnforcementOrder = new EnforcementOrder { Id = 1 },
        });

        var repo = Substitute.For<IEnforcementOrderRepository>();
        repo.GetAttachmentAsync(Arg.Any<Guid>()).Returns(view);

        var fileService = Substitute.For<IFileService>();
        fileService.GetFileAsync(Arg.Any<string>()).Returns(Array.Empty<byte>());

        var page = new Attachment(repo, fileService) { PageContext = pageContext };
        var response = await page.OnGetAsync(Guid.Empty, view.FileName);

        using (new AssertionScope())
        {
            response.Should().BeOfType<NotFoundObjectResult>();
            ((NotFoundObjectResult)response).Value.Should().Be($"File not available: {view.FileName}");
        }
    }
}
