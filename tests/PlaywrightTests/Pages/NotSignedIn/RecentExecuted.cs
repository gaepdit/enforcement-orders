using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace PlaywrightTests.Pages.NotSignedIn;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class RecentExecuted : PageTest
{
    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions()
        {
            ColorScheme = ColorScheme.Light,
            IgnoreHTTPSErrors = true
        };
    }

    [Test]
    public async Task TestRecentExecuted()
    {
        await Page.GotoAsync("https://localhost:44331/");

        // click the button
        await Page.Locator("div:has-text(\"Georgia EPD issues a notice of fully executed administrative orders and fully ex\")").GetByRole(AriaRole.Link, new() { NameString = "View Full Report" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/RecentExecuted");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Check for text in the front of the Page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Recently Executed Enforcement Orders" })).ToBeVisibleAsync();
        await Expect(Page.GetByText("(Notices that change weekly)")).ToBeVisibleAsync();

        // check the number of tables
        int numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(3));

        // check the number of rows in the first table
        int tableRows = await Page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(7));

        // check the first column of the first table (the label)
        await Expect(Page.Locator("//table[1]/tbody/tr[1]/th")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//table[1]/tbody/tr[2]/th")).ToContainTextAsync("County");
        await Expect(Page.Locator("//table[1]/tbody/tr[3]/th")).ToContainTextAsync("Cause of Order");
        await Expect(Page.Locator("//table[1]/tbody/tr[4]/th")).ToContainTextAsync("Requirements of Order");
        await Expect(Page.Locator("//table[1]/tbody/tr[5]/th")).ToContainTextAsync("Settlement Amount");
        await Expect(Page.Locator("//table[1]/tbody/tr[6]/th")).ToContainTextAsync("Legal Authority");
        await Expect(Page.Locator("//table[1]/tbody/tr[7]/th")).ToContainTextAsync("Date Executed");

    }

    [Test]
    public async Task TestRecentExecutedDetails()
    {
        // I could not click on the `view` link since it involves a randomly generated id
        await Page.GotoAsync("https://localhost:44331/Details/2");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Check for text in the front of the Page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Enforcement Order EPD-WP-0002" })).ToBeVisibleAsync();

        // check the number of tables
        int numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows in the first table
        int tableRows = await Page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(9));

        // check the first column of the first table (the label)
        await Expect(Page.Locator("//table[1]/tbody/tr[1]/th")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//table[1]/tbody/tr[2]/th")).ToContainTextAsync("County");
        await Expect(Page.Locator("//table[1]/tbody/tr[3]/th")).ToContainTextAsync("Cause of Order");
        await Expect(Page.Locator("//table[1]/tbody/tr[4]/th")).ToContainTextAsync("Requirements of Order");
        await Expect(Page.Locator("//table[1]/tbody/tr[5]/th")).ToContainTextAsync("Settlement Amount");
        await Expect(Page.Locator("//table[1]/tbody/tr[6]/th")).ToContainTextAsync("Legal Authority");
        await Expect(Page.Locator("//table[1]/tbody/tr[7]/th")).ToContainTextAsync("Date Executed");
        await Expect(Page.Locator("//table[1]/tbody/tr[8]/th")).ToContainTextAsync("Publication Date For Executed Order");
        await Expect(Page.Locator("//table[1]/tbody/tr[9]/th")).ToContainTextAsync("File Attachments");

    }
}
