using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace PlaywrightTests.Pages.NotSignedIn;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
[SuppressMessage("ReSharper", "ArrangeObjectCreationWhenTypeNotEvident")]
public class RecentExecuted : PageTest
{
    [SuppressMessage("Structure", "NUnit1028:The non-test method is public")]
    public override BrowserNewContextOptions ContextOptions() => PlaywrightHelpers.DefaultContextOptions();

    [Test]
    public async Task TestRecentExecuted()
    {
        await Page.GotoAsync("/");

        // click the button
        await Page.GetByRole(AriaRole.Heading, new() { NameString = "Executed Orders" })
            .Locator("..")
            .GetByRole(AriaRole.Link, new() { NameString = "View Full Report" }).ClickAsync();
        await Page.WaitForURLAsync("/RecentExecuted");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("Recently Executed Enforcement Orders"));

        // Check for text in the front of the Page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Recently Executed Enforcement Orders" }))
            .ToBeVisibleAsync();
        await Expect(Page.GetByText("(Notices that change weekly)")).ToBeVisibleAsync();

        // check the number of tables
        var numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(3));

        // check the number of rows in the first table
        var tableRows = await Page.Locator("//table[1]/tbody/tr").CountAsync();
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
        await Page.GotoAsync("/Details/2");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Check for text in the front of the Page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Enforcement Order EPD-WP-0002" }))
            .ToBeVisibleAsync();

        // check the number of tables
        var numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows in the first table
        var tableRows = await Page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(9));

        // check the first column of the first table (the label)
        await Expect(Page.Locator("//table[1]/tbody/tr[1]/th")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//table[1]/tbody/tr[2]/th")).ToContainTextAsync("County");
        await Expect(Page.Locator("//table[1]/tbody/tr[3]/th")).ToContainTextAsync("Cause of Order");
        await Expect(Page.Locator("//table[1]/tbody/tr[4]/th")).ToContainTextAsync("Requirements of Order");
        await Expect(Page.Locator("//table[1]/tbody/tr[5]/th")).ToContainTextAsync("Settlement Amount");
        await Expect(Page.Locator("//table[1]/tbody/tr[6]/th")).ToContainTextAsync("Legal Authority");
        await Expect(Page.Locator("//table[1]/tbody/tr[7]/th")).ToContainTextAsync("Date Executed");
        await Expect(Page.Locator("//table[1]/tbody/tr[8]/th"))
            .ToContainTextAsync("Publication Date For Executed Order");
        await Expect(Page.Locator("//table[1]/tbody/tr[9]/th")).ToContainTextAsync("File Attachments");
    }
}
