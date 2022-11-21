using Enfo.Domain.Users.Resources;
using Enfo.Domain.Users.Services;
using Enfo.WebApp.Pages.Admin.Users;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.WebApp.Pages.Admin.Users;

[TestFixture]
public class UserIndexTests
{
    [Test]
    [TestCase(null, null, null)]
    [TestCase("a", "b", "c")]
    public async Task OnSearch_IfValidModel_ReturnPage(string name, string email, string role)
    {
        var users = UserTestData.ApplicationUsers;
        var searchResults = users.Select(e => new UserView(e)).ToList();

        var userService = new Mock<IUserService>();
        userService.Setup(l => l.GetUsersAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(searchResults);
        var pageModel = new Index();

        var result = await pageModel.OnGetSearchAsync(userService.Object, name, email, role);

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.ModelState.IsValid.Should().BeTrue();
            pageModel.SearchResults.Should().BeEquivalentTo(searchResults);
            pageModel.ShowResults.Should().BeTrue();
        }
    }

    [Test]
    public async Task OnSearch_IfInvalidModel_ReturnPageWithInvalidModelState()
    {
        var userService = new Mock<IUserService>();
        var pageModel = new Index();
        pageModel.ModelState.AddModelError("Error", "Sample error description");

        var result = await pageModel.OnGetSearchAsync(userService.Object, null, null, null);

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.ModelState.IsValid.Should().BeFalse();
        }
    }
}
