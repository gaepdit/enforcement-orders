﻿using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;

namespace PlaywrightTests.pages.NotSignedIn;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class CurrentProposed
{
    [Test]
    public async Task TestCurrentProposed()
    {
        // launches Playwright
        using var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
        // Create a new incognito browser context that ignores HTTPS errors
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true
        });
        // Create a new page inside context.
        var page = await context.NewPageAsync();

        await page.GotoAsync("https://localhost:44331/");

        // click on the link
        await page.Locator("div:has-text(\"Georgia EPD provides notice and opportunity for public comment on certain propos\")").GetByRole(AriaRole.Link, new() { NameString = "View Full Report" }).ClickAsync();
        await page.WaitForURLAsync("https://localhost:44331/CurrentProposed");

        // Expect a title "to contain" a substring.
        await Assertions.Expect(page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Check for text in the front of the page
        await Assertions.Expect(page.GetByRole(AriaRole.Heading, new() { NameString = "Current Proposed Enforcement Orders" })).ToBeVisibleAsync();
        await Assertions.Expect(page.GetByText("(Notices that change weekly)")).ToBeVisibleAsync();

        // check the number of tables
        int numTables = await page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(3));

        // check the number of rows in the first table
        int tableRows = await page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(10));

        // check to see if there are these text in the table
        await Assertions.Expect(page.Locator("//table[1]/tbody/tr[1]/th")).ToContainTextAsync("Facility");
        await Assertions.Expect(page.Locator("//table[1]/tbody/tr[2]/th")).ToContainTextAsync("County");
        await Assertions.Expect(page.Locator("//table[1]/tbody/tr[3]/th")).ToContainTextAsync("Cause of Order");
        await Assertions.Expect(page.Locator("//table[1]/tbody/tr[4]/th")).ToContainTextAsync("Requirements of Order");
        await Assertions.Expect(page.Locator("//table[1]/tbody/tr[5]/th")).ToContainTextAsync("Proposed Settlement Amount");
        await Assertions.Expect(page.Locator("//table[1]/tbody/tr[6]/th")).ToContainTextAsync("Legal Authority");
        await Assertions.Expect(page.Locator("//table[1]/tbody/tr[7]/th")).ToContainTextAsync("Publication Date For Proposed Order");
        await Assertions.Expect(page.Locator("//table[1]/tbody/tr[8]/th")).ToContainTextAsync("Date Comment Period Closes");
        await Assertions.Expect(page.Locator("//table[1]/tbody/tr[9]/th")).ToContainTextAsync("Send Comments To");
        await Assertions.Expect(page.Locator("//table[1]/tbody/tr[10]/th")).ToContainTextAsync("Public Hearing Scheduled");
    }

    [Test]
    public async Task TestCurrentProposedDetails()
    {
        // launches Playwright
        using var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
        // Create a new incognito browser context that ignores HTTPS errors
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true
        });
        // Create a new page inside context.
        var page = await context.NewPageAsync();

        await page.GotoAsync("https://localhost:44331/");

        // click on the link
        await page.GetByRole(AriaRole.Row, new() { NameString = "fgh15-2d4b3e37-2b23-4a59-aab1-67384b86c790 Stephens County 26‑Nov‑2022 View" }).GetByRole(AriaRole.Link, new() { NameString = "View" }).ClickAsync();
        await page.WaitForURLAsync("https://localhost:44331/Details/15");

        // Expect a title "to contain" a substring.
        await Assertions.Expect(page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Check for text in the front of the page
        await Assertions.Expect(page.GetByRole(AriaRole.Heading, new() { NameString = "Enforcement Order EPD-SW-WQ-0015" })).ToBeVisibleAsync();

        // check the number of tables
        int numTables = await page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows in the first table
        int tableRows = await page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(11));

        // check to see if there are these text in the table
        await Assertions.Expect(page.Locator("//table[1]/tbody/tr[1]/th")).ToContainTextAsync("Facility");
        await Assertions.Expect(page.Locator("//table[1]/tbody/tr[2]/th")).ToContainTextAsync("County");
        await Assertions.Expect(page.Locator("//table[1]/tbody/tr[3]/th")).ToContainTextAsync("Cause of Order");
        await Assertions.Expect(page.Locator("//table[1]/tbody/tr[4]/th")).ToContainTextAsync("Requirements of Order");
        await Assertions.Expect(page.Locator("//table[1]/tbody/tr[5]/th")).ToContainTextAsync("Proposed Settlement Amount");
        await Assertions.Expect(page.Locator("//table[1]/tbody/tr[6]/th")).ToContainTextAsync("Legal Authority");
        await Assertions.Expect(page.Locator("//table[1]/tbody/tr[7]/th")).ToContainTextAsync("Publication Date For Proposed Order");
        await Assertions.Expect(page.Locator("//table[1]/tbody/tr[8]/th")).ToContainTextAsync("Date Comment Period Closes");
        await Assertions.Expect(page.Locator("//table[1]/tbody/tr[9]/th")).ToContainTextAsync("Send Comments To");
        await Assertions.Expect(page.Locator("//table[1]/tbody/tr[10]/th")).ToContainTextAsync("Public Hearing Scheduled");
        await Assertions.Expect(page.Locator("//table[1]/tbody/tr[11]/th")).ToContainTextAsync("File Attachments");

    }
}
