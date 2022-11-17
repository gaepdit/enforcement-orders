using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;

namespace PlaywrightTests.pages.NotSignedIn;

public class CurrentProposed
{
    [Test]
    public async Task TestCurrentProposed()
    {
        // launches Playwright
        using var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
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
}
