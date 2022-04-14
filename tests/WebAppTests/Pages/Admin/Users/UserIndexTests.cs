using System.Linq;
using System.Threading.Tasks;
using Enfo.Domain.Resources.Users;
using Enfo.Domain.Services;
using Enfo.WebApp.Pages.Admin.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace EnfoTests.WebApp.Pages.Admin.Users
{
    public class UserIndexTests
    {
        [Theory]
        [InlineData(null, null, null)]
        [InlineData("a", "b", "c")]
        public async Task OnSearch_IfValidModel_ReturnPage(string name, string email, string role)
        {
            var users = UserTestData.ApplicationUsers;
            var searchResults = users.Select(e => new UserView(e)).ToList();

            var userService = new Mock<IUserService>();
            userService.Setup(l => l.GetUsersAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(searchResults);
            var pageModel = new Index();

            var result = await pageModel.OnGetSearchAsync(userService.Object, name, email, role);

            result.Should().BeOfType<PageResult>();
            pageModel.ModelState.IsValid.ShouldBeTrue();
            pageModel.SearchResults.Should().BeEquivalentTo(searchResults);
            pageModel.ShowResults.ShouldBeTrue();
        }

        [Fact]
        public async Task OnSearch_IfInvalidModel_ReturnPageWithInvalidModelState()
        {
            var userService = new Mock<IUserService>();
            var pageModel = new Index();
            pageModel.ModelState.AddModelError("Error", "Sample error description");

            var result = await pageModel.OnGetSearchAsync(userService.Object, null, null, null);

            result.Should().BeOfType<PageResult>();
            pageModel.ModelState.IsValid.ShouldBeFalse();
        }
    }
}