using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.Users.Entities;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages.Admin;
using Enfo.WebApp.Platform.RazorHelpers;
using EnfoTests.TestData;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EnfoTests.WebApp.Pages.Admin;

// TODO: Improve test coverage for Details page

[TestFixture]
public class DetailsTests
{
    [Test]
    public async Task OnGet_ReturnsWithOrder()
    {
        var itemId = EnforcementOrderData.EnforcementOrders.First().Id;
        var item = ResourceHelper.GetEnforcementOrderAdminView(itemId);
        var repo = new Mock<IEnforcementOrderRepository>();
        repo.Setup(l => l.GetAdminViewAsync(itemId)).ReturnsAsync(item);
        var page = new Details(repo.Object);

        await page.OnGetAsync(itemId);

        page.Item.Should().Be(item);
    }

    [Test]
    public async Task SetDisplayMessage_ReturnsWithDisplayMessage()
    {
        // Not testing returned Item, but it must be populated to return Page
        var itemId = EnforcementOrderData.EnforcementOrders.First().Id;
        var item = ResourceHelper.GetEnforcementOrderAdminView(itemId);
        var repo = new Mock<IEnforcementOrderRepository>();
        repo.Setup(l => l.GetAdminViewAsync(itemId)).ReturnsAsync(item);

        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        var page = new Details(repo.Object) { TempData = tempData };

        page.TempData.SetDisplayMessage(Context.Info, "Info message");
        await page.OnGetAsync(itemId);

        var expected = new DisplayMessage(Context.Info, "Info message");
        page.Message.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task OnGet_MissingIdReturnsNotFound()
    {
        var repo = new Mock<IEnforcementOrderRepository>();
        var page = new Details(repo.Object);

        var result = await page.OnGetAsync(null);

        using (new AssertionScope())
        {
            result.Should().BeOfType<NotFoundResult>();
            page.Item.Should().BeNull();
            page.Message.Should().BeNull();
        }
    }

    [Test]
    public async Task OnGet_NonexistentIdReturnsNotFound()
    {
        var repo = new Mock<IEnforcementOrderRepository>();
        var page = new Details(repo.Object);

        var result = await page.OnGetAsync(-1);

        using (new AssertionScope())
        {
            result.Should().BeOfType<NotFoundObjectResult>();
            page.Item.Should().BeNull();
            page.Message.Should().BeNull();
        }
    }

    [Test]
    public async Task AddAttachments_ReturnsRedirect()
    {
        // Stub user & page context
        var claims = new List<Claim> { new(ClaimTypes.Role, UserRole.OrderAdministrator) };
        var httpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(claims)) };
        var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor());
        var pageContext = new PageContext(actionContext);

        // Mock repo
        var itemId = EnforcementOrderData.EnforcementOrders.First().Id;
        var item = ResourceHelper.GetEnforcementOrderAdminView(itemId);
        var repoMock = new Mock<IEnforcementOrderRepository>();
        repoMock.Setup(l => l.GetAdminViewAsync(itemId)).ReturnsAsync(item);

        // Mock attachment
        var formFileMock = new Mock<IFormFile>();
        formFileMock.Setup(l => l.Length).Returns(1);
        formFileMock.Setup(l => l.FileName).Returns("test.pdf");
        
        var page = new Details(repoMock.Object)
        {
            Id = itemId, 
            Attachment = formFileMock.Object,
            PageContext = pageContext,
        };

        var result = await page.OnPostAddAttachmentAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Details");
            ((RedirectToPageResult)result).RouteValues!["Id"].Should().Be(itemId);
        }
    }
}
