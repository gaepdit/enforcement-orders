using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace PlaywrightTests.Pages.NotSignedIn;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class HomePage : PageTest
{
    [SuppressMessage("Structure", "NUnit1028:The non-test method is public")]
    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions()
        {
            ColorScheme = ColorScheme.Light,
            IgnoreHTTPSErrors = true
        };
    }

    [Test]
    public async Task TestTextHomePage()
    {
        await Page.GotoAsync("https://localhost:44331/");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Expect the following text in the home page
        await Expect(Page.GetByText("The Georgia Environmental Protection Division uses enforcement actions to correc")).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Proposed Orders" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Executed Orders" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Mail Subscriptions" })).ToBeVisibleAsync();
        await Expect(Page.GetByText("Mail subscriptions to these notices are available at a cost of $50 per year. Tha")).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "API Access" })).ToBeVisibleAsync();

        // check the number of tables
        int numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(2));

        // check the number of rows in the first table
        int tableRows1 = await Page.Locator("//section[1]/div/table/tbody/tr").CountAsync();
        Assert.That(tableRows1, Is.EqualTo(3));
        // check the column labels of the first table
        await Expect(Page.Locator("//section[1]/div/table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//section[1]/div/table/thead/tr/th[2]")).ToContainTextAsync("Date Comment Period Closes");
        await Expect(Page.Locator("//section[1]/div/table/thead/tr/th[3]")).ToContainTextAsync("");
        
        // check the number of rows in the second table
        int tableRows2 = await Page.Locator("//section[2]/div/table/tbody/tr").CountAsync();
        Assert.That(tableRows2, Is.EqualTo(3));
        // check the column labels of the first table
        await Expect(Page.Locator("//section[2]/div/table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//section[2]/div/table/thead/tr/th[2]")).ToContainTextAsync("Date Executed");
        await Expect(Page.Locator("//section[2]/div/table/thead/tr/th[3]")).ToContainTextAsync("");
    }
}
