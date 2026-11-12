using Enfo.AppServices.Staff;
using Enfo.Domain.Identity;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages.Admin.Users;
using Enfo.WebApp.Platform.PageModelHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebAppTests.Pages.Admin.Users;

[TestFixture]
public class EditRolesTests
{
    private readonly List<EditRolesModel.RoleSetting> _roleSettings =
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
        var userView = new StaffViewDto(UserTestData.ApplicationUsers[0]);

        var userService = Substitute.For<IStaffService>();
        userService.FindAsync(Arg.Any<Guid>()).Returns(userView);
        userService.GetAppRolesAsync(Arg.Any<Guid>()).Returns([]);

        var userId = Guid.NewGuid();
        var pageModel = new EditRolesModel(userService) { Id = userId };

        var result = await pageModel.OnGetAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.Id.Should().Be(userId);
            pageModel.DisplayStaff.Should().Be(userView);
            pageModel.RoleSettings.Should().NotBeEmpty();
            pageModel.RoleSettings.Should().HaveCount(3);
        }
    }

    [Test]
    public async Task OnGet_WithRoles_PopulatesThePageModel()
    {
        var userView = new StaffViewDto(UserTestData.ApplicationUsers[0]);

        var userService = Substitute.For<IStaffService>();
        userService.FindAsync(Arg.Any<Guid>()).Returns(userView);
        userService.GetRolesAsync(Arg.Any<Guid>()).Returns(new List<string> { AppRole.OrderAdministrator });

        var userId = Guid.NewGuid();
        var pageModel = new EditRolesModel(userService) { Id = userId };

        var result = await pageModel.OnGetAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.Id.Should().Be(userId);
            pageModel.DisplayStaff.Should().Be(userView);
            pageModel.RoleSettings.Should().BeEquivalentTo(_roleSettings);
        }
    }

    [Test]
    public async Task OnGet_MissingId_ReturnsRedirect()
    {
        var userService = Substitute.For<IStaffService>();
        var pageModel = new EditRolesModel(userService) { Id = null };

        var result = await pageModel.OnGetAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<RedirectToPageResult>();
            pageModel.Id.Should().Be(null);
            pageModel.DisplayStaff.Should().BeNull();
            pageModel.RoleSettings.Should().BeEmpty();
        }
    }

    [Test]
    public async Task OnGet_NonexistentId_ReturnsNotFound()
    {
        var userService = Substitute.For<IStaffService>();
        userService.FindAsync(Arg.Any<Guid>()).Returns((StaffViewDto)null);

        var userId = Guid.NewGuid();
        var pageModel = new EditRolesModel(userService) { Id = userId };

        var result = await pageModel.OnGetAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<NotFoundResult>();
            pageModel.Id.Should().Be(userId);
            pageModel.DisplayStaff.Should().BeNull();
            pageModel.RoleSettings.Should().BeEmpty();
        }
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var userService = Substitute.For<IStaffService>();
        userService.UpdateRolesAsync(Arg.Any<Guid>(), Arg.Any<Dictionary<string, bool>>())
            .Returns(IdentityResult.Success);
        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Substitute.For<ITempDataProvider>());
        var pageModel = new EditRolesModel(userService)
        {
            TempData = tempData,
            Id = Guid.Empty,
            RoleSettings = _roleSettings,
        };

        var result = await pageModel.OnPostAsync();

        using (new AssertionScope())
        {
            pageModel.ModelState.IsValid.Should().BeTrue();
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Details");
            ((RedirectToPageResult)result).RouteValues!["id"].Should().Be(Guid.Empty);
            var expectedMessage =
                new DisplayMessage(DisplayMessage.AlertContext.Success, "User roles successfully updated.");
            pageModel.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
        }
    }

    [Test]
    public async Task OnPost_UpdateRolesFails_ReturnsPageWithInvalidModelState()
    {
        var userView = new StaffViewDto(UserTestData.ApplicationUsers[0]);
        var identityResult = IdentityResult.Failed(new IdentityError { Code = "CODE", Description = "DESCRIPTION" });

        var userService = Substitute.For<IStaffService>();
        userService.UpdateRolesAsync(Arg.Any<Guid>(), Arg.Any<Dictionary<string, bool>>()).Returns(identityResult);
        userService.FindAsync(Arg.Any<Guid>()).Returns(userView);
        var pageModel = new EditRolesModel(userService) { RoleSettings = _roleSettings, Id = Guid.NewGuid() };

        var result = await pageModel.OnPostAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.ModelState.IsValid.Should().BeFalse();
            pageModel.ModelState[string.Empty]!.Errors[0].ErrorMessage.Should().Be("CODE: DESCRIPTION");
        }
    }
}
