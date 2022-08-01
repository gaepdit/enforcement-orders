using Enfo.Domain.EpdContacts.Repositories;
using Enfo.Domain.EpdContacts.Resources;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages.Admin.Maintenance.Contacts;
using Enfo.WebApp.Platform.RazorHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace EnfoTests.WebApp.Pages.Admin.Maintenance.Contacts;

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
        var repo = new Mock<IEpdContactRepository> { DefaultValue = DefaultValue.Mock };
        repo.Setup(l => l.CreateAsync(It.IsAny<EpdContactCommand>()))
            .ReturnsAsync(1);

        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        var page = new Add { TempData = tempData, Item = item };

        var result = await page.OnPostAsync(repo.Object);

        var expected = new DisplayMessage(Context.Success,
            $"{Add.ThisOption.SingularName} successfully added.");

        Assert.Multiple(() =>
        {
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expected);
            page.HighlightId.Should().Be(1);

            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        });
    }

    [Test]
    public async Task OnPost_GivenModelError_ReturnsPageWithModelError()
    {
        var repo = new Mock<IEpdContactRepository> { DefaultValue = DefaultValue.Mock };
        var page = new Add { Item = new EpdContactCommand() };
        page.ModelState.AddModelError("key", "message");

        var result = await page.OnPostAsync(repo.Object);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
        });
    }
}
