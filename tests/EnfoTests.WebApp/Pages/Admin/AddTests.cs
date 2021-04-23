using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Domain.Repositories;
using Enfo.Domain.Resources.EnforcementOrder;
using Enfo.Domain.Resources.EpdContact;
using Enfo.Domain.Resources.LegalAuthority;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages.Admin;
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

namespace EnfoTests.WebApp.Pages.Admin
{
    public class AddTests
    {
        [Fact]
        public async Task OnGet_ReturnsWithDefaultCreateResource()
        {
            var legalRepo = new Mock<ILegalAuthorityRepository>();
            legalRepo.Setup(l => l.ListAsync(false)).ReturnsAsync(new List<LegalAuthorityView>());
            var contactRepo = new Mock<IEpdContactRepository>();
            contactRepo.Setup(l => l.ListAsync(false)).ReturnsAsync(new List<EpdContactView>());
            var page = new Add(Mock.Of<IEnforcementOrderRepository>(), legalRepo.Object, contactRepo.Object);

            await page.OnGetAsync();
            page.Item.Should().BeEquivalentTo(new EnforcementOrderCreate());
        }

        [Fact]
        public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
        {
            var item = GetValidEnforcementOrderCreate();
            // Initialize Page TempData
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            // Mock repos
            var orderRepo = new Mock<IEnforcementOrderRepository>();
            orderRepo.Setup(l => l.OrderNumberExistsAsync(It.IsAny<string>(), It.IsAny<int?>()))
                .ReturnsAsync(false);
            orderRepo.Setup(l => l.CreateAsync(item)).ReturnsAsync(9);
            // Construct Page
            var page = new Add(orderRepo.Object, Mock.Of<ILegalAuthorityRepository>(), Mock.Of<IEpdContactRepository>())
                {TempData = tempData, Item = item};

            var result = await page.OnPostAsync();

            var expected = new DisplayMessage(Context.Success,
                "The new Enforcement Order has been successfully added.");
            page.TempData?.GetDisplayMessage().Should().BeEquivalentTo(expected);
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult) result).PageName.ShouldEqual("Details");
            ((RedirectToPageResult) result).RouteValues["id"].ShouldEqual(9);
        }

        [Fact]
        public async Task OnPost_GivenModelError_ReturnsPageWithModelError()
        {
            var item = GetValidEnforcementOrderCreate();
            // Mock repos
            var legalRepo = new Mock<ILegalAuthorityRepository>();
            legalRepo.Setup(l => l.ListAsync(false)).ReturnsAsync(new List<LegalAuthorityView>());
            var contactRepo = new Mock<IEpdContactRepository>();
            contactRepo.Setup(l => l.ListAsync(false)).ReturnsAsync(new List<EpdContactView>());
            var orderRepo = new Mock<IEnforcementOrderRepository>();
            orderRepo.Setup(l => l.OrderNumberExistsAsync(It.IsAny<string>(), It.IsAny<int?>()))
                .ReturnsAsync(false);
            // Construct Page
            var page = new Add(orderRepo.Object, legalRepo.Object, contactRepo.Object)
                {Item = item};
            page.ModelState.AddModelError("key", "message");

            var result = await page.OnPostAsync();

            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.ShouldBeFalse();
            page.ModelState.ErrorCount.ShouldEqual(1);
        }

        [Fact]
        public async Task OnPost_GivenInvalidResource_ReturnsPageWithModelError()
        {
            var item = GetValidEnforcementOrderCreate();
            item.CommentContactId = null;
            item.CommentPeriodClosesDate = null;
            item.ProposedOrderPostedDate = null;
            // Mock repos
            var legalRepo = new Mock<ILegalAuthorityRepository>();
            legalRepo.Setup(l => l.ListAsync(false)).ReturnsAsync(new List<LegalAuthorityView>());
            var contactRepo = new Mock<IEpdContactRepository>();
            contactRepo.Setup(l => l.ListAsync(false)).ReturnsAsync(new List<EpdContactView>());
            var orderRepo = new Mock<IEnforcementOrderRepository>();
            orderRepo.Setup(l => l.OrderNumberExistsAsync(It.IsAny<string>(), It.IsAny<int?>()))
                .ReturnsAsync(false);
            // Construct Page
            var page = new Add(orderRepo.Object, legalRepo.Object, contactRepo.Object)
                {Item = item};

            var result = await page.OnPostAsync();

            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.ShouldBeFalse();
            page.ModelState.ErrorCount.ShouldEqual(3);
        }
        [Fact]
        public async Task OnPost_GivenOrderNumberExists_ReturnsPageWithModelError()
        {
            var item = GetValidEnforcementOrderCreate();
            // Mock repos
            var legalRepo = new Mock<ILegalAuthorityRepository>();
            legalRepo.Setup(l => l.ListAsync(false)).ReturnsAsync(new List<LegalAuthorityView>());
            var contactRepo = new Mock<IEpdContactRepository>();
            contactRepo.Setup(l => l.ListAsync(false)).ReturnsAsync(new List<EpdContactView>());
            var orderRepo = new Mock<IEnforcementOrderRepository>();
            orderRepo.Setup(l => l.OrderNumberExistsAsync(It.IsAny<string>(), It.IsAny<int?>()))
                .ReturnsAsync(true);
            // Construct Page
            var page = new Add(orderRepo.Object, legalRepo.Object, contactRepo.Object)
                {Item = item};

            var result = await page.OnPostAsync();

            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.ShouldBeFalse();
            page.ModelState.ErrorCount.ShouldEqual(1);
        }
    }
}