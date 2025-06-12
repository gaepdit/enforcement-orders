using Enfo.AppServices.Staff;
using Enfo.WebApp.Pages.Admin.Users;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NSubstitute;
using NUnit.Framework;

namespace EnfoTests.WebApp.Pages.Admin.Users;

[TestFixture]
public class UserDetailsTests
{
    [Test]
    public async Task OnGet_PopulatesThePageModel()
    {
        var userView = new StaffView(UserTestData.ApplicationUsers[0]);
        var roles = new List<string> { "abc" };

        var userService = Substitute.For<IStaffService>();
        userService.FindUserAsync(Arg.Any<Guid>()).Returns(userView);
        userService.GetUserRolesAsync(Arg.Any<Guid>()).Returns(roles);
        var pageModel = new Details();

        var result = await pageModel.OnGetAsync(userService, userView.Id);

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.DisplayStaff.Should().Be(userView);
            pageModel.Roles.Should().BeEquivalentTo(roles);
        }
    }

    [Test]
    public async Task OnGet_NonexistentIdReturnsNotFound()
    {
        var userService = Substitute.For<IStaffService>();
        var pageModel = new Details();

        var result = await pageModel.OnGetAsync(userService, Guid.Empty);

        using (new AssertionScope())
        {
            result.Should().BeOfType<NotFoundResult>();
            pageModel.DisplayStaff.Should().BeNull();
            pageModel.Roles.Should().BeNull();
        }
    }

    [Test]
    public async Task OnGet_MissingIdReturnsNotFound()
    {
        var userService = Substitute.For<IStaffService>();
        var pageModel = new Details();

        var result = await pageModel.OnGetAsync(userService, null);

        using (new AssertionScope())
        {
            result.Should().BeOfType<NotFoundResult>();
            pageModel.DisplayStaff.Should().BeNull();
            pageModel.Roles.Should().BeNull();
        }
    }
}
