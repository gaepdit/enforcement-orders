using Enfo.Domain.EpdContacts;
using Enfo.Domain.EpdContacts.Resources;
using Enfo.WebApp.Pages.Admin.Maintenance.Contacts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebAppTests.Pages.Admin.Maintenance.Contacts;

[TestFixture]
public class EditTests
{
    [Test]
    public async Task OnGet_ReturnsWithItem()
    {
        var item = ResourceHelper.GetEpdContactViewList()[0];
        var repo = Substitute.For<IEpdContactRepository>();
        repo.GetAsync(item.Id).Returns(item);
        var page = new EditModel(repo) { Id = item.Id };

        await page.OnGetAsync();

        using (new AssertionScope())
        {
            page.Item.Should().BeEquivalentTo(new EpdContactCommand(item));
            page.Id.Should().Be(item.Id);
        }
    }

    [Test]
    public async Task OnGet_GivenNullId_ReturnsNotFound()
    {
        var page = new EditModel(Substitute.For<IEpdContactRepository>());

        var result = await page.OnGetAsync();

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
        repo.GetAsync(Arg.Any<int>()).Returns(null as EpdContactViewDto);
        var page = new EditModel(repo) { Id = -1 };

        var result = await page.OnGetAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<NotFoundObjectResult>();
            page.Item.Should().BeNull();
        }
    }

    [Test]
    public async Task OnPost_GivenInvalidId_ReturnsNotFound()
    {
        var repo = Substitute.For<IEpdContactRepository>();
        repo.GetAsync(Arg.Any<int>()).Returns(null as EpdContactViewDto);
        var page = new EditModel(repo) { Id = 0 };

        var result = await page.OnPostAsync();

        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirect()
    {
        var view = ResourceHelper.GetEpdContactViewList()[0];
        var item = new EpdContactCommand(view);
        var repo = Substitute.For<IEpdContactRepository>();
        repo.GetAsync(Arg.Any<int>()).Returns(view);

        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Substitute.For<ITempDataProvider>());
        var page = new EditModel(repo) { TempData = tempData, Item = item, Id = view.Id };

        var result = await page.OnPostAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        }
    }

    [Test]
    public async Task OnPost_GivenModelError_ReturnsPageWithModelError()
    {
        var repo = Substitute.For<IEpdContactRepository>();
        repo.GetAsync(Arg.Any<int>()).Returns(ResourceHelper.GetEpdContactViewList()[0]);
        var page = new EditModel(repo) { Item = new EpdContactCommand(), Id = 1 };
        page.ModelState.AddModelError("key", "message");

        var result = await page.OnPostAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
        }
    }
}
