using Enfo.Domain.Users.Entities;
using Enfo.Domain.Users.Resources;
using Enfo.Domain.Users.Services;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages.Admin.Users;
using Enfo.WebApp.Platform.RazorHelpers;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnfoTests.WebApp.Pages.Admin.Users;

[TestFixture]
public class UserEditTests
{
    private readonly List<Edit.UserRoleSetting> _roleSettings = new()
    {
        new Edit.UserRoleSetting
        {
            Name = UserRole.OrderAdministrator,
            Description = UserRole.OrderAdministratorRole.Description,
            DisplayName = UserRole.OrderAdministratorRole.DisplayName,
            IsSelected = true,
        },
        new Edit.UserRoleSetting
        {
            Name = UserRole.SiteMaintenance,
            Description = UserRole.SiteMaintenanceRole.Description,
            DisplayName = UserRole.SiteMaintenanceRole.DisplayName,
            IsSelected = false,
        },
        new Edit.UserRoleSetting
        {
            Name = UserRole.UserMaintenance,
            Description = UserRole.UserMaintenanceRole.Description,
            DisplayName = UserRole.UserMaintenanceRole.DisplayName,
            IsSelected = false,
        },
    };

    [Test]
    public async Task OnGet_WithoutRoles_PopulatesThePageModel()
    {
        var userView = new UserView(UserTestData.ApplicationUsers[0]);

        var userService = Substitute.For<IUserService>();
        userService.GetUserByIdAsync(Arg.Any<Guid>()).Returns(userView);
        userService.GetUserRolesAsync(Arg.Any<Guid>()).Returns(new List<string>());
        var pageModel = new Edit(userService);

        var result = await pageModel.OnGetAsync(Guid.Empty);

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.UserId.Should().Be(Guid.Empty);
            pageModel.DisplayUser.Should().Be(userView);
            pageModel.UserRoleSettings.Should().NotBeEmpty();
            pageModel.UserRoleSettings.Count.Should().Be(3);
        }
    }

    [Test]
    public async Task OnGet_WithRoles_PopulatesThePageModel()
    {
        var userView = new UserView(UserTestData.ApplicationUsers[0]);
        var roles = new List<string> { UserRole.OrderAdministrator };

        var userService = Substitute.For<IUserService>();
        userService.GetUserByIdAsync(Arg.Any<Guid>()).Returns(userView);
        userService.GetUserRolesAsync(Arg.Any<Guid>()).Returns(roles);
        var pageModel = new Edit(userService);

        var result = await pageModel.OnGetAsync(Guid.Empty);

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.UserId.Should().Be(Guid.Empty);
            pageModel.DisplayUser.Should().Be(userView);
            pageModel.UserRoleSettings.Should().BeEquivalentTo(_roleSettings);
        }
    }

    [Test]
    public async Task OnGet_MissingId_ReturnsNotFound()
    {
        var userService = Substitute.For<IUserService>();
        var pageModel = new Edit(userService);

        var result = await pageModel.OnGetAsync(null);

        using (new AssertionScope())
        {
            result.Should().BeOfType<NotFoundResult>();
            pageModel.UserId.Should().Be(Guid.Empty);
            pageModel.DisplayUser.Should().BeNull();
            pageModel.UserRoleSettings.Should().BeNull();
        }
    }

    [Test]
    public async Task OnGet_NonexistentId_ReturnsNotFound()
    {
        var userService = Substitute.For<IUserService>();
        userService.GetUserByIdAsync(Arg.Any<Guid>()).Returns((UserView)null);
        var pageModel = new Edit(userService);

        var result = await pageModel.OnGetAsync(Guid.Empty);

        using (new AssertionScope())
        {
            result.Should().BeOfType<NotFoundResult>();
            pageModel.UserId.Should().Be(Guid.Empty);
            pageModel.DisplayUser.Should().BeNull();
            pageModel.UserRoleSettings.Should().BeNull();
        }
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var userService = Substitute.For<IUserService>();
        userService.UpdateUserRolesAsync(Arg.Any<Guid>(), Arg.Any<Dictionary<string, bool>>())
            .Returns(IdentityResult.Success);
        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Substitute.For<ITempDataProvider>());
        var pageModel = new Edit(userService)
        {
            TempData = tempData,
            UserId = Guid.Empty,
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
        var userService = Substitute.For<IUserService>();
        userService.GetUserByIdAsync(Arg.Any<Guid>()).Returns(new UserView(UserTestData.ApplicationUsers[0]));
        userService.GetUserRolesAsync(Arg.Any<Guid>()).Returns(new List<string>());

        var pageModel = new Edit(userService) { UserRoleSettings = new List<Edit.UserRoleSetting>() };
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
        var userView = new UserView(UserTestData.ApplicationUsers[0]);
        var identityResult = IdentityResult.Failed(new IdentityError { Code = "CODE", Description = "DESCRIPTION" });

        var userService = Substitute.For<IUserService>();
        userService.UpdateUserRolesAsync(Arg.Any<Guid>(), Arg.Any<Dictionary<string, bool>>()).Returns(identityResult);
        userService.GetUserByIdAsync(Arg.Any<Guid>()).Returns(userView);
        var pageModel = new Edit(userService) { UserRoleSettings = _roleSettings };

        var result = await pageModel.OnPostAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.ModelState.IsValid.Should().BeFalse();
            pageModel.ModelState[string.Empty]!.Errors[0].ErrorMessage.Should().Be("CODE: DESCRIPTION");
        }
    }
}
