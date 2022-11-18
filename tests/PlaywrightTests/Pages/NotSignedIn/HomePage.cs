using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace PlaywrightTests.Pages.NotSignedIn;

public class HomePage
{

    [Test]
    public async Task TestTextHomePage()
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

        // Expect a title "to contain" a substring.
        await Assertions.Expect(page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Expect the following text in the home page
        await Assertions.Expect(page.GetByText("The Georgia Environmental Protection Division uses enforcement actions to correc")).ToBeVisibleAsync();
        await Assertions.Expect(page.GetByRole(AriaRole.Heading, new() { NameString = "Proposed Orders" })).ToBeVisibleAsync();
        await Assertions.Expect(page.GetByRole(AriaRole.Heading, new() { NameString = "Executed Orders" })).ToBeVisibleAsync();
        await Assertions.Expect(page.GetByRole(AriaRole.Heading, new() { NameString = "Mail Subscriptions" })).ToBeVisibleAsync();
        await Assertions.Expect(page.GetByText("Mail subscriptions to these notices are available at a cost of $50 per year. Tha")).ToBeVisibleAsync();
        await Assertions.Expect(page.GetByRole(AriaRole.Heading, new() { NameString = "API Access" })).ToBeVisibleAsync();

        // check the number of tables
        int numTables = await page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(2));

        // check the number of rows in the first table
        int tableRows1 = await page.Locator("//section[1]/div/table/tbody/tr").CountAsync();
        Assert.That(tableRows1, Is.EqualTo(3));
        // check the column labels of the first table
        await Assertions.Expect(page.Locator("//section[1]/div/table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Assertions.Expect(page.Locator("//section[1]/div/table/thead/tr/th[2]")).ToContainTextAsync("Date Comment Period Closes");
        await Assertions.Expect(page.Locator("//section[1]/div/table/thead/tr/th[3]")).ToContainTextAsync("");
        
        // check the number of rows in the second table
        int tableRows2 = await page.Locator("//section[2]/div/table/tbody/tr").CountAsync();
        Assert.That(tableRows2, Is.EqualTo(3));
        // check the column labels of the first table
        await Assertions.Expect(page.Locator("//section[2]/div/table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Assertions.Expect(page.Locator("//section[2]/div/table/thead/tr/th[2]")).ToContainTextAsync("Date Executed");
        await Assertions.Expect(page.Locator("//section[2]/div/table/thead/tr/th[3]")).ToContainTextAsync("");

        // Dispose context and page once it is no longer needed.
        await context.CloseAsync();
        await page.CloseAsync();
    }
}
