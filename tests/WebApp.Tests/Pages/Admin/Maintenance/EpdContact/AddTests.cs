using System.Threading.Tasks;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources.EpdContact;
using Enfo.WebApp.Extensions;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages.Admin.Maintenance.EpdContacts;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace WebApp.Tests.Pages.Admin.Maintenance.EpdContact
{
    public class AddTests
    {
        [Fact]
        public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
        {
            var item = new EpdContactCreate
            {
                Email = "abc", Organization = "abc", Telephone = "abc", Title = "abc", AddressId = 2000,
                ContactName = "abc"
            };
            var repo = new Mock<IEpdContactRepository> {DefaultValue = DefaultValue.Mock};
            repo.Setup(l => l.CreateAsync(It.IsAny<EpdContactCreate>()))
                .ReturnsAsync(1);

            // Initialize Page TempData
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var page = new Add(repo.Object) {TempData = tempData, Item = item};

            var result = await page.OnPostAsync();

            var expected = new DisplayMessage(Context.Success,
                $"{Add.ThisOption.SingularName} successfully added.");
            page.TempData?.GetDisplayMessage().Should().BeEquivalentTo(expected);
            page.NewId.ShouldEqual(1);

            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult) result).PageName.ShouldEqual("Index");
        }

        [Fact]
        public async Task OnPost_GivenModelError_ReturnsPageWithModelError()
        {
            var repo = new Mock<IEpdContactRepository> {DefaultValue = DefaultValue.Mock};
            var page = new Add(repo.Object);
            page.ModelState.AddModelError("key", "message");

            var result = await page.OnPostAsync();

            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.ShouldBeFalse();
        }
    }
}