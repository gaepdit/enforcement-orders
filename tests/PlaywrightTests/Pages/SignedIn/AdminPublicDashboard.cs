using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace PlaywrightTests.Pages.SignedIn;

[Parallelizable(ParallelScope.None)]
[TestFixture]
public class AdminPublicDashboard : PageTest
{
    [SuppressMessage("Structure", "NUnit1028:The non-test method is public")]
    public override BrowserNewContextOptions ContextOptions() =>
        new()
        {
            BaseURL = "https://localhost:44331",
            IgnoreHTTPSErrors = true,
        };


    [TearDown]
    public async Task TearDown()
    {
        await LogOutAsync();
    }
    
    private async Task LogOutAsync()
    {
        await Page.GotoAsync("/");
        // The account is signed in when there is an Account button
        var isSignedIn = await Page.Locator("text=Account").CountAsync() != 0;
        if (isSignedIn)
        {
            await Page.GetByRole(AriaRole.Button, new() { NameString = "Account" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { NameString = "Sign out" }).ClickAsync();
            await Page.WaitForURLAsync("/");
        }
    }

    [Test]
    public async Task TestTextPublicDashboard()
    {
        await Page.GotoAsync("/");
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Sign in" }).ClickAsync();
        await Page.WaitForURLAsync("/Account/Login");
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Sign in" }).ClickAsync();
        await Page.WaitForURLAsync("/Admin/Index");
        await Page.GetByRole(AriaRole.Button, new() { NameString = "More" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Public Dashboard" }).ClickAsync();
        await Page.WaitForURLAsync("/");
        
        // check dashboard menus
        await Expect(Page.Locator("nav[role=\"navigation\"]:has-text(\"+ New Order Search More Site Maintenance Users List Public Dashboard Account Vie\")")).ToBeVisibleAsync();

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
