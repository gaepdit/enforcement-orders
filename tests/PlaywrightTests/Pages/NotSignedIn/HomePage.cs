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

        // Dispose context and page once it is no longer needed.
        await context.CloseAsync();
        await page.CloseAsync();
    }
}
