using Enfo.Domain.Users.Resources;
using Enfo.Domain.Users.Services;
using Enfo.WebApp.Pages.Account;
using EnfoTests.WebApp.Pages.Admin.Users;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnfoTests.WebApp.Pages.Account;

[TestFixture]
public class AccountIndexTests
{
    [Test]
    public async Task OnGet_PopulatesThePageModel()
    {
        var userView = new UserView(UserTestData.ApplicationUsers[0]);
        var roles = new List<string> { "abc" };

        var userService = new Mock<IUserService>();
        userService.Setup(l => l.GetCurrentUserAsync())
            .ReturnsAsync(userView);
        userService.Setup(l => l.GetCurrentUserRolesAsync())
            .ReturnsAsync(roles);
        var pageModel = new Index();

        await pageModel.OnGetAsync(userService.Object);

        Assert.Multiple(() =>
        {
            pageModel.DisplayUser.Should().Be(userView);
            pageModel.Roles.Should().BeEquivalentTo(roles);
        });
    }
}
