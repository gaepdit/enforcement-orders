using System.Diagnostics.CodeAnalysis;

namespace PlaywrightTests.Pages.NotSignedIn;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
[SuppressMessage("ReSharper", "ArrangeObjectCreationWhenTypeNotEvident")]
public class HomePage : PageTest
{
    [SuppressMessage("Structure", "NUnit1028:The non-test method is public")]
    public override BrowserNewContextOptions ContextOptions() => PlaywrightHelpers.DefaultContextOptions();

    [Test]
    public async Task TestTextHomePage()
    {
        await Page.GotoAsync("/");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync("Georgia EPD Enforcement Orders");

        // Expect the following text in the home page
        await Expect(Page.GetByText("The Georgia Environmental Protection Division uses enforcement actions"))
            .ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Proposed Orders" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Executed Orders" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Mail Subscriptions" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "API Access" })).ToBeVisibleAsync();

        // check the number of tables
        var numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(2));

        // check the number of rows in the first table
        var tableRows1 = await Page.Locator("//section[1]/div/table/tbody/tr").CountAsync();
        Assert.That(tableRows1, Is.EqualTo(3));
        // check the column labels of the first table
        await Expect(Page.Locator("//section[1]/div/table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//section[1]/div/table/thead/tr/th[2]"))
            .ToContainTextAsync("Date Comment Period Closes");
        await Expect(Page.Locator("//section[1]/div/table/thead/tr/th[3]")).ToContainTextAsync("");

        // check the number of rows in the second table
        var tableRows2 = await Page.Locator("//section[2]/div/table/tbody/tr").CountAsync();
        Assert.That(tableRows2, Is.EqualTo(3));
        // check the column labels of the first table
        await Expect(Page.Locator("//section[2]/div/table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//section[2]/div/table/thead/tr/th[2]")).ToContainTextAsync("Date Executed");
        await Expect(Page.Locator("//section[2]/div/table/thead/tr/th[3]")).ToContainTextAsync("");
    }
}
