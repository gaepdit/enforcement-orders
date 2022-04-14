using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Domain.Resources.Users;
using Enfo.Domain.Services;
using Enfo.WebApp.Pages.Account;
using EnfoTests.WebApp.Pages.Admin.Users;
using FluentAssertions;
using Moq;
using Xunit;

namespace EnfoTests.WebApp.Pages.Account
{
    public class AccountIndexTests
    {
        [Fact]
        public async Task OnGet_PopulatesThePageModel()
        {
            var userView = new UserView(UserTestData.ApplicationUsers[0]);
            var roles = new List<string> {"abc"};

            var userService = new Mock<IUserService>();
            userService.Setup(l => l.GetCurrentUserAsync())
                .ReturnsAsync(userView);
            userService.Setup(l => l.GetCurrentUserRolesAsync())
                .ReturnsAsync(roles);
            var pageModel = new Index();

            await pageModel.OnGetAsync(userService.Object);

            pageModel.DisplayUser.Should().Be(userView);
            pageModel.Roles.Should().BeEquivalentTo(roles);
        }
    }
}