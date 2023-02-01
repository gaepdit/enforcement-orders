using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace PlaywrightTests.Pages.NotSignedIn;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class CurrentProposed : PageTest
{
    [SuppressMessage("SonarLint", "external_roslyn:NUnit1028")]
    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions()
        {
            ColorScheme = ColorScheme.Light,
            IgnoreHTTPSErrors = true
        };
    }
    [Test]
    public async Task TestCurrentProposed()
    {
        await Page.GotoAsync("https://localhost:44331/");

        // click on the link
        await Page.Locator("div:has-text(\"Georgia EPD provides notice and opportunity for public comment on certain propos\")").GetByRole(AriaRole.Link, new() { NameString = "View Full Report" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/CurrentProposed");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Check for text in the front of the Page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Current Proposed Enforcement Orders" })).ToBeVisibleAsync();
        await Expect(Page.GetByText("(Notices that change weekly)")).ToBeVisibleAsync();

        // check the number of tables
        int numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(3));

        // check the number of rows in the first table
        int tableRows = await Page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(10));

        // check to see if there are these text in the table
        await Expect(Page.Locator("//table[1]/tbody/tr[1]/th")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//table[1]/tbody/tr[2]/th")).ToContainTextAsync("County");
        await Expect(Page.Locator("//table[1]/tbody/tr[3]/th")).ToContainTextAsync("Cause of Order");
        await Expect(Page.Locator("//table[1]/tbody/tr[4]/th")).ToContainTextAsync("Requirements of Order");
        await Expect(Page.Locator("//table[1]/tbody/tr[5]/th")).ToContainTextAsync("Proposed Settlement Amount");
        await Expect(Page.Locator("//table[1]/tbody/tr[6]/th")).ToContainTextAsync("Legal Authority");
        await Expect(Page.Locator("//table[1]/tbody/tr[7]/th")).ToContainTextAsync("Publication Date For Proposed Order");
        await Expect(Page.Locator("//table[1]/tbody/tr[8]/th")).ToContainTextAsync("Date Comment Period Closes");
        await Expect(Page.Locator("//table[1]/tbody/tr[9]/th")).ToContainTextAsync("Send Comments To");
        await Expect(Page.Locator("//table[1]/tbody/tr[10]/th")).ToContainTextAsync("Public Hearing Scheduled");
    }

    [Test]
    public async Task TestCurrentProposedDetails()
    {
        // I could not click on the `view` link since it involves a randomly generated id
        await Page.GotoAsync("https://localhost:44331/Details/15");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Check for text in the front of the Page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Enforcement Order EPD-SW-WQ-0015" })).ToBeVisibleAsync();

        // check the number of tables
        int numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows in the first table
        int tableRows = await Page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(11));

        // check to see if there are these text in the table
        await Expect(Page.Locator("//table[1]/tbody/tr[1]/th")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//table[1]/tbody/tr[2]/th")).ToContainTextAsync("County");
        await Expect(Page.Locator("//table[1]/tbody/tr[3]/th")).ToContainTextAsync("Cause of Order");
        await Expect(Page.Locator("//table[1]/tbody/tr[4]/th")).ToContainTextAsync("Requirements of Order");
        await Expect(Page.Locator("//table[1]/tbody/tr[5]/th")).ToContainTextAsync("Proposed Settlement Amount");
        await Expect(Page.Locator("//table[1]/tbody/tr[6]/th")).ToContainTextAsync("Legal Authority");
        await Expect(Page.Locator("//table[1]/tbody/tr[7]/th")).ToContainTextAsync("Publication Date For Proposed Order");
        await Expect(Page.Locator("//table[1]/tbody/tr[8]/th")).ToContainTextAsync("Date Comment Period Closes");
        await Expect(Page.Locator("//table[1]/tbody/tr[9]/th")).ToContainTextAsync("Send Comments To");
        await Expect(Page.Locator("//table[1]/tbody/tr[10]/th")).ToContainTextAsync("Public Hearing Scheduled");
        await Expect(Page.Locator("//table[1]/tbody/tr[11]/th")).ToContainTextAsync("File Attachments");

    }
}
