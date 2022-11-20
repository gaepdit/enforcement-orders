using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace PlaywrightTests.Pages.SignedIn;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class AdminIndexPage : PageTest
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
    public async Task TestTextAdminIndexPage()
    {
        await Page.GotoAsync("https://localhost:44331/");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Click sign in tab
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Sign in" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Account/Login");

        // Expect the following text in the home page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Agency Login" })).ToBeVisibleAsync();
        await Expect(Page.GetByText("The Enforcement Orders admin site is a State of Georgia application. It is provi")).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Sign in using your work account" })).ToBeVisibleAsync();

        // click in sign in button
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Sign in" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Index");

        // check the text in the page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Admin Dashboard" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Quick Search" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Currently Published Orders" })).ToBeVisibleAsync();
        await Expect(Page.GetByText("These Enforcement Orders are currently displayed on the Public Website. Date of")).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Proposed Orders" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Executed Orders" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Unpublished Orders" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Pending Orders" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Draft Orders" })).ToBeVisibleAsync();
    }

    [Test]
    public async Task TestTableAdminIndexPage()
    {
        await Page.GotoAsync("https://localhost:44331/");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Click sign in tab
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Sign in" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Account/Login");

        // Expect the following text in the home page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Agency Login" })).ToBeVisibleAsync();
        await Expect(Page.GetByText("The Enforcement Orders admin site is a State of Georgia application. It is provi")).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Sign in using your work account" })).ToBeVisibleAsync();

        // click in sign in button
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Sign in" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Index");

        // check the number of tables
        int numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(4));

        // Proposed Orders
        int tableRows1 = await Page.Locator("//section[1]/div/section[1]/table/tbody/tr").CountAsync();
        Assert.That(tableRows1, Is.EqualTo(3));
        // check the column labels of the first table
        await Expect(Page.Locator("//section[1]/div/section[1]/table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//section[1]/div/section[1]/table/thead/tr/th[2]")).ToContainTextAsync("Date Comment Period Closes");
        await Expect(Page.Locator("//section[1]/div/section[1]/table/thead/tr/th[3]")).ToContainTextAsync("");

        // Executed Orders
        int tableRows2 = await Page.Locator("//section[1]/div/section[2]/table/tbody/tr").CountAsync();
        Assert.That(tableRows2, Is.EqualTo(3));
        // check the column labels of the second table
        await Expect(Page.Locator("//section[1]/div/section[2]/table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//section[1]/div/section[2]/table/thead/tr/th[2]")).ToContainTextAsync("Date Executed");
        await Expect(Page.Locator("//section[1]/div/section[2]/table/thead/tr/th[3]")).ToContainTextAsync("");

        // Pending Orders
        int tableRows3 = await Page.Locator("//section[2]/div/section[1]/table/tbody/tr").CountAsync();
        Assert.That(tableRows3, Is.EqualTo(3));
        // check the column labels of the third table
        await Expect(Page.Locator("//section[2]/div/section[1]/table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//section[2]/div/section[1]/table/thead/tr/th[2]")).ToContainTextAsync("Pending Publication Date");
        await Expect(Page.Locator("//section[2]/div/section[1]/table/thead/tr/th[3]")).ToContainTextAsync("");

        // Draft Orders
        int tableRows4 = await Page.Locator("//section[2]/div/section[2]/table/tbody/tr").CountAsync();
        Assert.That(tableRows4, Is.EqualTo(3));
        // check the column labels of the fourth table 
        await Expect(Page.Locator("//section[2]/div/section[2]/table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//section[2]/div/section[2]/table/thead/tr/th[2]")).ToContainTextAsync("");
    }

}
