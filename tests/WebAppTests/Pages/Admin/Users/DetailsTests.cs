using Enfo.AppServices.Staff;
using Enfo.Domain.Identity;
using Enfo.WebApp.Pages.Admin.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppTests.Pages.Admin.Users;

[TestFixture]
public class DetailsTests
{
    [Test]
    public async Task OnGet_PopulatesThePageModel()
    {
        var userView = new StaffViewDto(UserTestData.ApplicationUsers[0]);
        IReadOnlyList<AppRole> roles = [AppRole.SiteMaintenanceRole];

        var userService = Substitute.For<IStaffService>();
        userService.FindAsync(Arg.Any<Guid>()).Returns(userView);
        userService.GetAppRolesAsync(Arg.Any<Guid>()).Returns(roles);
        var pageModel = new DetailsModel();

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
        var pageModel = new DetailsModel();

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
        var pageModel = new DetailsModel();

        var result = await pageModel.OnGetAsync(userService, null);

        using (new AssertionScope())
        {
            result.Should().BeOfType<RedirectToPageResult>();
            pageModel.DisplayStaff.Should().BeNull();
            pageModel.Roles.Should().BeNull();
        }
    }
}
