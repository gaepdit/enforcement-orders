using Enfo.AppServices.Staff;
using Enfo.WebApp.Pages.Account;
using WebAppTests.Pages.Admin.Users;

namespace WebAppTests.Pages.Account;

[TestFixture]
public class AccountIndexTests
{
    [Test]
    public async Task OnGet_PopulatesThePageModel()
    {
        var userView = new StaffView(UserTestData.ApplicationUsers[0]);
        var roles = new List<string> { "abc" };

        var userService = Substitute.For<IStaffService>();
        userService.GetCurrentUserAsync().Returns(userView);
        userService.GetCurrentUserRolesAsync().Returns(roles);
        var pageModel = new IndexModel(userService);

        await pageModel.OnGetAsync();

        using (new AssertionScope())
        {
            pageModel.DisplayStaff.Should().Be(userView);
            pageModel.Roles.Should().BeEquivalentTo(roles);
        }
    }
}
