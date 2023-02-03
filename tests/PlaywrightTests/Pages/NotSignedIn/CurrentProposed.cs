using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace PlaywrightTests.Pages.NotSignedIn;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
[SuppressMessage("ReSharper", "ArrangeObjectCreationWhenTypeNotEvident")]
public class CurrentProposed : PageTest
{
    [SuppressMessage("Structure", "NUnit1028:The non-test method is public")]
    public override BrowserNewContextOptions ContextOptions() => PlaywrightHelpers.DefaultContextOptions();

    [Test]
    public async Task TestCurrentProposed()
    {
        await Page.GotoAsync("/");

        // click on the link
        await Page.GetByRole(AriaRole.Heading, new() { NameString = "Proposed Orders" })
            .Locator("..")
            .GetByRole(AriaRole.Link, new() { NameString = "View Full Report" }).ClickAsync();
        await Page.WaitForURLAsync("/CurrentProposed");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("Current Proposed Enforcement Orders"));

        // Check for text in the front of the Page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Current Proposed Enforcement Orders" }))
            .ToBeVisibleAsync();
        await Expect(Page.GetByText("(Notices that change weekly)")).ToBeVisibleAsync();

        // check the number of tables
        var numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(3));

        // check the number of rows in the first table
        var tableRows = await Page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(10));

        // check to see if there are these text in the table
        await Expect(Page.Locator("//table[1]/tbody/tr[1]/th")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//table[1]/tbody/tr[2]/th")).ToContainTextAsync("County");
        await Expect(Page.Locator("//table[1]/tbody/tr[3]/th")).ToContainTextAsync("Cause of Order");
        await Expect(Page.Locator("//table[1]/tbody/tr[4]/th")).ToContainTextAsync("Requirements of Order");
        await Expect(Page.Locator("//table[1]/tbody/tr[5]/th")).ToContainTextAsync("Proposed Settlement Amount");
        await Expect(Page.Locator("//table[1]/tbody/tr[6]/th")).ToContainTextAsync("Legal Authority");
        await Expect(Page.Locator("//table[1]/tbody/tr[7]/th"))
            .ToContainTextAsync("Publication Date For Proposed Order");
        await Expect(Page.Locator("//table[1]/tbody/tr[8]/th")).ToContainTextAsync("Date Comment Period Closes");
        await Expect(Page.Locator("//table[1]/tbody/tr[9]/th")).ToContainTextAsync("Send Comments To");
        await Expect(Page.Locator("//table[1]/tbody/tr[10]/th")).ToContainTextAsync("Public Hearing Scheduled");
    }

    [Test]
    public async Task TestCurrentProposedDetails()
    {
        await Page.GotoAsync("/Details/15");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("Enforcement Order EPD-SW-WQ-0015"));

        // Check for text in the front of the Page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Enforcement Order EPD-SW-WQ-0015" }))
            .ToBeVisibleAsync();

        // check the number of tables
        var numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows in the first table
        var tableRows = await Page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(11));

        // check to see if there are these text in the table
        await Expect(Page.Locator("//table[1]/tbody/tr[1]/th")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//table[1]/tbody/tr[2]/th")).ToContainTextAsync("County");
        await Expect(Page.Locator("//table[1]/tbody/tr[3]/th")).ToContainTextAsync("Cause of Order");
        await Expect(Page.Locator("//table[1]/tbody/tr[4]/th")).ToContainTextAsync("Requirements of Order");
        await Expect(Page.Locator("//table[1]/tbody/tr[5]/th")).ToContainTextAsync("Proposed Settlement Amount");
        await Expect(Page.Locator("//table[1]/tbody/tr[6]/th")).ToContainTextAsync("Legal Authority");
        await Expect(Page.Locator("//table[1]/tbody/tr[7]/th"))
            .ToContainTextAsync("Publication Date For Proposed Order");
        await Expect(Page.Locator("//table[1]/tbody/tr[8]/th")).ToContainTextAsync("Date Comment Period Closes");
        await Expect(Page.Locator("//table[1]/tbody/tr[9]/th")).ToContainTextAsync("Send Comments To");
        await Expect(Page.Locator("//table[1]/tbody/tr[10]/th")).ToContainTextAsync("Public Hearing Scheduled");
        await Expect(Page.Locator("//table[1]/tbody/tr[11]/th")).ToContainTextAsync("File Attachments");
    }
}
