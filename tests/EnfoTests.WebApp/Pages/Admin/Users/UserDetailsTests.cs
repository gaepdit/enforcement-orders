using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Domain.Resources.Users;
using Enfo.Domain.Services;
using Enfo.WebApp.Pages.Admin.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace EnfoTests.WebApp.Pages.Admin.Users
{
    public class UserDetailsTests
    {
        [Fact]
        public async Task OnGet_PopulatesThePageModel()
        {
            var userView = new UserView(UserTestData.ApplicationUsers[0]);
            var roles = new List<string> {"abc"};

            var userService = new Mock<IUserService>();
            userService.Setup(l => l.GetUserByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(userView);
            userService.Setup(l => l.GetUserRolesAsync(It.IsAny<Guid>()))
                .ReturnsAsync(roles);
            var pageModel = new Details();

            var result = await pageModel.OnGetAsync(userService.Object, userView.Id);

            result.Should().BeOfType<PageResult>();
            pageModel.DisplayUser.Should().Be(userView);
            pageModel.Roles.Should().BeEquivalentTo(roles);
        }

        [Fact]
        public async Task OnGet_NonexistentIdReturnsNotFound()
        {
            var userService = new Mock<IUserService>();
            var pageModel = new Details();

            var result = await pageModel.OnGetAsync(userService.Object, Guid.Empty);

            result.Should().BeOfType<NotFoundResult>();
            pageModel.DisplayUser.ShouldBeNull();
            pageModel.Roles.ShouldBeNull();
        }

        [Fact]
        public async Task OnGet_MissingIdReturnsNotFound()
        {
            var userService = new Mock<IUserService>();
            var pageModel = new Details();

            var result = await pageModel.OnGetAsync(userService.Object, null);

            result.Should().BeOfType<NotFoundResult>();
            pageModel.DisplayUser.ShouldBeNull();
            pageModel.Roles.ShouldBeNull();
        }
    }
}