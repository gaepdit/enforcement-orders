using Enfo.Domain.EpdContacts.Repositories;
using Enfo.Domain.EpdContacts.Resources;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages.Admin.Maintenance.Contacts;
using Enfo.WebApp.Platform.RazorHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebAppTests.Pages.Admin.Maintenance.Contacts;

[TestFixture]
public class AddTests
{
    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var item = new EpdContactCommand
        {
            Email = "abc", Organization = "abc", Telephone = "abc", Title = "abc", ContactName = "abc",
            City = "abc", State = "GA", Street = "123", PostalCode = "01234",
        };
        var repo = Substitute.For<IEpdContactRepository>();
        repo.CreateAsync(Arg.Any<EpdContactCommand>()).Returns(1);

        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Substitute.For<ITempDataProvider>());
        var page = new Add { TempData = tempData, Item = item };

        var result = await page.OnPostAsync(repo);

        var expected = new DisplayMessage(Context.Success,
            $"{Add.ThisOption.SingularName} successfully added.");

        using (new AssertionScope())
        {
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expected);
            page.HighlightId.Should().Be(1);

            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        }
    }

    [Test]
    public async Task OnPost_GivenModelError_ReturnsPageWithModelError()
    {
        var repo = Substitute.For<IEpdContactRepository>();
        var page = new Add { Item = new EpdContactCommand() };
        page.ModelState.AddModelError("key", "message");

        var result = await page.OnPostAsync(repo);

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
        }
    }
}
