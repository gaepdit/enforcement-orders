using Enfo.Domain.Users.Resources;
using Enfo.Domain.Users.Services;
using Enfo.WebApp.Pages.Account;
using EnfoTests.WebApp.Pages.Admin.Users;
using FluentAssertions;
using FluentAssertions.Execution;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Index = Enfo.WebApp.Pages.Account.Index;

namespace EnfoTests.WebApp.Pages.Account;

[TestFixture]
public class AccountIndexTests
{
    [Test]
    public async Task OnGet_PopulatesThePageModel()
    {
        var userView = new UserView(UserTestData.ApplicationUsers[0]);
        var roles = new List<string> { "abc" };

        var userService = Substitute.For<IUserService>();
        userService.GetCurrentUserAsync().Returns(userView);
        userService.GetCurrentUserRolesAsync().Returns(roles);
        var pageModel = new Index();

        await pageModel.OnGetAsync(userService);

        using (new AssertionScope())
        {
            pageModel.DisplayUser.Should().Be(userView);
            pageModel.Roles.Should().BeEquivalentTo(roles);
        }
    }
}
