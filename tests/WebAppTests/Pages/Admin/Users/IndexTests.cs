using Enfo.AppServices.Staff;
using Enfo.WebApp.Pages.Admin.Users;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppTests.Pages.Admin.Users;

[TestFixture]
public class IndexTests
{
    [Test]
    [TestCase(null, null, null)]
    [TestCase("a", "b", "c")]
    public async Task OnSearch_IfValidModel_ReturnPage(string name, string email, string role)
    {
        var users = UserTestData.ApplicationUsers;
        var searchResults = users.Select(e => new StaffViewDto(e)).ToList();

        var staffService = Substitute.For<IStaffService>();
        staffService.SearchAsync(Arg.Any<StaffSearchDto>()).Returns(searchResults);
        var pageModel = new UsersIndexModel(staffService);
        var spec = new StaffSearchDto(name, email, role);

        var result = await pageModel.OnGetSearchAsync(spec);

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.ModelState.IsValid.Should().BeTrue();
            pageModel.SearchResults.Should().BeEquivalentTo(searchResults);
            pageModel.ShowResults.Should().BeTrue();
        }
    }

    [Test]
    public async Task OnSearch_IfInvalidModel_ReturnPageWithInvalidModelState()
    {
        var staffService = Substitute.For<IStaffService>();
        var pageModel = new UsersIndexModel(staffService);
        pageModel.ModelState.AddModelError("Error", "Sample error description");

        var result = await pageModel.OnGetSearchAsync(new StaffSearchDto(null, null, null));

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.ModelState.IsValid.Should().BeFalse();
        }
    }
}
