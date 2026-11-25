using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.Identity;
using Enfo.WebApp.Pages.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;

namespace WebAppTests.Pages.Admin;

[TestFixture]
public class DetailsModelTests
{
    [Test]
    public async Task OnGet_ReturnsWithOrder()
    {
        var itemId = EnforcementOrderData.EnforcementOrders[0].Id;
        var item = ResourceHelper.GetEnforcementOrderAdminView(itemId);
        var repo = Substitute.For<IEnforcementOrderRepository>();
        repo.GetAdminViewAsync(itemId).Returns(item);
        var page = new DetailsModel(repo) { Id = itemId };

        await page.OnGetAsync();

        page.Item.Should().Be(item);
    }

    [Test]
    public async Task OnGet_MissingIdReturnsNotFound()
    {
        var repo = Substitute.For<IEnforcementOrderRepository>();
        var page = new DetailsModel(repo) { Id = 0 };

        var result = await page.OnGetAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<NotFoundResult>();
            page.Item.Should().BeNull();
        }
    }

    [Test]
    public async Task OnGet_NonexistentIdReturnsNotFound()
    {
        var repo = Substitute.For<IEnforcementOrderRepository>();
        var page = new DetailsModel(repo) { Id = -1 };

        var result = await page.OnGetAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<NotFoundObjectResult>();
            page.Item.Should().BeNull();
        }
    }

    [Test]
    public async Task AddAttachments_ReturnsRedirect()
    {
        // Stub user & page context
        var claims = new List<Claim> { new(ClaimTypes.Role, AppRole.OrderAdministrator) };
        var httpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(claims)) };
        var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor());
        var pageContext = new PageContext(actionContext);

        // Mock repo
        var itemId = EnforcementOrderData.EnforcementOrders[0].Id;
        var item = ResourceHelper.GetEnforcementOrderAdminView(itemId);
        var repoMock = Substitute.For<IEnforcementOrderRepository>();
        repoMock.GetAdminViewAsync(itemId).Returns(item);

        // Mock attachment
        var formFileMock = Substitute.For<IFormFile>();
        formFileMock.Length.Returns(1);
        formFileMock.FileName.Returns("test.pdf");

        var page = new DetailsModel(repoMock)
        {
            Id = itemId,
            Attachment = formFileMock,
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
