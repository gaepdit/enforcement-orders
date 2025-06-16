using Enfo.AppServices.Staff;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Index = Enfo.WebApp.Pages.Admin.Users.Index;

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
        var searchResults = users.Select(e => new StaffView(e)).ToList();

        var userService = Substitute.For<IStaffService>();
        userService.SearchUsersAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(searchResults);
        var pageModel = new Index();

        var result = await pageModel.OnGetSearchAsync(userService, name, email, role);

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
        var userService = Substitute.For<IStaffService>();
        var pageModel = new Index();
        pageModel.ModelState.AddModelError("Error", "Sample error description");

        var result = await pageModel.OnGetSearchAsync(userService, null, null, null);

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.ModelState.IsValid.Should().BeFalse();
        }
    }
}
