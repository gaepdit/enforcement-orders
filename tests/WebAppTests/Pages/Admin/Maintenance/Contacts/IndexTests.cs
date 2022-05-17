using Enfo.Domain.EpdContacts.Repositories;
using Enfo.Domain.EpdContacts.Resources;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages.Admin.Maintenance.Contacts;
using Enfo.WebApp.Platform.RazorHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using static EnfoTests.Helpers.ResourceHelper;

namespace EnfoTests.WebApp.Pages.Admin.Maintenance.Contacts;

[TestFixture]
public class IndexTests
{
    [Test]
    public async Task OnGet_ReturnsWithOrder()
    {
        var list = GetEpdContactViewList();
        var repo = new Mock<IEpdContactRepository>();
        repo.Setup(l => l.ListAsync(true)).ReturnsAsync(list);
        var page = new Index(repo.Object);

        await page.OnGetAsync();

        Assert.Multiple(() =>
        {
            page.Items.Should().BeEquivalentTo(list);
            page.Message.Should().BeNull();
        });
    }

    [Test]
    public async Task SetDisplayMessage_ReturnsWithDisplayMessage()
    {
        var repo = new Mock<IEpdContactRepository> { DefaultValue = DefaultValue.Mock };

        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        var page = new Index(repo.Object) { TempData = tempData };

        page.TempData.SetDisplayMessage(Context.Info, "Info message");
        await page.OnGetAsync();

        var expected = new DisplayMessage(Context.Info, "Info message");
        page.Message.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task OnPost_ReturnsRedirectWithDisplayMessage()
    {
        var item = GetEpdContactViewList()[0];
        var repo = new Mock<IEpdContactRepository> { DefaultValue = DefaultValue.Mock };
        repo.Setup(l => l.GetAsync(It.IsAny<int>()))
            .ReturnsAsync(item);

        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        var page = new Index(repo.Object) { TempData = tempData };

        var result = await page.OnPostAsync(item.Id);

        var expected = new DisplayMessage(Context.Success,
            $"{Index.ThisOption.SingularName} successfully {(item.Active ? "deactivated" : "restored")}.");

        Assert.Multiple(() =>
        {
            page.TempData?.GetDisplayMessage().Should().BeEquivalentTo(expected);
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        });
    }

    [Test]
    public async Task OnPost_GivenNullId_ReturnsBadRequest()
    {
        var repo = new Mock<IEpdContactRepository> { DefaultValue = DefaultValue.Mock };
        var page = new Index(repo.Object);

        var result = await page.OnPostAsync(null);

        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPost_GivenInvalidId_ReturnsNotFound()
    {
        var repo = new Mock<IEpdContactRepository> { DefaultValue = DefaultValue.Mock };
        repo.Setup(l => l.GetAsync(It.IsAny<int>()))
            .ReturnsAsync(null as EpdContactView);
        var page = new Index(repo.Object);

        var result = await page.OnPostAsync(1);

        result.Should().BeOfType<NotFoundResult>();
    }
}
