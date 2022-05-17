using Enfo.Domain.Users.Resources;
using Enfo.Domain.Users.Services;
using Enfo.WebApp.Pages.Admin.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnfoTests.WebApp.Pages.Admin.Users;

[TestFixture]
public class UserDetailsTests
{
    [Test]
    public async Task OnGet_PopulatesThePageModel()
    {
        var userView = new UserView(UserTestData.ApplicationUsers[0]);
        var roles = new List<string> { "abc" };

        var userService = new Mock<IUserService>();
        userService.Setup(l => l.GetUserByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(userView);
        userService.Setup(l => l.GetUserRolesAsync(It.IsAny<Guid>()))
            .ReturnsAsync(roles);
        var pageModel = new Details();

        var result = await pageModel.OnGetAsync(userService.Object, userView.Id);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<PageResult>();
            pageModel.DisplayUser.Should().Be(userView);
            pageModel.Roles.Should().BeEquivalentTo(roles);
        });
    }

    [Test]
    public async Task OnGet_NonexistentIdReturnsNotFound()
    {
        var userService = new Mock<IUserService>();
        var pageModel = new Details();

        var result = await pageModel.OnGetAsync(userService.Object, Guid.Empty);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<NotFoundResult>();
            pageModel.DisplayUser.Should().BeNull();
            pageModel.Roles.Should().BeNull();
        });
    }

    [Test]
    public async Task OnGet_MissingIdReturnsNotFound()
    {
        var userService = new Mock<IUserService>();
        var pageModel = new Details();

        var result = await pageModel.OnGetAsync(userService.Object, null);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<NotFoundResult>();
            pageModel.DisplayUser.Should().BeNull();
            pageModel.Roles.Should().BeNull();
        });
    }
}
