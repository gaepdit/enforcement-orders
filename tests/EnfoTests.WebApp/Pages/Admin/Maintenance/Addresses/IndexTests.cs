using System.Threading.Tasks;
using Enfo.Domain.Repositories;
using Enfo.Domain.Resources.Address;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages.Admin.Maintenance.Addresses;
using Enfo.WebApp.Platform.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;
using Xunit.Extensions.AssertExtensions;
using static EnfoTests.Helpers.ResourceHelper;

namespace EnfoTests.WebApp.Pages.Admin.Maintenance.Addresses
{
    public class IndexTests
    {
        [Fact]
        public async Task OnGet_ReturnsWithOrder()
        {
            var list = GetAddressViewList();
            var repo = new Mock<IAddressRepository>();
            repo.Setup(l => l.ListAsync(true)).ReturnsAsync(list);
            var page = new Index(repo.Object);

            await page.OnGetAsync();

            page.Items.Should().BeEquivalentTo(list);
            page.Message.ShouldBeNull();
        }

        [Fact]
        public async Task SetDisplayMessage_ReturnsWithDisplayMessage()
        {
            var repo = new Mock<IAddressRepository> {DefaultValue = DefaultValue.Mock};

            // Initialize Page TempData
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var page = new Index(repo.Object) {TempData = tempData};

            page.TempData.SetDisplayMessage(Context.Info, "Info message");
            await page.OnGetAsync();

            var expected = new DisplayMessage(Context.Info, "Info message");
            page.Message.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task OnPost_ReturnsRedirectWithDisplayMessage()
        {
            var item = GetAddressViewList()[0];
            var repo = new Mock<IAddressRepository> {DefaultValue = DefaultValue.Mock};
            repo.Setup(l => l.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(item);

            // Initialize Page TempData
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var page = new Index(repo.Object) {TempData = tempData};

            var result = await page.OnPostAsync(item.Id);

            var expected = new DisplayMessage(Context.Success,
                $"{Index.ThisOption.SingularName} successfully {(item.Active ? "deactivated" : "restored")}.");
            page.TempData?.GetDisplayMessage().Should().BeEquivalentTo(expected);

            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult) result).PageName.ShouldEqual("Index");
        }

        [Fact]
        public async Task OnPost_GivenNullId_ReturnsBadRequest()
        {
            var repo = new Mock<IAddressRepository> {DefaultValue = DefaultValue.Mock};
            var page = new Index(repo.Object);

            var result = await page.OnPostAsync(null);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task OnPost_GivenInvalidId_ReturnsNotFound()
        {
            var repo = new Mock<IAddressRepository> {DefaultValue = DefaultValue.Mock};
            repo.Setup(l => l.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(null as AddressView);
            var page = new Index(repo.Object);

            var result = await page.OnPostAsync(1);

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}