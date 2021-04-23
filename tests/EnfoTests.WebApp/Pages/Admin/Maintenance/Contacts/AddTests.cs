using System.Threading.Tasks;
using Enfo.Domain.Repositories;
using Enfo.Domain.Resources.EpdContact;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages.Admin.Maintenance.Contacts;
using Enfo.WebApp.Platform.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;
using Xunit.Extensions.AssertExtensions;
using static EnfoTests.Helpers.ResourceHelper;

namespace EnfoTests.WebApp.Pages.Admin.Maintenance.Contacts
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
            var addressRepo = new Mock<IAddressRepository> {DefaultValue = DefaultValue.Mock};
            addressRepo.Setup(l => l.ListAsync(false))
                .ReturnsAsync(GetAddressViewList());

            // Initialize Page TempData
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var page = new Add(repo.Object, addressRepo.Object) {TempData = tempData, Item = item};

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
            var repo = new Mock<IEpdContactRepository> {DefaultValue = DefaultValue.Mock};
            var addressRepo = new Mock<IAddressRepository> {DefaultValue = DefaultValue.Mock};
            addressRepo.Setup(l => l.ListAsync(false))
                .ReturnsAsync(GetAddressViewList());
            var page = new Add(repo.Object, addressRepo.Object) {Item = new EpdContactCreate()};
            page.ModelState.AddModelError("key", "message");

            var result = await page.OnPostAsync();

            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.ShouldBeFalse();
        }
    }
}