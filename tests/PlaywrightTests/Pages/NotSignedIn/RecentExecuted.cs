using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace PlaywrightTests.Pages.NotSignedIn;

public class RecentExecuted
{
    [Test]
    public async Task TestRecentExecuted()
    {
        // launches Playwright
        using var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions {
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

        // click the button
        await page.Locator("div:has-text(\"Georgia EPD issues a notice of fully executed administrative orders and fully ex\")").GetByRole(AriaRole.Link, new() { NameString = "View Full Report" }).ClickAsync();
        await page.WaitForURLAsync("https://localhost:44331/RecentExecuted");

        // check if the URL is correct
        //string title = await page.TitleAsync();
        //Assertions.Equals(title, "https://localhost:44331/RecentExecuted");

        // Expect a title "to contain" a substring.
        await Assertions.Expect(page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Check for text in the front of the page
        await Assertions.Expect(page.GetByRole(AriaRole.Heading, new() { NameString = "Recently Executed Enforcement Orders" })).ToBeVisibleAsync();
        await Assertions.Expect(page.GetByText("(Notices that change weekly)")).ToBeVisibleAsync();

        // check the number of tables
        int numTables = await page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(3));

        // check the number of rows in the first table
        int tableRows = await page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(7));

        // check the first column of the first table (the label)
        await Assertions.Expect(page.Locator("//table[1]/tbody/tr[1]/th")).ToContainTextAsync("Facility");
        await Assertions.Expect(page.Locator("//table[1]/tbody/tr[2]/th")).ToContainTextAsync("County");
        await Assertions.Expect(page.Locator("//table[1]/tbody/tr[3]/th")).ToContainTextAsync("Cause of Order");
        await Assertions.Expect(page.Locator("//table[1]/tbody/tr[4]/th")).ToContainTextAsync("Requirements of Order");
        await Assertions.Expect(page.Locator("//table[1]/tbody/tr[5]/th")).ToContainTextAsync("Settlement Amount");
        await Assertions.Expect(page.Locator("//table[1]/tbody/tr[6]/th")).ToContainTextAsync("Legal Authority");
        await Assertions.Expect(page.Locator("//table[1]/tbody/tr[7]/th")).ToContainTextAsync("Date Executed");

        // Dispose context and page once it is no longer needed.
        await context.CloseAsync();
        await page.CloseAsync();

    }
}
