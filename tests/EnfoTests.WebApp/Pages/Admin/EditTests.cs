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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions.AssertExtensions;
using static EnfoTests.Helpers.ResourceHelper;

namespace EnfoTests.WebApp.Pages.Admin
{
    public class EditTests
    {
        [Fact]
        public async Task OnGet_ReturnsWithItem()
        {
            var item = GetEnforcementOrderAdminView(1);
            var orderRepo = new Mock<IEnforcementOrderRepository>();
            orderRepo.Setup(l => l.GetAdminViewAsync(It.IsAny<int>())).ReturnsAsync(item);
            var legalRepo = new Mock<ILegalAuthorityRepository>();
            legalRepo.Setup(l => l.ListAsync(false)).ReturnsAsync(new List<LegalAuthorityView>());
            var contactRepo = new Mock<IEpdContactRepository>();
            contactRepo.Setup(l => l.ListAsync(false)).ReturnsAsync(new List<EpdContactView>());
            var page = new Edit(orderRepo.Object, legalRepo.Object, contactRepo.Object);

            await page.OnGetAsync(1);

            page.Item.Should().BeEquivalentTo(new EnforcementOrderUpdate(item));
            page.Item.Id.ShouldEqual(item.Id);
            page.OriginalOrderNumber.ShouldEqual(item.OrderNumber);
        }


        [Fact]
        public async Task OnGet_GivenNullId_ReturnsNotFound()
        {
            var page = new Edit(Mock.Of<IEnforcementOrderRepository>(),
                Mock.Of<ILegalAuthorityRepository>(), Mock.Of<IEpdContactRepository>());

            var result = await page.OnGetAsync(null);

            result.Should().BeOfType<NotFoundResult>();
            page.Item.ShouldBeNull();
        }

        [Fact]
        public async Task OnGet_GivenInvalidId_ReturnsNotFound()
        {
            var orderRepo = new Mock<IEnforcementOrderRepository>();
            orderRepo.Setup(l => l.GetAdminViewAsync(It.IsAny<int>()))
                .ReturnsAsync(null as EnforcementOrderAdminView);
            var page = new Edit(orderRepo.Object, Mock.Of<ILegalAuthorityRepository>(),
                Mock.Of<IEpdContactRepository>());

            var result = await page.OnGetAsync(-1);

            result.Should().BeOfType<NotFoundObjectResult>();
            page.Item.ShouldBeNull();
        }

        [Fact]
        public async Task OnGet_GivenDeletedItem_RedirectsWithDisplayMessage()
        {
            var item = GetEnforcementOrderAdminViewList().Single(e => e.Deleted);
            var orderRepo = new Mock<IEnforcementOrderRepository>();
            orderRepo.Setup(l => l.GetAdminViewAsync(It.IsAny<int>()))
                .ReturnsAsync(item);

            // Initialize Page TempData
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var page = new Edit(orderRepo.Object, Mock.Of<ILegalAuthorityRepository>(),
                    Mock.Of<IEpdContactRepository>())
                { TempData = tempData };

            var result = await page.OnGetAsync(item.Id);

            var expected = new DisplayMessage(Context.Warning,
                "This Enforcement Order is deleted and cannot be edited.");
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expected);

            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.ShouldEqual("Details");
            ((RedirectToPageResult)result).RouteValues!["id"].ShouldEqual(item.Id);
        }


        [Fact]
        public async Task OnPost_GivenInvalidId_ReturnsNotFound()
        {
            var orderRepo = new Mock<IEnforcementOrderRepository>();
            orderRepo.Setup(l => l.GetAdminViewAsync(It.IsAny<int>()))
                .ReturnsAsync(null as EnforcementOrderAdminView);
            var page = new Edit(orderRepo.Object, Mock.Of<ILegalAuthorityRepository>(),
                Mock.Of<IEpdContactRepository>())
            {
                Item = new EnforcementOrderUpdate(),
            };

            var result = await page.OnPostAsync();

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task OnPost_GivenDeletedItem_RedirectsWithDisplayMessage()
        {
            var item = GetEnforcementOrderAdminViewList().Single(e => e.Deleted);
            var orderRepo = new Mock<IEnforcementOrderRepository>();
            orderRepo.Setup(l => l.GetAdminViewAsync(It.IsAny<int>()))
                .ReturnsAsync(item);

            // Initialize Page TempData
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var page = new Edit(orderRepo.Object, Mock.Of<ILegalAuthorityRepository>(),
                Mock.Of<IEpdContactRepository>())
            {
                TempData = tempData,
                Item = new EnforcementOrderUpdate { Id = item.Id },
            };

            var result = await page.OnPostAsync();

            var expected = new DisplayMessage(Context.Warning,
                "This Enforcement Order is deleted and cannot be edited.");
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expected);

            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.ShouldEqual("Details");
            ((RedirectToPageResult)result).RouteValues!["id"].ShouldEqual(item.Id);
        }

        [Fact]
        public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
        {
            var originalItem = GetEnforcementOrderAdminViewList().First(e => !e.Deleted);
            var item = new EnforcementOrderUpdate(originalItem);
            var orderRepo = new Mock<IEnforcementOrderRepository> { DefaultValue = DefaultValue.Mock };
            orderRepo.Setup(l => l.GetAdminViewAsync(It.IsAny<int>()))
                .ReturnsAsync(originalItem);
            orderRepo.Setup(l => l.OrderNumberExistsAsync(It.IsAny<string>(), It.IsAny<int?>()))
                .ReturnsAsync(false);

            // Initialize Page TempData
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var page = new Edit(orderRepo.Object, Mock.Of<ILegalAuthorityRepository>(),
                    Mock.Of<IEpdContactRepository>())
                { TempData = tempData, Item = item };

            var result = await page.OnPostAsync();

            var expected = new DisplayMessage(Context.Success,
                "The Enforcement Order has been successfully updated.");
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expected);
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.ShouldEqual("Details");
            ((RedirectToPageResult)result).RouteValues!["Id"].ShouldEqual(originalItem.Id);
        }

        [Fact]
        public async Task OnPost_GivenModelError_ReturnsPageWithModelError()
        {
            var item = GetEnforcementOrderAdminView(1);
            var orderRepo = new Mock<IEnforcementOrderRepository>();
            orderRepo.Setup(l => l.GetAdminViewAsync(It.IsAny<int>())).ReturnsAsync(item);
            orderRepo.Setup(l => l.OrderNumberExistsAsync(It.IsAny<string>(), It.IsAny<int?>()))
                .ReturnsAsync(false);
            var legalRepo = new Mock<ILegalAuthorityRepository>();
            legalRepo.Setup(l => l.ListAsync(false)).ReturnsAsync(new List<LegalAuthorityView>());
            var contactRepo = new Mock<IEpdContactRepository>();
            contactRepo.Setup(l => l.ListAsync(false)).ReturnsAsync(new List<EpdContactView>());
            var page = new Edit(orderRepo.Object, legalRepo.Object, contactRepo.Object)
            {
                Item = new EnforcementOrderUpdate(item),
                OriginalOrderNumber = "original order number",
            };
            page.ModelState.AddModelError("key", "message");

            var result = await page.OnPostAsync();

            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.ShouldBeFalse();
            page.ModelState.ErrorCount.ShouldEqual(1);
            page.OriginalOrderNumber.ShouldEqual("original order number");
        }
    }
}
