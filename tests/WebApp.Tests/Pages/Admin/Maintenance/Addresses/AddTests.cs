using System.Threading.Tasks;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources.Address;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages.Admin.Maintenance.Addresses;
using Enfo.WebApp.Platform.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace WebApp.Tests.Pages.Admin.Maintenance.Addresses
{
    public class AddTests
    {
        [Fact]
        public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
        {
            var item = new AddressCreate {City = "abc", State = "GA", Street = "123", PostalCode = "01234"};
            var repo = new Mock<IAddressRepository> {DefaultValue = DefaultValue.Mock};
            repo.Setup(l => l.CreateAsync(It.IsAny<AddressCreate>()))
                .ReturnsAsync(1);

            // Initialize Page TempData
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var page = new Add(repo.Object) {TempData = tempData, Item = item};

            var result = await page.OnPostAsync();

            var expected = new DisplayMessage(Context.Success,
                $"{Add.ThisOption.SingularName} successfully added.");
            page.TempData?.GetDisplayMessage().Should().BeEquivalentTo(expected);
            page.HighlightId.ShouldEqual(1);

            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult) result).PageName.ShouldEqual("Index");
        }

        [Fact]
        public async Task OnPost_GivenModelError_ReturnsPageWithModelError()
        {
            var repo = new Mock<IAddressRepository> {DefaultValue = DefaultValue.Mock};
            var page = new Add(repo.Object) {Item = new AddressCreate()};
            page.ModelState.AddModelError("key", "message");

            var result = await page.OnPostAsync();

            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.ShouldBeFalse();
        }
    }
}