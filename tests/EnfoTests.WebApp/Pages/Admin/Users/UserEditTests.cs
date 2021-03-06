﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Domain.Entities.Users;
using Enfo.Domain.Resources.Users;
using Enfo.Domain.Services;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages.Admin.Users;
using Enfo.WebApp.Platform.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace EnfoTests.WebApp.Pages.Admin.Users
{
    public class UserEditTests
    {
        private readonly List<Edit.UserRoleSetting> _roleSettings = new()
        {
            new Edit.UserRoleSetting
            {
                Name = UserRole.OrderAdministrator,
                Description = UserRole.OrderAdministratorRole.Description,
                DisplayName = UserRole.OrderAdministratorRole.DisplayName,
                IsSelected = true
            },
            new Edit.UserRoleSetting
            {
                Name = UserRole.SiteMaintenance,
                Description = UserRole.SiteMaintenanceRole.Description,
                DisplayName = UserRole.SiteMaintenanceRole.DisplayName,
                IsSelected = false
            },
            new Edit.UserRoleSetting
            {
                Name = UserRole.UserMaintenance,
                Description = UserRole.UserMaintenanceRole.Description,
                DisplayName = UserRole.UserMaintenanceRole.DisplayName,
                IsSelected = false
            },
        };

        [Fact]
        public async Task OnGet_WithoutRoles_PopulatesThePageModel()
        {
            var userView = new UserView(UserTestData.ApplicationUsers[0]);

            var userService = new Mock<IUserService>();
            userService.Setup(l => l.GetUserByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(userView);
            userService.Setup(l => l.GetUserRolesAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new List<string>());
            var pageModel = new Edit(userService.Object);

            var result = await pageModel.OnGetAsync(Guid.Empty);

            result.Should().BeOfType<PageResult>();
            pageModel.UserId.ShouldEqual(Guid.Empty);
            pageModel.DisplayUser.ShouldEqual(userView);
            pageModel.UserRoleSettings.ShouldNotBeEmpty();
            pageModel.UserRoleSettings.Count.ShouldEqual(3);
        }

        [Fact]
        public async Task OnGet_WithRoles_PopulatesThePageModel()
        {
            var userView = new UserView(UserTestData.ApplicationUsers[0]);
            var roles = new List<string> {UserRole.OrderAdministrator};

            var userService = new Mock<IUserService>();
            userService.Setup(l => l.GetUserByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(userView);
            userService.Setup(l => l.GetUserRolesAsync(It.IsAny<Guid>()))
                .ReturnsAsync(roles);
            var pageModel = new Edit(userService.Object);

            var result = await pageModel.OnGetAsync(Guid.Empty);

            result.Should().BeOfType<PageResult>();
            pageModel.UserId.ShouldEqual(Guid.Empty);
            pageModel.DisplayUser.ShouldEqual(userView);
            pageModel.UserRoleSettings.Should().BeEquivalentTo(_roleSettings);
        }

        [Fact]
        public async Task OnGet_MissingId_ReturnsNotFound()
        {
            var userService = new Mock<IUserService>();
            var pageModel = new Edit(userService.Object);

            var result = await pageModel.OnGetAsync(null);

            result.Should().BeOfType<NotFoundResult>();
            pageModel.UserId.ShouldEqual(Guid.Empty);
            pageModel.DisplayUser.ShouldBeNull();
            pageModel.UserRoleSettings.ShouldBeNull();
        }

        [Fact]
        public async Task OnGet_NonexistentId_ReturnsNotFound()
        {
            var userService = new Mock<IUserService>();
            userService.Setup(l => l.GetUserByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((UserView) null);
            var pageModel = new Edit(userService.Object);

            var result = await pageModel.OnGetAsync(Guid.Empty);

            result.Should().BeOfType<NotFoundResult>();
            pageModel.UserId.Should().Be(Guid.Empty);
            pageModel.DisplayUser.ShouldBeNull();
            pageModel.UserRoleSettings.ShouldBeNull();
        }

        [Fact]
        public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
        {
            var userService = new Mock<IUserService>();
            userService.Setup(l => l.UpdateUserRolesAsync(It.IsAny<Guid>(), It.IsAny<Dictionary<string, bool>>()))
                .ReturnsAsync(IdentityResult.Success);
            // Initialize Page TempData
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var pageModel = new Edit(userService.Object)
            {
                TempData = tempData,
                UserId = Guid.Empty,
                UserRoleSettings = _roleSettings
            };

            var result = await pageModel.OnPostAsync();

            pageModel.ModelState.IsValid.ShouldBeTrue();
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult) result).PageName.ShouldEqual("Details");
            ((RedirectToPageResult) result).RouteValues["id"].ShouldEqual(Guid.Empty);
            var expectedMessage = new DisplayMessage(Context.Success, "User roles successfully updated.");
            pageModel.TempData?.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
        }

        [Fact]
        public async Task OnPost_InvalidModel_ReturnsPageWithInvalidModelState()
        {
            var userService = new Mock<IUserService>();
            var pageModel = new Edit(userService.Object) {UserRoleSettings = new List<Edit.UserRoleSetting>()};
            pageModel.ModelState.AddModelError("Error", "Sample error description");

            var result = await pageModel.OnPostAsync();

            result.Should().BeOfType<PageResult>();
            pageModel.ModelState.IsValid.ShouldBeFalse();
            pageModel.ModelState["Error"].Errors[0].ErrorMessage.Should().Be("Sample error description");
        }

        [Fact]
        public async Task OnPost_UpdateRolesFails_ReturnsPageWithInvalidModelState()
        {
            var userView = new UserView(UserTestData.ApplicationUsers[0]);
            var identityResult = IdentityResult.Failed(new IdentityError {Code = "CODE", Description = "DESCRIPTION"});

            var userService = new Mock<IUserService> {DefaultValue = DefaultValue.Mock};
            userService.Setup(l => l.UpdateUserRolesAsync(It.IsAny<Guid>(), It.IsAny<Dictionary<string, bool>>()))
                .ReturnsAsync(identityResult);
            userService.Setup(l => l.GetUserByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(userView);
            var pageModel = new Edit(userService.Object) {UserRoleSettings = _roleSettings};

            var result = await pageModel.OnPostAsync();

            result.Should().BeOfType<PageResult>();
            pageModel.ModelState.IsValid.ShouldBeFalse();
            pageModel.ModelState[string.Empty].Errors[0].ErrorMessage.Should().Be("CODE: DESCRIPTION");
        }
    }
}