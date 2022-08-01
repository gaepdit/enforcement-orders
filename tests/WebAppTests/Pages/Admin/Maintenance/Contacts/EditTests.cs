using Enfo.Domain.EpdContacts.Repositories;
using Enfo.Domain.EpdContacts.Resources;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages.Admin.Maintenance.Contacts;
using Enfo.WebApp.Platform.RazorHelpers;
using EnfoTests.TestData;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.WebApp.Pages.Admin.Maintenance.Contacts;

[TestFixture]
public class EditTests
{
    [Test]
    public async Task OnGet_ReturnsWithItem()
    {
        var item = ResourceHelper.GetEpdContactViewList()[0];
        var repo = new Mock<IEpdContactRepository>();
        repo.Setup(l => l.GetAsync(item.Id)).ReturnsAsync(item);
        var page = new Edit(repo.Object);

        await page.OnGetAsync(item.Id);

        Assert.Multiple(() =>
        {
            page.Item.Should().BeEquivalentTo(new EpdContactCommand(item));
            page.Item.Id.Should().Be(item.Id);
        });
    }

    [Test]
    public async Task OnGet_GivenNullId_ReturnsNotFound()
    {
        var page = new Edit(Mock.Of<IEpdContactRepository>());

        var result = await page.OnGetAsync(null);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<NotFoundResult>();
            page.Item.Should().BeNull();
        });
    }

    [Test]
    public async Task OnGet_GivenInvalidId_ReturnsNotFound()
    {
        var repo = new Mock<IEpdContactRepository>();
        repo.Setup(l => l.GetAsync(It.IsAny<int>())).ReturnsAsync(null as EpdContactView);
        var page = new Edit(repo.Object);

        var result = await page.OnGetAsync(-1);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<NotFoundObjectResult>();
            page.Item.Should().BeNull();
        });
    }

    [Test]
    public async Task OnGet_GivenInactiveItem_RedirectsWithDisplayMessage()
    {
        var item = ResourceHelper.GetEpdContactViewList().Single(e => !e.Active);
        var repo = new Mock<IEpdContactRepository>();
        repo.Setup(l => l.GetAsync(It.IsAny<int>()))
            .ReturnsAsync(item);

        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        var page = new Edit(repo.Object)
            { TempData = tempData };

        var result = await page.OnGetAsync(item.Id);

        var expected = new DisplayMessage(Context.Warning,
            $"Inactive {Edit.ThisOption.PluralName} cannot be edited.");

        Assert.Multiple(() =>
        {
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expected);
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        });
    }

    [Test]
    public async Task OnPost_GivenInvalidId_ReturnsNotFound()
    {
        var item = new EpdContactCommand { Id = 0 };
        var repo = new Mock<IEpdContactRepository>();
        repo.Setup(l => l.GetAsync(It.IsAny<int>())).ReturnsAsync(null as EpdContactView);
        var page = new Edit(repo.Object) { Item = item };

        var result = await page.OnPostAsync();

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnPost_GivenInactiveItem_RedirectsWithDisplayMessage()
    {
        var item = ResourceHelper.GetEpdContactViewList().Single(e => !e.Active);
        var repo = new Mock<IEpdContactRepository>();
        repo.Setup(l => l.GetAsync(It.IsAny<int>()))
            .ReturnsAsync(item);

        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        var page = new Edit(repo.Object)
            { TempData = tempData, Item = new EpdContactCommand { Id = 0 } };

        var result = await page.OnPostAsync();

        var expected = new DisplayMessage(Context.Warning,
            $"Inactive {Edit.ThisOption.PluralName} cannot be edited.");

        Assert.Multiple(() =>
        {
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expected);
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        });
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var item = new EpdContactCommand(ResourceHelper.GetEpdContactViewList()[0]);
        var repo = new Mock<IEpdContactRepository> { DefaultValue = DefaultValue.Mock };
        repo.Setup(l => l.GetAsync(It.IsAny<int>()))
            .ReturnsAsync(ResourceHelper.GetEpdContactViewList()[0]);

        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        var page = new Edit(repo.Object) { TempData = tempData, Item = item };

        var result = await page.OnPostAsync();

        var expected = new DisplayMessage(Context.Success,
            $"{Edit.ThisOption.SingularName} successfully updated.");

        Assert.Multiple(() =>
        {
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expected);
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        });
    }

    [Test]
    public async Task OnPost_GivenModelError_ReturnsPageWithModelError()
    {
        var repo = new Mock<IEpdContactRepository> { DefaultValue = DefaultValue.Mock };
        repo.Setup(l => l.GetAsync(It.IsAny<int>()))
            .ReturnsAsync(ResourceHelper.GetEpdContactViewList()[0]);
        var page = new Edit(repo.Object) { Item = new EpdContactCommand { Id = 0 } };
        page.ModelState.AddModelError("key", "message");

        var result = await page.OnPostAsync();

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
        });
    }
}
