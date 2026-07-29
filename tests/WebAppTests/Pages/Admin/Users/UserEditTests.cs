using Enfo.AppServices.Staff;
using Enfo.Domain.Users;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages.Admin.Users;
using Enfo.WebApp.Platform.RazorHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebAppTests.Pages.Admin.Users;

[TestFixture]
public class UserEditTests
{
    private readonly List<Edit.UserRoleSetting> _roleSettings =
    [
        new()
        {
            Name = AppRole.OrderAdministrator,
            Description = AppRole.OrderAdministratorRole.Description,
            DisplayName = AppRole.OrderAdministratorRole.DisplayName,
            IsSelected = true,
        },

        new()
        {
            Name = AppRole.SiteMaintenance,
            Description = AppRole.SiteMaintenanceRole.Description,
            DisplayName = AppRole.SiteMaintenanceRole.DisplayName,
            IsSelected = false,
        },

        new()
        {
            Name = AppRole.UserMaintenance,
            Description = AppRole.UserMaintenanceRole.Description,
            DisplayName = AppRole.UserMaintenanceRole.DisplayName,
            IsSelected = false,
        },
    ];

    [Test]
    public async Task OnGet_WithoutRoles_PopulatesThePageModel()
    {
        var userView = new StaffView(UserTestData.ApplicationUsers[0]);

        var userService = Substitute.For<IStaffService>();
        userService.FindUserAsync(Arg.Any<Guid>()).Returns(userView);
        userService.GetUserRolesAsync(Arg.Any<Guid>()).Returns(new List<string>());

        var userId = Guid.NewGuid();
        var pageModel = new Edit(userService) { Id = userId };

        var result = await pageModel.OnGetAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.Id.Should().Be(userId);
            pageModel.DisplayStaff.Should().Be(userView);
            pageModel.UserRoleSettings.Should().NotBeEmpty();
            pageModel.UserRoleSettings.Should().HaveCount(3);
        }
    }

    [Test]
    public async Task OnGet_WithRoles_PopulatesThePageModel()
    {
        var userView = new StaffView(UserTestData.ApplicationUsers[0]);
        var roles = new List<string> { AppRole.OrderAdministrator };

        var userService = Substitute.For<IStaffService>();
        userService.FindUserAsync(Arg.Any<Guid>()).Returns(userView);
        userService.GetUserRolesAsync(Arg.Any<Guid>()).Returns(roles);

        var userId = Guid.NewGuid();
        var pageModel = new Edit(userService) { Id = userId };

        var result = await pageModel.OnGetAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.Id.Should().Be(userId);
            pageModel.DisplayStaff.Should().Be(userView);
            pageModel.UserRoleSettings.Should().BeEquivalentTo(_roleSettings);
        }
    }

    [Test]
    public async Task OnGet_MissingId_ReturnsRedirect()
    {
        var userService = Substitute.For<IStaffService>();
        var pageModel = new Edit(userService) { Id = null };

        var result = await pageModel.OnGetAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<RedirectToPageResult>();
            pageModel.Id.Should().Be(null);
            pageModel.DisplayStaff.Should().BeNull();
            pageModel.UserRoleSettings.Should().BeNull();
        }
    }

    [Test]
    public async Task OnGet_NonexistentId_ReturnsNotFound()
    {
        var userService = Substitute.For<IStaffService>();
        userService.FindUserAsync(Arg.Any<Guid>()).Returns((StaffView)null);

        var userId = Guid.NewGuid();
        var pageModel = new Edit(userService) { Id = userId };

        var result = await pageModel.OnGetAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<NotFoundResult>();
            pageModel.Id.Should().Be(userId);
            pageModel.DisplayStaff.Should().BeNull();
            pageModel.UserRoleSettings.Should().BeNull();
        }
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var userService = Substitute.For<IStaffService>();
        userService.UpdateUserRolesAsync(Arg.Any<Guid>(), Arg.Any<Dictionary<string, bool>>())
            .Returns(IdentityResult.Success);
        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Substitute.For<ITempDataProvider>());
        var pageModel = new Edit(userService)
        {
            TempData = tempData,
            Id = Guid.Empty,
            UserRoleSettings = _roleSettings,
        };

        var result = await pageModel.OnPostAsync();

        using (new AssertionScope())
        {
            pageModel.ModelState.IsValid.Should().BeTrue();
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Details");
            ((RedirectToPageResult)result).RouteValues!["id"].Should().Be(Guid.Empty);
            var expectedMessage = new DisplayMessage(Context.Success, "User roles successfully updated.");
            pageModel.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
        }
    }

    [Test]
    public async Task OnPost_InvalidModel_ReturnsPageWithInvalidModelState()
    {
        var userService = Substitute.For<IStaffService>();
        userService.FindUserAsync(Arg.Any<Guid>()).Returns(new StaffView(UserTestData.ApplicationUsers[0]));
        userService.GetUserRolesAsync(Arg.Any<Guid>()).Returns(new List<string>());

        var pageModel = new Edit(userService) { UserRoleSettings = [], Id = Guid.NewGuid() };
        pageModel.ModelState.AddModelError("Error", "Sample error description");

        var result = await pageModel.OnPostAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.ModelState.IsValid.Should().BeFalse();
            pageModel.ModelState["Error"]!.Errors[0].ErrorMessage.Should().Be("Sample error description");
        }
    }

    [Test]
    public async Task OnPost_UpdateRolesFails_ReturnsPageWithInvalidModelState()
    {
        var userView = new StaffView(UserTestData.ApplicationUsers[0]);
        var identityResult = IdentityResult.Failed(new IdentityError { Code = "CODE", Description = "DESCRIPTION" });

        var userService = Substitute.For<IStaffService>();
        userService.UpdateUserRolesAsync(Arg.Any<Guid>(), Arg.Any<Dictionary<string, bool>>()).Returns(identityResult);
        userService.FindUserAsync(Arg.Any<Guid>()).Returns(userView);
        var pageModel = new Edit(userService) { UserRoleSettings = _roleSettings, Id = Guid.NewGuid() };

        var result = await pageModel.OnPostAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.ModelState.IsValid.Should().BeFalse();
            pageModel.ModelState[string.Empty]!.Errors[0].ErrorMessage.Should().Be("CODE: DESCRIPTION");
        }
    }
}
