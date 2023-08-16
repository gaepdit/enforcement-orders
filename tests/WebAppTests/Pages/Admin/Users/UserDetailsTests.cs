using Enfo.Domain.Users.Resources;
using Enfo.Domain.Users.Services;
using Enfo.WebApp.Pages.Admin.Users;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NSubstitute;
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

        var userService = Substitute.For<IUserService>();
        userService.GetUserByIdAsync(Arg.Any<Guid>()).Returns(userView);
        userService.GetUserRolesAsync(Arg.Any<Guid>()).Returns(roles);
        var pageModel = new Details();

        var result = await pageModel.OnGetAsync(userService, userView.Id);

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.DisplayUser.Should().Be(userView);
            pageModel.Roles.Should().BeEquivalentTo(roles);
        }
    }

    [Test]
    public async Task OnGet_NonexistentIdReturnsNotFound()
    {
        var userService = Substitute.For<IUserService>();
        var pageModel = new Details();

        var result = await pageModel.OnGetAsync(userService, Guid.Empty);

        using (new AssertionScope())
        {
            result.Should().BeOfType<NotFoundResult>();
            pageModel.DisplayUser.Should().BeNull();
            pageModel.Roles.Should().BeNull();
        }
    }

    [Test]
    public async Task OnGet_MissingIdReturnsNotFound()
    {
        var userService = Substitute.For<IUserService>();
        var pageModel = new Details();

        var result = await pageModel.OnGetAsync(userService, null);

        using (new AssertionScope())
        {
            result.Should().BeOfType<NotFoundResult>();
            pageModel.DisplayUser.Should().BeNull();
            pageModel.Roles.Should().BeNull();
        }
    }
}
