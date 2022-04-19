using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages.Admin.Maintenance.LegalAuthorities;
using Enfo.WebApp.Platform.RazorHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions.AssertExtensions;
using static EnfoTests.Helpers.ResourceHelper;

namespace EnfoTests.WebApp.Pages.Admin.Maintenance.LegalAuthorities
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

            page.Item.Should().BeEquivalentTo(new LegalAuthorityCommand(item));
            page.Item.Id.ShouldEqual(item.Id);
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
            var page = new Edit(repo.Object) { TempData = tempData };

            var result = await page.OnGetAsync(item.Id);

            var expected = new DisplayMessage(Context.Warning,
                $"Inactive {Edit.ThisOption.PluralName} cannot be edited.");
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expected);

            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.ShouldEqual("Index");
        }


        [Fact]
        public async Task OnPost_GivenInvalidId_ReturnsNotFound()
        {
            var repo = new Mock<ILegalAuthorityRepository>();
            repo.Setup(l => l.GetAsync(It.IsAny<int>())).ReturnsAsync(null as LegalAuthorityView);
            var page = new Edit(repo.Object) { Item = new LegalAuthorityCommand { Id = 0 } };

            var result = await page.OnPostAsync();

            result.Should().BeOfType<NotFoundResult>();
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
            var page = new Edit(repo.Object)
            {
                TempData = tempData,
                Item = new LegalAuthorityCommand(item),
            };

            var result = await page.OnPostAsync();

            var expected = new DisplayMessage(Context.Warning,
                $"Inactive {Edit.ThisOption.PluralName} cannot be edited.");
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expected);

            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.ShouldEqual("Index");
        }

        [Fact]
        public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
        {
            var item = new LegalAuthorityCommand(GetLegalAuthorityViewList()[0]);
            var repo = new Mock<ILegalAuthorityRepository> { DefaultValue = DefaultValue.Mock };
            repo.Setup(l => l.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(GetLegalAuthorityViewList()[0]);
            repo.Setup(l => l.NameExistsAsync(It.IsAny<string>(), null))
                .ReturnsAsync(false);

            // Initialize Page TempData
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var page = new Edit(repo.Object) { TempData = tempData, Item = item };

            var result = await page.OnPostAsync();

            var expected = new DisplayMessage(Context.Success,
                $"{item.AuthorityName} successfully updated.");
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expected);

            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.ShouldEqual("Index");
        }
    }
}
