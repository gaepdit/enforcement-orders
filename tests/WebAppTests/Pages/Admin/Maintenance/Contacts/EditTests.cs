using Enfo.Domain.EpdContacts.Repositories;
using Enfo.Domain.EpdContacts.Resources;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages.Admin.Maintenance.Contacts;
using Enfo.WebApp.Platform.RazorHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace EnfoTests.WebApp.Pages.Admin.Maintenance.Contacts;

[TestFixture]
public class EditTests
{
    [Test]
    public async Task OnGet_ReturnsWithItem()
    {
        var item = ResourceHelper.GetEpdContactViewList()[0];
        var repo = Substitute.For<IEpdContactRepository>();
        repo.GetAsync(item.Id).Returns(item);
        var page = new Edit(repo);

        await page.OnGetAsync(item.Id);

        using (new AssertionScope())
        {
            page.Item.Should().BeEquivalentTo(new EpdContactCommand(item));
            page.Item.Id.Should().Be(item.Id);
        }
    }

    [Test]
    public async Task OnGet_GivenNullId_ReturnsNotFound()
    {
        var page = new Edit(Substitute.For<IEpdContactRepository>());

        var result = await page.OnGetAsync(null);

        using (new AssertionScope())
        {
            result.Should().BeOfType<NotFoundResult>();
            page.Item.Should().BeNull();
        }
    }

    [Test]
    public async Task OnGet_GivenInvalidId_ReturnsNotFound()
    {
        var repo = Substitute.For<IEpdContactRepository>();
        repo.GetAsync(Arg.Any<int>()).Returns(null as EpdContactView);
        var page = new Edit(repo);

        var result = await page.OnGetAsync(-1);

        using (new AssertionScope())
        {
            result.Should().BeOfType<NotFoundObjectResult>();
            page.Item.Should().BeNull();
        }
    }

    [Test]
    public async Task OnGet_GivenInactiveItem_RedirectsWithDisplayMessage()
    {
        var item = ResourceHelper.GetEpdContactViewList().Single(e => !e.Active);
        var repo = Substitute.For<IEpdContactRepository>();
        repo.GetAsync(Arg.Any<int>()).Returns(item);

        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Substitute.For<ITempDataProvider>());
        var page = new Edit(repo)
            { TempData = tempData };

        var result = await page.OnGetAsync(item.Id);

        var expected = new DisplayMessage(Context.Warning,
            $"Inactive {Edit.ThisOption.PluralName} cannot be edited.");

        using (new AssertionScope())
        {
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expected);
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        }
    }

    [Test]
    public async Task OnPost_GivenInvalidId_ReturnsNotFound()
    {
        var item = new EpdContactCommand { Id = 0 };
        var repo = Substitute.For<IEpdContactRepository>();
        repo.GetAsync(Arg.Any<int>()).Returns(null as EpdContactView);
        var page = new Edit(repo) { Item = item };

        var result = await page.OnPostAsync();

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnPost_GivenInactiveItem_RedirectsWithDisplayMessage()
    {
        var item = ResourceHelper.GetEpdContactViewList().Single(e => !e.Active);
        var repo = Substitute.For<IEpdContactRepository>();
        repo.GetAsync(Arg.Any<int>()).Returns(item);

        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Substitute.For<ITempDataProvider>());
        var page = new Edit(repo)
            { TempData = tempData, Item = new EpdContactCommand { Id = 0 } };

        var result = await page.OnPostAsync();

        var expected = new DisplayMessage(Context.Warning,
            $"Inactive {Edit.ThisOption.PluralName} cannot be edited.");

        using (new AssertionScope())
        {
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expected);
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        }
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var item = new EpdContactCommand(ResourceHelper.GetEpdContactViewList()[0]);
        var repo = Substitute.For<IEpdContactRepository>();
        repo.GetAsync(Arg.Any<int>()).Returns(ResourceHelper.GetEpdContactViewList()[0]);

        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Substitute.For<ITempDataProvider>());
        var page = new Edit(repo) { TempData = tempData, Item = item };

        var result = await page.OnPostAsync();

        var expected = new DisplayMessage(Context.Success,
            $"{Edit.ThisOption.SingularName} successfully updated.");

        using (new AssertionScope())
        {
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expected);
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        }
    }

    [Test]
    public async Task OnPost_GivenModelError_ReturnsPageWithModelError()
    {
        var repo = Substitute.For<IEpdContactRepository>();
        repo.GetAsync(Arg.Any<int>()).Returns(ResourceHelper.GetEpdContactViewList()[0]);
        var page = new Edit(repo) { Item = new EpdContactCommand { Id = 0 } };
        page.ModelState.AddModelError("key", "message");

        var result = await page.OnPostAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
        }
    }
}
