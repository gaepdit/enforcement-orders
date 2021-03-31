using System.Linq;
using System.Threading.Tasks;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources.LegalAuthority;
using Enfo.WebApp.Extensions;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages.Admin.Maintenance.LegalAuthority;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;
using Xunit.Extensions.AssertExtensions;
using static TestHelpers.ResourceHelper;

namespace WebApp.Tests.Pages.Admin.Maintenance.LegalAuthority
{
    public class EditTests
    {
        [Fact]
        public async Task OnGet_ReturnsWithItem()
        {
            var item = GetLegalAuthorityViewList()[0];
            var repo = new Mock<ILegalAuthorityRepository>();
            repo.Setup(l => l.GetAsync(item.Id)).ReturnsAsync(item);
            var page = new Edit(repo.Object);

            await page.OnGetAsync(item.Id);

            page.Item.Should().BeEquivalentTo(new LegalAuthorityUpdate(item));
            page.Id.ShouldEqual(item.Id);
            page.OriginalName.ShouldEqual(item.AuthorityName);
        }

        [Fact]
        public async Task OnGet_GivenNullId_ReturnsNotFound()
        {
            var repo = new Mock<ILegalAuthorityRepository>();
            var page = new Edit(repo.Object);

            var result = await page.OnGetAsync(null);

            result.Should().BeOfType<NotFoundResult>();
            page.Item.ShouldBeNull();
        }

        [Fact]
        public async Task OnGet_GivenInvalidId_ReturnsNotFound()
        {
            var repo = new Mock<ILegalAuthorityRepository>();
            repo.Setup(l => l.GetAsync(It.IsAny<int>())).ReturnsAsync(null as LegalAuthorityView);
            var page = new Edit(repo.Object);

            var result = await page.OnGetAsync(-1);

            result.Should().BeOfType<NotFoundObjectResult>();
            page.Item.ShouldBeNull();
        }

        [Fact]
        public async Task OnGet_GivenInactiveItem_RedirectsWithDisplayMessage()
        {
            var item = GetLegalAuthorityViewList().Single(e => !e.Active);
            var repo = new Mock<ILegalAuthorityRepository>();
            repo.Setup(l => l.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(item);

            // Initialize Page TempData
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var page = new Edit(repo.Object) {TempData = tempData};

            var result = await page.OnGetAsync(item.Id);

            var expected = new DisplayMessage(Context.Warning,
                $"Inactive {Edit.ThisOption.PluralName} cannot be edited.");
            page.TempData?.GetDisplayMessage().Should().BeEquivalentTo(expected);

            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult) result).PageName.ShouldEqual("Index");
        }


        [Fact]
        public async Task OnPost_GivenInvalidId_ReturnsNotFound()
        {
            var repo = new Mock<ILegalAuthorityRepository>();
            repo.Setup(l => l.GetAsync(It.IsAny<int>())).ReturnsAsync(null as LegalAuthorityView);
            var page = new Edit(repo.Object) {Id = 0};

            var result = await page.OnPostAsync();

            result.Should().BeOfType<NotFoundResult>();
            page.Item.ShouldBeNull();
        }

        [Fact]
        public async Task OnPost_GivenInactiveItem_RedirectsWithDisplayMessage()
        {
            var item = GetLegalAuthorityViewList().Single(e => !e.Active);
            var repo = new Mock<ILegalAuthorityRepository>();
            repo.Setup(l => l.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(item);

            // Initialize Page TempData
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var page = new Edit(repo.Object) {TempData = tempData, Id = 0};

            var result = await page.OnPostAsync();

            var expected = new DisplayMessage(Context.Warning,
                $"Inactive {Edit.ThisOption.PluralName} cannot be edited.");
            page.TempData?.GetDisplayMessage().Should().BeEquivalentTo(expected);

            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult) result).PageName.ShouldEqual("Index");
        }

        [Fact]
        public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
        {
            var item = new LegalAuthorityUpdate(GetLegalAuthorityViewList()[0]);
            var repo = new Mock<ILegalAuthorityRepository> {DefaultValue = DefaultValue.Mock};
            repo.Setup(l => l.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(GetLegalAuthorityViewList()[0]);
            repo.Setup(l => l.NameExistsAsync(It.IsAny<string>(), null))
                .ReturnsAsync(false);

            // Initialize Page TempData
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var page = new Edit(repo.Object) {TempData = tempData, Item = item};

            var result = await page.OnPostAsync();

            var expected = new DisplayMessage(Context.Success,
                $"{item.AuthorityName} successfully updated.");
            page.TempData?.GetDisplayMessage().Should().BeEquivalentTo(expected);

            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult) result).PageName.ShouldEqual("Index");
        }

        [Fact]
        public async Task OnPost_GivenModelError_ReturnsPageWithModelError()
        {
            var repo = new Mock<ILegalAuthorityRepository> {DefaultValue = DefaultValue.Mock};
            repo.Setup(l => l.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(GetLegalAuthorityViewList()[0]);
            var page = new Edit(repo.Object);
            page.ModelState.AddModelError("key", "message");

            var result = await page.OnPostAsync();

            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.ShouldBeFalse();
        }

        [Fact]
        public async Task OnPost_GivenNameExists_ReturnsPageWithModelError()
        {
            var item = new LegalAuthorityUpdate(GetLegalAuthorityViewList()[0]);
            var repo = new Mock<ILegalAuthorityRepository> {DefaultValue = DefaultValue.Mock};
            repo.Setup(l => l.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(GetLegalAuthorityViewList()[0]);
            repo.Setup(l => l.NameExistsAsync(It.IsAny<string>(), It.IsAny<int?>()))
                .ReturnsAsync(true);
            var page = new Edit(repo.Object) {Item = item};

            var result = await page.OnPostAsync();

            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.ShouldBeFalse();
        }
    }
}