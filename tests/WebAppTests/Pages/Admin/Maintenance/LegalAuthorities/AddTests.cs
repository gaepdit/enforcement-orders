using Enfo.Domain.Repositories;
using Enfo.Domain.Resources.LegalAuthority;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages.Admin.Maintenance.LegalAuthorities;
using Enfo.WebApp.Platform.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace EnfoTests.WebApp.Pages.Admin.Maintenance.LegalAuthorities
{
    public class AddTests
    {
        [Fact]
        public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
        {
            var item = new LegalAuthorityCommand { AuthorityName = "test" };
            var repo = new Mock<ILegalAuthorityRepository> { DefaultValue = DefaultValue.Mock };
            repo.Setup(l => l.NameExistsAsync(It.IsAny<string>(), null))
                .ReturnsAsync(false);
            repo.Setup(l => l.CreateAsync(It.IsAny<LegalAuthorityCommand>()))
                .ReturnsAsync(1);

            // Initialize Page TempData
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var page = new Add { TempData = tempData, Item = item };

            var result = await page.OnPostAsync(repo.Object);

            var expected = new DisplayMessage(Context.Success,
                $"{item.AuthorityName} successfully added.");
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expected);
            page.HighlightId.ShouldEqual(1);

            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.ShouldEqual("Index");
        }

        [Fact]
        public async Task OnPost_GivenModelError_ReturnsPageWithModelError()
        {
            var repo = new Mock<ILegalAuthorityRepository> { DefaultValue = DefaultValue.Mock };
            var page = new Add { Item = new LegalAuthorityCommand() };
            page.ModelState.AddModelError("key", "message");

            var result = await page.OnPostAsync(repo.Object);

            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.ShouldBeFalse();
        }
    }
}
