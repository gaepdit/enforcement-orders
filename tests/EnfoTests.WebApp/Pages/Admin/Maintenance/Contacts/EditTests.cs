﻿using System.Linq;
using System.Threading.Tasks;
using Enfo.Domain.Mapping;
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
    public class EditTests
    {
        [Fact]
        public async Task OnGet_ReturnsWithItem()
        {
            var item = GetEpdContactViewList()[0];
            var repo = new Mock<IEpdContactRepository>();
            repo.Setup(l => l.GetAsync(item.Id)).ReturnsAsync(item);
            var page = new Edit(repo.Object);

            await page.OnGetAsync(item.Id);

            page.Item.Should().BeEquivalentTo(EpdContactMapping.ToEpdContactUpdate(item));
            page.Id.ShouldEqual(item.Id);
        }

        [Fact]
        public async Task OnGet_GivenNullId_ReturnsNotFound()
        {
            var page = new Edit(Mock.Of<IEpdContactRepository>());

            var result = await page.OnGetAsync(null);

            result.Should().BeOfType<NotFoundResult>();
            page.Item.ShouldBeNull();
        }

        [Fact]
        public async Task OnGet_GivenInvalidId_ReturnsNotFound()
        {
            var repo = new Mock<IEpdContactRepository>();
            repo.Setup(l => l.GetAsync(It.IsAny<int>())).ReturnsAsync(null as EpdContactView);
            var page = new Edit(repo.Object);

            var result = await page.OnGetAsync(-1);

            result.Should().BeOfType<NotFoundObjectResult>();
            page.Item.ShouldBeNull();
        }

        [Fact]
        public async Task OnGet_GivenInactiveItem_RedirectsWithDisplayMessage()
        {
            var item = GetEpdContactViewList().Single(e => !e.Active);
            var repo = new Mock<IEpdContactRepository>();
            repo.Setup(l => l.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(item);

            // Initialize Page TempData
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var page = new Edit(repo.Object)
                {TempData = tempData};

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
            var repo = new Mock<IEpdContactRepository>();
            repo.Setup(l => l.GetAsync(It.IsAny<int>())).ReturnsAsync(null as EpdContactView);
            var page = new Edit(repo.Object) {Id = 0};

            var result = await page.OnPostAsync();

            result.Should().BeOfType<NotFoundResult>();
            page.Item.ShouldBeNull();
        }

        [Fact]
        public async Task OnPost_GivenInactiveItem_RedirectsWithDisplayMessage()
        {
            var item = GetEpdContactViewList().Single(e => !e.Active);
            var repo = new Mock<IEpdContactRepository>();
            repo.Setup(l => l.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(item);

            // Initialize Page TempData
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var page = new Edit(repo.Object) 
                {TempData = tempData, Id = 0};

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
            var item = EpdContactMapping.ToEpdContactUpdate(GetEpdContactViewList()[0]);
            var repo = new Mock<IEpdContactRepository> {DefaultValue = DefaultValue.Mock};
            repo.Setup(l => l.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(GetEpdContactViewList()[0]);
            
            // Initialize Page TempData
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var page = new Edit(repo.Object) {TempData = tempData, Item = item};

            var result = await page.OnPostAsync();

            var expected = new DisplayMessage(Context.Success,
                $"{Edit.ThisOption.SingularName} successfully updated.");
            page.TempData?.GetDisplayMessage().Should().BeEquivalentTo(expected);

            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult) result).PageName.ShouldEqual("Index");
        }

        [Fact]
        public async Task OnPost_GivenModelError_ReturnsPageWithModelError()
        {
            var repo = new Mock<IEpdContactRepository> {DefaultValue = DefaultValue.Mock};
            repo.Setup(l => l.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(GetEpdContactViewList()[0]);
            var page = new Edit(repo.Object) {Item = new EpdContactUpdate()};
            page.ModelState.AddModelError("key", "message");

            var result = await page.OnPostAsync();

            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.ShouldBeFalse();
        }
    }
}