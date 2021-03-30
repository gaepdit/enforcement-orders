﻿using System.Linq;
using System.Threading.Tasks;
using Enfo.Repository.Repositories;
using Enfo.WebApp.Extensions;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages.Admin;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;
using Xunit.Extensions.AssertExtensions;
using static TestHelpers.ResourceHelper;
using static TestHelpers.DataHelper;

namespace WebApp.Tests.Pages.Admin
{
    public class DetailsTests
    {
        [Fact]
        public async Task OnGet_ReturnsWithOrder()
        {
            var itemId = GetEnforcementOrders.First().Id;
            var item = GetEnforcementOrderAdminView(itemId);
            var repo = new Mock<IEnforcementOrderRepository>();
            repo.Setup(l => l.GetAdminViewAsync(itemId)).ReturnsAsync(item);
            var page = new Details(repo.Object);

            await page.OnGetAsync(itemId);

            page.Item.ShouldEqual(item);
        }

        [Fact]
        public async Task SetDisplayMessage_ReturnsWithDisplayMessage()
        {
            // Not testing returned Item, but it must be populated to return Page
            var itemId = GetEnforcementOrders.First().Id;
            var item = GetEnforcementOrderAdminView(itemId);
            var repo = new Mock<IEnforcementOrderRepository>();
            repo.Setup(l => l.GetAdminViewAsync(itemId)).ReturnsAsync(item);

            // Initialize Page TempData
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var page = new Details(repo.Object) {TempData = tempData};

            page.TempData.SetDisplayMessage(Context.Info, "Info message");
            await page.OnGetAsync(itemId);

            var expected = new DisplayMessage(Context.Info, "Info message");
            page.Message.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task OnGet_MissingIdReturnsNotFound()
        {
            var mockRepo = new Mock<IEnforcementOrderRepository>();
            var pageModel = new Details(mockRepo.Object);

            var result = await pageModel.OnGetAsync(null).ConfigureAwait(false);

            result.Should().BeOfType<NotFoundObjectResult>();
            pageModel.Item.ShouldBeNull();
            pageModel.Message.ShouldBeNull();
        }

        [Fact]
        public async Task OnGet_NonexistentIdReturnsNotFound()
        {
            var mockRepo = new Mock<IEnforcementOrderRepository>();
            var pageModel = new Details(mockRepo.Object);

            var result = await pageModel.OnGetAsync(-1).ConfigureAwait(false);

            result.Should().BeOfType<NotFoundObjectResult>();
            pageModel.Item.ShouldBeNull();
            pageModel.Message.ShouldBeNull();
        }
    }
}